using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X509Data.ChargePadLine.Api.Dtos;
using X509Data.ChargePadLine.Api.Entities;
using X509Data.ChargePadLine.Api.Infrastructure;

namespace X509Data.ChargePadLine.Api.Services
{
  public class LatestDataService : ILatestDataService
  {
    private readonly AppDbContext _dbContext;
    private readonly ILogger<LatestDataService> _logger;

    public LatestDataService(AppDbContext dbContext, ILogger<LatestDataService> logger)
    {
      _dbContext = dbContext;
      _logger = logger;
    }

    public async Task<bool> UpsertLatestDataAsync(LatestDataRequestDto request)
    {
      try
      {
        var station = await _dbContext.StationLists
            .FirstOrDefaultAsync(s => s.StationId == request.StationId);

        if (station == null)
        {
          _logger.LogWarning("未找到对应站点，传入的StationId: {StationId}", request.StationId);
          var allStations = await _dbContext.StationLists.ToListAsync();
          _logger.LogInformation("数据库中所有站点: {@Stations}", allStations.Select(s => new { s.StationId, s.StationCode, s.StationName }));
          return false;
        }

        var existingData = await _dbContext.DataLists
            .Where(x => x.StationListId == station.StationId)
            .OrderByDescending(x => x.CreateTime)
            .FirstOrDefaultAsync();

        if (existingData == null)
        {
          var newData = new DataList
          {
            DataId = Guid.NewGuid(),
            StationListId = station.StationId,
            SNnumber = request.SNnumber,
            Data = request.Data,
            CreateTime = request.CreateTime ?? DateTimeOffset.UtcNow
          };
          _dbContext.DataLists.Add(newData);
        }
        else
        {
          existingData.SNnumber = request.SNnumber;
          existingData.Data = request.Data;
          existingData.CreateTime = DateTimeOffset.UtcNow;
          _dbContext.DataLists.Update(existingData);
        }

        await _dbContext.SaveChangesAsync();
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "UpsertLatestDataAsync 执行失败，StationId: {StationId}", request.StationId);
        return false;
      }
    }
  }
}