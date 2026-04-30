using X509Data.ChargePadLine.Api.Dtos;

namespace X509Data.ChargePadLine.Api.Services
{
  public interface ILatestDataService
  {
    Task<bool> UpsertLatestDataAsync(LatestDataRequestDto request);
  }
}