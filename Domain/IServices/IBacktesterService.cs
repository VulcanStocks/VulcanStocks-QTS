using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.IServices
{
    public interface IBacktesterService
    {
        Task<BacktestResult> RunBacktest(Func<float, float, StrategyResult> strategy, int balance, int amountOfAssetsToBuy, string symbol, string interval, string apikey);
    }
}