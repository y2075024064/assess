using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assess.Entity;

/// <summary>
/// 模拟测评组卷方案子项
/// </summary>
[Table("ta_test_plan_item")]
public class TestPlanItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 模拟测评组卷方案ID
    /// </summary>
    public int PlanId { get; set; }

    /// <summary>
    /// 子题目专项ID
    /// </summary>
    public int PartId { get; set; }

    /// <summary>
    /// 平均难度
    /// </summary>
    public decimal AvgLevel { get; set; }

    [ForeignKey(nameof(PlanId))]
    public virtual TestPlan? TestPlan { get; set; }

    [ForeignKey(nameof(PartId))]
    public virtual TopicPart? TopicPart { get; set; }
}
