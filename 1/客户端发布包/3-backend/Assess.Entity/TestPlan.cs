using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

[Table("ta_test_plan")]
public class TestPlan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 题目专项ID
    /// </summary>
    public int PartId { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 答题时长（秒）
    /// </summary>
    public int? TestTime { get; set; }

    /// <summary>
    /// 题目数量
    /// </summary>
    public int TopicNumber { get; set; }

    [ForeignKey(nameof(PartId))]
    public virtual TopicPart? TopicPart { get; set; }

    public virtual List<TestPlanItem> Items { get; set; } = new List<TestPlanItem>();
}
