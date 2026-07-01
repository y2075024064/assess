using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.ExcelTool;
using Infrastructure.Utilities;
using Assess.Service.Services;
using Assess.Model.Result;
using Assess.Model.Param;
using Assess.Entity;
using System.Data;
using Repository;
using Mapster;

namespace Assess.WebApi.Controllers.Admin;

/// <summary>
/// 题目管理
/// </summary>
public class TopicController : AdminApiController<TopicInfo, ITopicService>
{
    public TopicController(
        ITopicService entityService) : base(entityService)
    {
    }

    /// <summary>
    /// 分页获取题目列表
    /// </summary>
    /// <param name="param">分页参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestPageResult<TopicPageResult>> Page(TopicPageParam param)
    {
        RequestPageResult<TopicPageResult> result = new();
        EntityPageModel<TopicInfo> pageModel = new(param)
        {
            Where = e => (string.IsNullOrEmpty(param.Keyword) || e.Number == param.Keyword || e.Label!.Contains(param.Keyword) || e.Title!.Contains(param.Keyword) ||
                          e.Purpose!.Contains(param.Keyword) || e.Remark!.Contains(param.Keyword) || e.Content.Contains(param.Keyword)) &&
                         (string.IsNullOrEmpty(param.MemberName) || e.Member!.RealName == param.MemberName) &&
                         (param.UseType == null || e.UseType == param.UseType) &&
                         (param.PartId == null || e.PartId == param.PartId) &&
                         (param.Type == null || e.Type == param.Type) &&
                         e.IsDelete == false
        };

        var page = await entityService.GetPageAsync(e => new TopicPageResult()
        {
            Id = e.Id,
            Type = e.Type,
            Level = e.Level,
            Title = e.Title,
            Number = e.Number,
            Remark = e.Remark,
            UseType = e.UseType,
            Purpose = e.Purpose,
            IsOriginal = e.IsOriginal,
            CommentNum = e.CommentNum,
            CreationTime = e.CreationTime,
            CollectionNum = e.CollectionNum,
            ModificationTime = e.ModificationTime,
            Content = e.Content.Substring(0, 100),
        }, pageModel).NotWaitBack();
        if (page != null) { result.LoadPage(page); }

        return result;
    }

    /// <summary>
    /// 新增题目信息
    /// </summary>
    /// <param name="param">新增的题目信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Insert(TopicInsertParam param)
    {
        RequestResult result = new();

        TopicInfo entity = param.Adapt<TopicInfo>();
        entity.Number = CommonTool.CreateNumber("ST");
        entity.CreationTime = DateTime.Now;
        entity.MemberId = CurrentMemberId!;

        if (param.Label?.Length > 0) { entity.Label = string.Join(',', param.Label); }
        if (param.RcmdGrade?.Length > 0) { entity.RcmdGrade = string.Join(',', param.RcmdGrade); }

        if (!string.IsNullOrWhiteSpace(entity.RightAnswer) && param.Type != TopicType.多选)
        {
            string[] array = param.Type switch
            {
                TopicType.编程 or TopicType.单选 => new[] { entity.RightAnswer },
                TopicType.填空 => entity.RightAnswer.Split('\n'),
                _ => Array.Empty<string>(),
            };
            entity.RightAnswer = array.ToJson();
        }

        using AssessDBContext db = entityService.GetDbContext();
        _ = db.TopicInfos.Add(entity);

        if (param.Options.IsNotEmpty())
        {
            TopicOptions options = new()
            {
                TopicInfo = entity,
                Options = param.Options!,
            };
            _ = db.TopicOptions.Add(options);
        }

        int count = await db.SaveChangesAsync().NotWaitBack();
        if (count > 0) { result.Success("新增成功"); }
        else { result.Fail("新增失败"); }

        return result;
    }

    /// <summary>
    /// 更改题目信息
    /// </summary>
    /// <param name="param">更改的题目信息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> Update(TopicUpdateParam param)
    {
        RequestResult result = new();

        using AssessDBContext db = entityService.GetDbContext();
        var entity = await db.TopicInfos.Include(e => e.TopicOptions)
            .SingleOrDefaultAsync(e => e.Id == param.Id).NotWaitBack();

        if (entity == null) { result.Fail("您要修改的题目不存在"); }
        else
        {
            if (!string.IsNullOrWhiteSpace(param.RightAnswer) && param.Type != TopicType.多选)
            {
                string[] array = param.Type switch
                {
                    TopicType.编程 or TopicType.单选 => new[] { param.RightAnswer },
                    TopicType.填空 => param.RightAnswer.Split('\n'),
                    _ => Array.Empty<string>(),
                };
                param.RightAnswer = array.ToJson();
            }

            bool hasChange = entity.ChangeField(param);

            string? label = null;
            if (param.Label?.Length > 0) { label = string.Join(',', param.Label); }
            if (entity.Label != label) { entity.Label = label; hasChange = true; }

            string? rcmdGrade = null;
            if (param.RcmdGrade?.Length > 0) { rcmdGrade = string.Join(',', param.RcmdGrade); }
            if (entity.RcmdGrade != rcmdGrade) { entity.RcmdGrade = rcmdGrade; hasChange = true; }

            if (param.Options.IsNotEmpty())
            {
                if (entity.TopicOptions != null)
                {
                    if (param.Options != entity.TopicOptions.Options)
                    {
                        entity.TopicOptions.Options = param.Options!;
                        hasChange = true;
                    }
                }
                else
                {
                    TopicOptions options = new()
                    {
                        Options = param.Options!,
                        TopicId = entity.Id,
                    };
                    _ = db.TopicOptions.Add(options);
                    hasChange = true;
                }
            }

            if (hasChange)
            {
                entity.ModificationTime = DateTime.Now;

                int count = await db.SaveChangesAsync().NotWaitBack();
                if (count > 0) { result.Success("更改成功"); }
                else { result.Fail("更改失败"); }
            }
            else { result.Fail("无更改项"); }
        }

        return result;
    }

    /// <summary>
    /// 删除题目信息
    /// </summary>
    /// <param name="id">题目ID</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<RequestResult> Delete(uint id)
    {
        RequestResult result = new();

        int count = await entityService.UpdateAsync(e => e.Id == id, e => new TopicInfo { IsDelete = true }).NotWaitBack();
        if (count > 0) { result.Success("删除成功"); }
        else { result.Fail("删除失败"); }

        return result;
    }

    /// <summary>
    /// 获取题目详情
    /// </summary>
    /// <param name="id">题目ID</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<RequestResult<TopicDetailResult>> Detail(uint id)
    {
        RequestResult<TopicDetailResult> result = new();

        result.Data = await entityService.GetEntityAsync(e => e.Id == id, e => new TopicDetailResult
        {
            Id = e.Id,
            Type = e.Type,
            Label = e.Label,
            Level = e.Level,
            Number = e.Number,
            Remark = e.Remark,
            PartId = e.PartId,
            Origin = e.Origin,
            Purpose = e.Purpose,
            Content = e.Content,
            UseType = e.UseType,
            Analysis = e.Analysis,
            RcmdGrade = e.RcmdGrade,
            PartName = e.Part!.Name,
            IsOriginal = e.IsOriginal,
            OriginType = e.OriginType,
            CommentNum = e.CommentNum,
            RightAnswer = e.RightAnswer,
            OriginNumber = e.OriginNumber,
            CreationTime = e.CreationTime,
            CollectionNum = e.CollectionNum,
            MemberName = e.Member!.RealName,
            Options = e.TopicOptions!.Options,
            ModificationTime = e.ModificationTime,
        }).NotWaitBack();
        if (result.Data == null) { result.Fail("您要获取的题目不存在"); }
        else { result.Success(); }

        return result;
    }

    /// <summary>
    /// 上传Excel文件批量导入
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<RequestResult> UploadExcel()
    {
        RequestResult result = new();
        uint memberId = (uint)CurrentMemberId!;

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
                    List<TopicInfo> entities = new();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        try
                        {
                            TopicInfo topicInfo = CreateTopicFromDataRow(row);
                            topicInfo.MemberId = memberId;
                            entities.Add(topicInfo);
                        }
                        catch (Exception ex)
                        {
                            result.Fail($"第 {i + 2} 行，" + ex.Message);
                            return result;
                        }
                    }

                    List<int> partIds = new();
                    entities.ForEach(e =>
                    {
                        if (e.PartId.HasValue && !partIds.Contains(e.PartId.Value)) partIds.Add(e.PartId.Value);
                    });

                    if (partIds.Count > 0)
                    {
                        List<TopicPart> list = await entityService.Adapter.GetListAsync<TopicPart, TopicPart>(
                            e => partIds.Contains(e.Id), e => new TopicPart { Id = e.Id }).NotWaitBack();
                        if (list.Count != partIds.Count)
                        {
                            var _partIds = list.Select(e => e.Id);
                            _ = partIds.RemoveAll(e => _partIds.Contains(e));
                            result.Fail($"专项ID：{string.Join('、', partIds)} 不存在");
                            return result;
                        }
                    }

                    int count = await entityService.InsertAsync(entities).NotWaitBack();
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

    /// <summary>
    /// 下载Excel模板
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AccreditAction(IgnoreAuth = true)]
    public FileResult DownloadExcel()
    {
        FileStream stream = new("wwwroot/template/topicTemp.xlsx", FileMode.Open, FileAccess.Read);
        return File(stream, "applicable/octet-stream", "TopicInfoTemp.xlsx");
    }

    /// <summary>
    /// 从DataRow中获取数据并创建 题目信息 实例
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private static TopicInfo CreateTopicFromDataRow(DataRow row)
    {
        if (!Enum.TryParse(row["类型"].ToString(), out TopicType type))
        {
            throw new Exception("类型不合法");
        }
        if (!Enum.TryParse(row["用途"].ToString(), out TopicUseType useType))
        {
            throw new Exception("用途不合法");
        }
        if (!decimal.TryParse(row["难度"].ToString(), out decimal level) || level > 1 || level < 0.1m)
        {
            throw new Exception("难度不合法");
        }
        string? label = row["标签"].ToString();
        label = label.IsEmpty() ? null : label;
        if (label != null && !CanConvertStringArray(ref label, out _))
        {
            throw new Exception("标签不合法");
        }
        string? answer = row["答案"].ToString();
        answer = answer.IsEmpty() ? null : answer;
        if (answer == null || !CanConvertStringArray(ref answer, out _))
        {
            throw new Exception("答案不合法");
        }
        string? grades = row["推荐年级"].ToString();
        grades = grades.IsEmpty() ? null : grades;
        if (grades != null && !CanConvertStringArray(ref grades, out _))
        {
            throw new Exception("推荐年级不合法");
        }
        string? content = row["题目内容"].ToString();
        if (content.IsEmpty())
        {
            throw new Exception("题目内容不能为空");
        }
        int? partId = null;
        string? partIdStr = row["专项ID"].ToString();
        if (partIdStr.IsNotEmpty())
        {
            if (!int.TryParse(partIdStr, out int _partId) || _partId <= 0)
            { throw new Exception("专项ID不合法"); }
            else { partId = _partId; }
        }

        TopicOptions? topicOptions = null;
        string? options = row["选项"].ToString();
        if (type == TopicType.单选 || type == TopicType.多选)
        {
            if (options.IsEmpty() || !CanConvertStringArray(ref options, out string[] data))
            {
                throw new Exception("选项不合法");
            }
            else if (data.Length > 0)
            {
                object[] array = new object[data.Length];
                for (int j = 0; j < data.Length; j++)
                {
                    char _label = (char)(65 + j);
                    array[j] = new { Label = _label, Content = data[j] };
                }
                options = array.ToJson();
                topicOptions = new TopicOptions { Options = options };
            }
        }

        string? origin = row["来源"].ToString();
        origin = origin.IsEmpty() ? null : origin;

        string? remark = row["备注"].ToString();
        remark = remark.IsEmpty() ? null : remark;

        string? purpose = row["考察目的"].ToString();
        purpose = purpose.IsEmpty() ? null : purpose;

        string? analysis = row["解析"].ToString();
        analysis = analysis.IsEmpty() ? null : analysis;

        return new TopicInfo()
        {
            Type = type,
            Level = level,
            Label = label,
            PartId = partId,
            Origin = origin,
            Remark = remark,
            UseType = useType,
            Purpose = purpose,
            Content = content!,
            RcmdGrade = grades,
            Analysis = analysis,
            RightAnswer = answer,
            CreationTime = DateTime.Now,
            TopicOptions = topicOptions,
            IsOriginal = origin.IsEmpty(),
            Number = CommonTool.CreateNumber("ST"),
        };
    }

    /// <summary>
    /// 是否可以转换为字符串数字
    /// </summary>
    /// <param name="str"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static bool CanConvertStringArray(ref string? str, out string[] data)
    {
        try
        {
            data = str?.Split(',') ?? Array.Empty<string>();
            if (data.Length == 0) { return false; }
            else
            {
                str = data.ToJson();
                return true;
            }
        }
        catch
        {
            data = Array.Empty<string>();
            return false;
        }
    }
}
