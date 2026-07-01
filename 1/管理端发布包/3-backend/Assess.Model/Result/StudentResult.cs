namespace Assess.Model.Result;

/// <summary>
/// 学生分页项信息模型
/// </summary>
public class StudentPageItem
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// 学生账号
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 学生姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// 学校名称
    /// </summary>
    public string? SchoolName { get; set; }

    /// <summary>
    /// 学校ID
    /// </summary>
    public int? SchoolId { get; set; }

    /// <summary>
    /// 学生学级
    /// </summary>
    public int GradeNum { get; set; }

    /// <summary>
    /// 学生班级
    /// </summary>
    public int ClassNum { get; set; }

    /// <summary>
    /// 当前年级
    /// </summary>
    public string? CurrentGrade { get; set; }
}
