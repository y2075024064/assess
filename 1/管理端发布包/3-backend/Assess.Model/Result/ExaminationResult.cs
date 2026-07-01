namespace Assess.Model.Result;

/// <summary>
/// 考试分页项信息模型
/// </summary>
public class ExaminationPageItem
{
    /// <summary>
    /// 考试ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 考试编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 考试名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 考试目的
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 考试开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 考试时长
    /// </summary>
    public uint Duration { get; set; }

    /// <summary>
    /// 参考对象
    /// </summary>
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
    public bool IsPublic { get; set; }

    /// <summary>
    /// 主办方
    /// </summary>
    public string? Organizer { get; set; }

    /// <summary>
    /// 协办方
    /// </summary>
    public string? CoOrganizer { get; set; }

    /// <summary>
    /// 报名人数
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 考试列表项信息模型
/// </summary>
public class ExaminationListItem
{
    /// <summary>
    /// 考试ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 考试编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 考试名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 主办方
    /// </summary>
    public string? Organizer { get; set; }

    /// <summary>
    /// 考试开始日期
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 考试结束日期
    /// </summary>
    public DateOnly? EndDate { get; set; }
}

/// <summary>
/// 考试详情信息模型
/// </summary>
public class ExaminationDetail
{
    /// <summary>
    /// 考试ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 考试编号
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 考试名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 考试目的
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 考试开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 考试时长
    /// </summary>
    public uint Duration { get; set; }

    /// <summary>
    /// 参考对象
    /// </summary>
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
    /// 主办方
    /// </summary>
    public string? Organizer { get; set; }

    /// <summary>
    /// 协办方
    /// </summary>
    public string? CoOrganizer { get; set; }
}