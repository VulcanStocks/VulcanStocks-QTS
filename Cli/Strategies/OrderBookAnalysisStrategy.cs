using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.Services.TradeEngineService;

namespace Cli.Strategies
{
    class OrderBookAnalysisStrategy
    {
        public float BidAskSpreadThreshold { get; set; } = 0.01f; // Ange en tröskel för bid-ask spread
        public float VolumeThreshold { get; set; } = 1000f; // Ange en tröskel för handelsvolym

        public StrategyResult Analyze(float bidPrice, float askPrice, float volume)
        {
            float bidAskSpread = askPrice - bidPrice;

            if (bidAskSpread >= BidAskSpreadThreshold && volume >= VolumeThreshold)
            {
                return StrategyResult.Buy;
            }
            else if (bidAskSpread <= -BidAskSpreadThreshold && volume >= VolumeThreshold)
            {
                return StrategyResult.Sell;
            }
            else
            {
                return StrategyResult.Hold;
            }
        }
    }
}