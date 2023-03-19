using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IHistoricalDataService
    {
        Task<List<HistoricalDataModel>> GetDataListAsync(string symbol, string interval, string apikey);

    }
}