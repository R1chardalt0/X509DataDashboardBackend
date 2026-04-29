using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Data.ChargePadLine.Api.Infrastructure;

namespace X509Data.ChargePadLine.Api.Entities
{

  [Table("X509mes_station_list")]
  public class StationList : BaseEntity
  {


    [Key]
    public Guid StationId { get; set; }

    /// <summary>
    /// 站点名称
    /// </summary>
    public string StationName { get; set; } = "";
    /// <summary>
    /// 站点编号
    /// </summary>

    public string StationCode { get; set; } = "";
  }
}
