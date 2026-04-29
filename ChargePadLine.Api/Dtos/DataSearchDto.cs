using X509Data.ChargePadLine.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X509Data.ChargePadLine.Api.Dtos
{
  public class DataSearchDto
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
