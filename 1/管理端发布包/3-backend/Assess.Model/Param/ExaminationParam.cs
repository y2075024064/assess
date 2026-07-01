using System.ComponentModel.DataAnnotations;
using Repository;

namespace Assess.Model.Param;

/// <summary>
/// 考试分页参数模型
/// </summary>
public class ExaminationPageParam : DefaultEntityPageParam
{
    /// <summary>
    /// 考试编号
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// 是否开放考试
    /// </summary>
    public bool? IsPublic { get; set; }

    /// <summary>
    /// 开始日期范围
    /// </summary>
    public DateOnly? StartTime { get; set; }

    /// <summary>
    /// 结束日期范围
    /// </summary>
    public DateOnly? EndTime { get; set; }
}

/// <summary>
/// 考试信息添加参数模型
/// </summary>
public class ExaminationInsertParam
{
    /// <summary>
    /// 考试名称
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 考试目的
    /// </summary>
    [MaxLength(128)]
    public string? Purpose { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    [Required]
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 考试开始时间
    /// </summary>
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 考试时长
    /// </summary>
    [Required]
    public uint Duration { get; set; }

    /// <summary>
    /// 参考对象
    /// </summary>
    [MaxLength(128)]
    public string? ReferenceObj { get; set; }

    /// <summary>
    /// 考试开始日期
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 考试结束日期
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// 是否开放考试
    /// </summary>
    [Required]
    public bool IsPublic { get; set; }

    /// <summary>
    /// 主办方
    /// </summary>
    [MaxLength(128)]
    public string? Organizer { get; set; }

    /// <summary>
    /// 协办方
    /// </summary>
    [MaxLength(128)]
    public string? CoOrganizer { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(255)]
    public string? Remark { get; set; }
}

/// <summary>
/// 考试信息更改参数模型
/// </summary>
public class ExaminationUpdateParam : ExaminationInsertParam
{
    /// <summary>
    /// 考试ID
    /// </summary>
    [Required]
    public int Id { get; set; }
}
