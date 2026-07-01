using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assess.Entity;

[Table("ta_tag_record")]
[Index(nameof(Name), Name = "UQ_TagRecord_Name", IsUnique = true)]
public class TagRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// 标签名称
    /// </summary>
    [StringLength(64)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 使用次数
    /// </summary>
    public uint UseNum { get; set; }
}
