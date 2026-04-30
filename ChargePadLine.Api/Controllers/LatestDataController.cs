using Microsoft.AspNetCore.Mvc;
using X509Data.ChargePadLine.Api.Dtos;
using X509Data.ChargePadLine.Api.Infrastructure;
using X509Data.ChargePadLine.Api.Responses;
using X509Data.ChargePadLine.Api.Services;

namespace X509Data.ChargePadLine.Api.Controllers
{
  /// <summary>
  /// 最新数据接收控制器
  /// </summary>
  [ApiController]
  public class LatestDataController : BaseController
  {
    private readonly ILatestDataService _latestDataService;

    public LatestDataController(ILatestDataService latestDataService)
    {
      _latestDataService = latestDataService;
    }

    /// <summary>
    /// 接收 MES 系统推送的最新站点数据
    /// </summary>
    /// <param name="request">包含站点编号和数据值的请求对象</param>
    /// <returns>操作结果</returns>
    [HttpPost]
    public async Task<IActionResult> ReceiveLatestData([FromBody] LatestDataRequestDto request)
    {
      try
      {
        if (request.StationId == Guid.Empty)
        {
          return Ok(new { success = false, message = "站点编号不能为空" });
        }

        if (string.IsNullOrWhiteSpace(request.Data))
        {
          return Ok(new { success = false, message = "数据值不能为空" });
        }

        var result = await _latestDataService.UpsertLatestDataAsync(request);

        if (result)
        {
          return Ok(new { success = true, message = "数据更新成功" });
        }
        else
        {
          return Ok(new { success = false, message = "未找到对应站点" });
        }
      }
      catch (Exception ex)
      {
        return Ok(new { success = false, message = $"数据更新失败：{ex.Message}" });
      }
    }
  }
}