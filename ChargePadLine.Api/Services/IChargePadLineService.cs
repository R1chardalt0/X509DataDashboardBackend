using X509Data.ChargePadLine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X509Data.ChargePadLine.Api.Dtos;

namespace X509Data.ChargePadLine.Api.Services
{
  public interface IChargePadLineService
  {
    Task<List<StationList>> GetStationListAsync();
    Task<DataList?> GetDataListByStationAsync(DataSearchDto searchDto);
  }
}
