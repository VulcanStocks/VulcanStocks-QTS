using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Domain.IServices;
using Newtonsoft.Json;

namespace Application.Services
{
    public class HistoricalDataService : IHistoricalDataService
    {
        public List<HistoricalDataModel> GetDataList(string symbol, string interval, string apikey)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval={interval}&apikey={apikey}";
            Uri queryUri = new Uri(QUERY_URL);
            List<HistoricalDataModel> historicalDataList = new List<HistoricalDataModel>();

            using (WebClient client = new WebClient())
            {
                dynamic jsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(client.DownloadString(queryUri));

                var timeSeries = jsonData["Time Series (5min)"];

                foreach (var timestamp in timeSeries)
                {
                    float closePrice = float.Parse(timestamp.Value["4. close"].Value, CultureInfo.InvariantCulture);
                    float volume = float.Parse(timestamp.Value["5. volume"].Value, CultureInfo.InvariantCulture);

                    HistoricalDataModel historicalData = new HistoricalDataModel
                    {
                        Price = closePrice,
                        Volume = volume
                    };

                    historicalDataList.Add(historicalData);
                }
            }

            return historicalDataList;
        }

    }

}