using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BacktestResult
    {
        public float WinLossRatio { get; set; }
        public int TradesTaken { get; set; }
        public int AmountOfTests { get; set; }
        public float Balance { get; set; }
        public float Profit { get; set; }
    }
}