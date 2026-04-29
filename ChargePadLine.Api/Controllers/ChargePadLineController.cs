using Microsoft.AspNetCore.Mvc;
using X509Data.ChargePadLine.Api.Entities;
using X509Data.ChargePadLine.Api.Services;
using X509Data.ChargePadLine.Api.Dtos;
using X509Data.ChargePadLine.Api.Infrastructure;
using X509Data.ChargePadLine.Api.Responses;

namespace X509Data.ChargePadLine.Api.Controllers
{
  /// <summary>
  /// 充电板生产线控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class ChargePadLineController : BaseController
  {
    private readonly IChargePadLineService _chargePadLineService;

    public ChargePadLineController(IChargePadLineService chargePadLineService)
    {
      _chargePadLineService = chargePadLineService;
    }

    /// <summary>
    /// 获取所有站点列表
    /// </summary>
    /// <returns>站点列表</returns>
    [HttpGet]
    public async Task<Resp<List<StationList>>> GetStationList()
    {
      try
      {
        var stations = await _chargePadLineService.GetStationListAsync();
        return RespExtensions.MakeSuccess(stations);
      }
      catch (Exception ex)
      {
        return RespExtensions.MakeFail<List<StationList>>("500", $"获取站点列表失败：{ex.Message}");
      }
    }

    /// <summary>
    /// 根据站点获取数据记录列表
    /// </summary>
    /// <param name="searchDto">查询参数</param>
    /// <returns>数据记录</returns>
    [HttpPost]
    public async Task<Resp<DataList>> GetDataListByStation(DataSearchDto searchDto)
    {
      try
      {
        if (searchDto?.StationCode == null)
        {
          return RespExtensions.MakeFail<DataList>("400", "站点信息不能为空");
        }

        if (string.IsNullOrWhiteSpace(searchDto.StationId.ToString()))
        {
          return RespExtensions.MakeFail<DataList>("400", "站点编码不能为空");
        }

        var data = await _chargePadLineService.GetDataListByStationAsync(searchDto);

        return RespExtensions.MakeSuccess(data!);
      }
      catch (Exception ex)
      {
        return RespExtensions.MakeFail<DataList>("500", $"获取数据失败：{ex.Message}");
      }
    }
  }
}
