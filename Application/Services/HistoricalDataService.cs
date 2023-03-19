using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Domain.IServices;
using Newtonsoft.Json;

namespace Application.Services
{
    public class HistoricalDataService : IHistoricalDataService
    {
        private readonly HttpClient _httpClient;

        public HistoricalDataService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<HistoricalDataModel>> GetDataListAsync(string symbol, string interval, string apikey)
        {
            string queryUrl = BuildQueryUrl(symbol, interval, apikey);
            var jsonData = await FetchJsonDataAsync(queryUrl);

            return ExtractHistoricalData(jsonData);
        }

        private string BuildQueryUrl(string symbol, string interval, string apikey)
        {
            return $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval={interval}&apikey={apikey}";
        }

        private async Task<dynamic> FetchJsonDataAsync(string queryUrl)
        {
            var response = await _httpClient.GetAsync(queryUrl);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content);
        }

        private List<HistoricalDataModel> ExtractHistoricalData(dynamic jsonData)
        {
            var timeSeries = jsonData["Time Series (5min)"];
            List<HistoricalDataModel> historicalDataList = new List<HistoricalDataModel>();

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

            return historicalDataList;
        }
    }
}
