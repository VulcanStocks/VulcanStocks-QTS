using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Domain;
using Domain.IServices;
using Domain.Models;

namespace Application.Services
{
    public class HistoricalDataService : IHistoricalDataService
    {
        private readonly HttpClient _httpClient;

        public HistoricalDataService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<HistoricalDataModel>>
        GetDataListAsync(string symbol, string interval, string apikey)
        {
            string queryUrl = BuildQueryUrl(symbol, interval, apikey);
            string csvData = await FetchCsvDataAsync(queryUrl);
            return ParseCsvData(csvData);
        }

        private string
        BuildQueryUrl(string symbol, string interval, string apikey)
        {
            return $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY_EXTENDED&symbol={
                symbol}&interval={interval}&apikey={apikey}";
        }

        private async Task<string> FetchCsvDataAsync(string queryUrl)
        {
            var response = await _httpClient.GetAsync(queryUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private List<HistoricalDataModel> ParseCsvData(string csvData)
        {
            using (var reader = new StringReader(csvData))
            using (
                var csv =
                    new CsvReader(reader,
                        new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = true,
                            IgnoreBlankLines = true,
                            BadDataFound = null
                        })
            )
            {
                csv.Context.RegisterClassMap<HistoricalDataModelClassMap>();

                return csv.GetRecords<HistoricalDataModel>().ToList();
            }
        }
    }

    public class HistoricalDataModelClassMap : ClassMap<HistoricalDataModel>
    {
        public HistoricalDataModelClassMap()
        {
            Map(m => m.Price).Name("close");
            Map(m => m.Volume).Name("volume");
        }
    }
}
