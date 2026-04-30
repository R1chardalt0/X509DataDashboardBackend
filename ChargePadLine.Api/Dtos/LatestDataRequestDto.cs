using System.ComponentModel.DataAnnotations;

namespace X509Data.ChargePadLine.Api.Dtos
{
  /// <summary>
  /// MES 系统推送最新数据请求DTO
  /// </summary>
  public class LatestDataRequestDto
  {
    /// <summary>
    /// 站点编号
    /// </summary>
    [Required(ErrorMessage = "站点编号不能为空")]
    public Guid StationId { get; set; }

    /// <summary>
    /// 数据值
    /// </summary>
    [Required(ErrorMessage = "数据值不能为空")]
    public string Data { get; set; } = "";

    /// <summary>
    /// 序列号
    /// </summary>
    public string SNnumber { get; set; } = "";

    /// <summary>
    /// 创建时间（由 MES 系统传入）
    /// </summary>
    public DateTimeOffset? CreateTime { get; set; }
  }
}