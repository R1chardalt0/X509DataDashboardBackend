using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X509Data.ChargePadLine.Api.Infrastructure;
using X509Data.ChargePadLine.Api.Entities;
using X509Data.ChargePadLine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Data.ChargePadLine.Api.Dtos;

namespace X509Data.ChargePadLine.Api.Services
{
  public class ChargePadLineService : IChargePadLineService
  {
    private readonly IRepository<DataList> _dataListRepo;
    private readonly IRepository<StationList> _stationListRepo;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ChargePadLineService> _logger;

    public ChargePadLineService(IRepository<DataList> dataListRepo, IRepository<StationList> stationListRepo, AppDbContext dbContext, ILogger<ChargePadLineService> logger)
    {
      _dataListRepo = dataListRepo;
      _stationListRepo = stationListRepo;
      _dbContext = dbContext;
      _logger = logger;
    }

    public async Task<List<StationList>> GetStationListAsync()
    {
      return await _stationListRepo.GetListAsync();
    }

    public async Task<DataList?> GetDataListByStationAsync(DataSearchDto searchDto)
    {
      var stationId = searchDto?.StationId;
      if (stationId == null)
      {
        _logger.LogWarning("获取设备信息失败，站点编码不能为空");
        return null;
      }

      return await _dbContext.DataLists
          .Where(x => x.StationListId == stationId)
          .OrderByDescending(x => x.CreateTime)
          .FirstOrDefaultAsync();
    }
  }
}
