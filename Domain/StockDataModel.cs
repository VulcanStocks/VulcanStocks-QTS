using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class StockResponse
    {
        public List<StockDataModel> data { get; set; }
        public string type { get; set; }
    }

    public class StockDataModel
    {
        public float p { get; set; }
        public float v { get; set; }
    }
}