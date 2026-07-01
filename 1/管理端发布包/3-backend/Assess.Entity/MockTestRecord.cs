using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

/// <summary>
/// 模拟评测记录表
/// </summary>
[Table("ta_mock_test_record")]
[Index(nameof(Number), Name = "UQ_MockTestRecord_Number", IsUnique = true)]
public class MockTestRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 模拟评测编号
    /// </summary>
    [Column(TypeName = "char(12)")]
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// 用户ID
    /// </summary>
    public uint MemberId { get; set; }

    /// <summary>
    /// 专项ID
    /// </summary>
    public int? PartId { get; set; }

    /// <summary>
    /// 题目信息数组
    /// </summary>
    [Column(TypeName = "json")]
    public string TopicInfos { get; set; } = string.Empty;

    /// <summary>
    /// 已回答题目索引数组
    /// </summary>
    [Column(TypeName = "json")]
    public string? AnsweredIndexs { get; set; }

    /// <summary>
    /// 错误题目索引数组
    /// </summary>
    [Column(TypeName = "json")]
    public string? ErrorIndexs { get; set; }

    /// <summary>
    /// 是否评测结束
    /// </summary>
    public bool IsOver { get; set; }

    /// <summary>
    /// 平均难度
    /// </summary>
    public decimal AvgDifficulty { get; set; } = 0.6m;

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; } = 100.00m;

    /// <summary>
    /// 当前得分
    /// </summary>
    public decimal CurrScore { get; set; }

    /// <summary>
    /// 当前答题位置
    /// </summary>
    public int CurrPostion { get; set; }

    /// <summary>
    /// 答题记录
    /// </summary>
    [Column(TypeName = "json")]
    public string? AnswerRecord { get; set; }

    /// <summary>
    /// 剩余时间（秒）
    /// </summary>
    public int? RemainTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 答题结束时间
    /// </summary>
    public DateTime? OverTime { get; set; }

    /// <summary>
    /// 年级
    /// </summary>
    [MaxLength(16)]
    public string? Grade { get; set; }

    [ForeignKey(nameof(MemberId))]
    public virtual Member? Member { get; set; }

    [ForeignKey(nameof(PartId))]
    public virtual TopicPart? TopicPart { get; set; }
}
