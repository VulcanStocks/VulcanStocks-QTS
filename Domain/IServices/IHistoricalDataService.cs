using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface IHistoricalDataService
    {
        List<HistoricalDataModel> GetDataList(string symbol, string interval, string apikey);   

    }
}