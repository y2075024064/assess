using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Infrastructure.HashTools;
using Infrastructure.Utilities;
using Infrastructure.ExcelTool;
using Microsoft.AspNetCore.Mvc;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using System.Data;
using Repository;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 学生管理相关接口
/// </summary>
public class StudentController : AdminApiController<StudentInfo, IStudentService>
{
    public StudentController(IStudentService entityService) : base(entityService)
    {
    }

    #region 分页

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<StudentPageItem>> Page(StudentPageParam param)
    {
        RequestPageResult<StudentPageItem> result = new();

        EntityPageModel<StudentInfo> entityParam = new(param)
        {
            Where = e =>
                (param.SchoolId == null || e.SchoolId == param.SchoolId) &&
                (param.Gender == null || e.Member!.Gender == param.Gender) &&
                (string.IsNullOrEmpty(param.Grade) || e.CurrentGrade == param.Grade) &&
                (string.IsNullOrEmpty(param.Keyword) ||
                 e.Member!.RealName!.Contains(param.Keyword) ||
                 e.Member!.MemberAccount!.Account!.Contains(param.Keyword))
        };

        using var qdb = entityService.GetDbContext();

        // 直接编写 LEFT JOIN 查询，避免 INNER JOIN 过滤掉无学校的学生
        var query = from si in qdb.StudentInfos
                    where si.Member != null && si.Member.MemberAccount != null
                    join sc in qdb.SchoolInfos on si.SchoolId equals sc.Id into scJoin
                    from sc in scJoin.DefaultIfEmpty()
                    select new StudentPageItem
                    {
                        Id = si.MemberId,
                        ClassNum = si.ClassNum,
                        GradeNum = si.GradeNum,
                        SchoolId = si.SchoolId,
                        CurrentGrade = si.CurrentGrade,
                        SchoolName = sc != null ? sc.Name : "",
                        Name = si.Member!.RealName ?? "",
                        Gender = si.Member.Gender,
                        Account = si.Member.MemberAccount!.Account,
                    };

        int page = entityParam.Param.Page;
        int take = entityParam.Param.Take;
        int total = await query.CountAsync().NotWaitBack();
        var data = await query.OrderBy(e => e.Id)
            .Skip(take * (page - 1))
            .Take(take)
            .ToListAsync().NotWaitBack();

        result.Total = total;
        result.Data = data.ToArray();
        result.Success("获取成功");
        return result;
    }

    #endregion

    #region 新增

    /// <summary>
    /// 新增信息
    /// </summary>
    /// <param name="param">新增参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(StudentInsertParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        var msg = InsertStudent(param, db);

        if (msg != null) { result.Fail(msg); }
        else
        {
            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("添加成功"); }
            else { result.Fail("添加失败"); }
        }

        return result;
    }

    #endregion

    #region 修改

    /// <summary>
    /// 修改信息
    /// </summary>
    /// <param name="param">修改参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(StudentUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();

        string account = CreateAccount(param.SchoolId, param.GradeNum, param.ClassNum, param.No);
        int count = await db.MemberAccounts.CountAsync(e => e.Account == account && e.MemberId != param.Id).NotWaitBack();

        if (count > 0) { result.Fail("该学生序号已添加过"); }
        else
        {
            StudentInfo? student = await db.StudentInfos.Include(e => e.Member!.MemberAccount)
                .SingleOrDefaultAsync(e => e.MemberId == param.Id);

            if (student == null) { result.Fail("您要更改的对象不存在"); }
            else
            {
                MemberAccount memberAccount = student.Member!.MemberAccount!;
                if (memberAccount.Account != account)
                {
                    string sign = StringHashTool.Signature();
                    EncodeType encodeType = EnumHelper.Random(EncodeType.None);
                    string pwd = StringHashTool.Encode(account, encodeType, sign);

                    memberAccount.EncodeType = encodeType;
                    memberAccount.Account = account;
                    memberAccount.Signature = sign;
                    memberAccount.Password = pwd;

                    if (student.RegistGrade != param.CurrentGrade)
                    { student.RegistGrade = param.CurrentGrade; }
                    student.ChangeField(param);
                }

                Member member = student.Member;
                if (member.RealName != param.Name)
                { member.RealName = param.Name; }
                if (member.Gender != param.Gender)
                { member.Gender = param.Gender; }

                count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("修改成功"); }
                else { result.Fail("修改失败"); }
            }
        }

        return result;
    }

    #endregion

    #region 删除

    /// <summary>
    /// 删除信息
    /// </summary>
    /// <param name="ids">用户ID集合</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(uint[] ids)
    {
        RequestResult result = new();

        try
        {
            using AssessDBContext db = entityService.GetDbContext();

            var list = await db.StudentInfos.Include(e => e.Member!.MemberAccount)
                .Where(e => ids.Contains(e.MemberId)).ToListAsync().NotWaitBack();

            foreach (var item in list)
            {
                var member = item.Member!;
                var account = member.MemberAccount!;

                _ = db.Remove(item);
                _ = db.Remove(account);
                _ = db.Remove(member);
            }

            int count = await db.SaveChangesAsync().NotWaitBack();
            if (count > 0) { result.Success("删除成功"); }
            else { result.Fail("删除失败"); }
        }
        catch (Exception ex)
        {
            result.Fail(ex.Message);
        }

        return result;
    }

    #endregion

    #region 批量导入

    /// <summary>
    /// 下载Excel模板
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AccreditAction(IgnoreAuth = true)]
    public FileResult DownloadExcel()
    {
        FileStream stream = new("wwwroot/template/studentTemp.xlsx", FileMode.Open, FileAccess.Read);
        return File(stream, "applicable/octet-stream", "StudentInfoTemp.xlsx");
    }

    /// <summary>
    /// 上传Excel文件批量导入
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> UploadExcel()
    {
        RequestResult result = new();

        var files = Request.Form.Files;
        if (files?.Count > 0)
        {
            IFormFile file = files[0];
            Stream stream = file.OpenReadStream();

            if (stream.IsExcelWithXlsx(file.FileName))
            {
                Excel excel = new();
                DataTable dt = excel.ReadExcelToDataTable(stream);
                if (dt.Rows.Count > 0)
                {
                    using var db = entityService.GetDbContext();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        try
                        {
                            StudentInsertParam param = CreateStudentFromDataRow(row);
                            var msg = InsertStudent(param, db);
                            if (msg != null) throw new Exception(msg);
                        }
                        catch (Exception ex)
                        {
                            result.Fail($"第 {i + 2} 行，" + ex.Message);
                            return result;
                        }
                    }

                    int count = await db.SaveChangesAsync().NotWaitBack();
                    if (count > 0) { result.Success("导入成功"); }
                    else { result.Fail("导入失败"); }
                }
                else { result.Fail("您未导入任何内容"); }
            }
            else { result.Fail("您上传的文件不合法"); }
        }
        else { result.Fail("您未上传任何文件"); }

        return result;
    }

    #endregion

    /// <summary>
    /// 添加学生信息
    /// </summary>
    /// <param name="param"></param>
    /// <param name="db"></param>
    /// <returns></returns>
    private string? InsertStudent(StudentInsertParam param, AssessDBContext db)
    {
        string account = CreateAccount(param.SchoolId, param.GradeNum, param.ClassNum, param.No);
        int count = db.MemberAccounts.Count(e => e.Account == account);

        if (count > 0) { return "该学生序号已添加过"; }
        else
        {
            Member member = new()
            {
                Gender = param.Gender,
                RealName = param.Name,
                Type = MemberType.学生,
                CreationTime = DateTime.Now,
                Nickname = CommonTool.CreateNickname(),
            };
            _ = db.Members.Add(member);

            string sign = StringHashTool.Signature();
            EncodeType encodeType = EnumHelper.Random(EncodeType.None);
            string pwd = StringHashTool.Encode(account, encodeType, sign);
            MemberAccount memberAccount = new()
            {
                Password = pwd,
                Member = member,
                Signature = sign,
                Account = account,
                EncodeType = encodeType,
                CanModifyAccount = false,
            };
            _ = db.MemberAccounts.Add(memberAccount);

            StudentInfo student = new()
            {
                Member = member,
                SchoolId = param.SchoolId,
                ClassNum = param.ClassNum,
                GradeNum = param.GradeNum,
                RegistGrade = param.CurrentGrade,
                CurrentGrade = param.CurrentGrade,
            };
            _ = db.StudentInfos.Add(student);

            return null;
        }
    }

    /// <summary>
    /// 创建用户账户
    /// </summary>
    /// <param name="schoolId">学校ID</param>
    /// <param name="gradeNum">学级号</param>
    /// <param name="classNum">班级号</param>
    /// <param name="no">序号</param>
    /// <returns></returns>
    private static string CreateAccount(int schoolId, int gradeNum, int classNum, int no)
    {
        char one = (char)((schoolId / 26) + 'A');
        char two = (char)((schoolId % 26) + 'A');
        string three = gradeNum.ToString()[^2..];
        string four = classNum.ToString().PadLeft(2, '0');
        string five = no.ToString().PadLeft(2, '0');

        return $"{one}{two}{three}{four}{five}";
    }

    /// <summary>
    /// 从DataRow中获取数据并创建 学生信息 实例
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private static StudentInsertParam CreateStudentFromDataRow(DataRow row)
    {
        if (!int.TryParse(row["学校ID"].ToString(), out int schoolId))
        {
            throw new Exception("学校ID不合法");
        }
        if (!int.TryParse(row["学级"].ToString(), out int gradeNum))
        {
            throw new Exception("学级不合法");
        }
        if (!int.TryParse(row["班级"].ToString(), out int classNum))
        {
            throw new Exception("班级不合法");
        }
        if (!int.TryParse(row["序号"].ToString(), out int no))
        {
            throw new Exception("序号不合法");
        }
        if (!Enum.TryParse(row["性别"].ToString(), out GenderType gender))
        {
            throw new Exception("性别不合法");
        }

        string? name = row["姓名"].ToString();
        if (name.IsEmpty())
        {
            throw new Exception("姓名不能为空");
        }

        string? grade = row["年级"].ToString();
        if (grade.IsEmpty())
        {
            throw new Exception("年级不能为空");
        }

        return new StudentInsertParam()
        {
            No = no,
            Name = name!,
            Gender = gender,
            ClassNum = classNum,
            GradeNum = gradeNum,
            SchoolId = schoolId,
            CurrentGrade = grade!,
        };
    }
}
