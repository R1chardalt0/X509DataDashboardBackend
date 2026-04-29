using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Data.ChargePadLine.Api.Infrastructure;

namespace X509Data.ChargePadLine.Api.Entities
{
  [Table("X509Data")]
  public class DataList
  {
    /// <summary>
    /// 状态ID
    /// </summary>
    [Description("状态ID")]
    [Key]
    [Column("DataId")]
    public Guid DataId { get; set; }

    /// <summary>
    /// 序列号/产品唯一码
    /// </summary>
    [Description("序列号/产品唯一码")]
    [Column("SNnumber")]
    public string SnNumber { get; set; } = "";

    /// <summary>
    /// 站点外键ID
    /// </summary>
    [Description("站点外键ID")]
    public Guid? StationListId { get; set; }

    /// <summary>
    /// 站点导航属性
    /// </summary>
    [ForeignKey("StationListId")]
    public StationList? StationList { get; set; }

    /// <summary>
    /// 测试数据
    /// </summary>
    [Description("测试数据")]
    public string? Data { get; set; }

    /// <summary>
    /// 返工时间
    /// </summary>
    [Description("创建时间")]
    public DateTimeOffset? CreateTime { get; set; }
  }
}
