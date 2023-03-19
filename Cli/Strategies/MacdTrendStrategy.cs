using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Indicators;
using Domain;

namespace Cli.Strategies
{
    public class MacdTrendStrategy
    {
        private MacD _macd;
        private Sma _sma;
        private bool _calculateSma { get; set; }

        public MacdTrendStrategy(int macdShortPeriod, int macdLongPeriod, int macdSignalPeriod, int smaPeriod, bool calculateSma)
        {
            _calculateSma = calculateSma;
            _macd = new MacD(macdShortPeriod, macdLongPeriod, macdSignalPeriod);
            _sma = new Sma(smaPeriod);
        }

        public StrategyResult Evaluate(float price, float volume)
        {
            float macd = _macd.TryGetValue(price, volume).Item1;
            float signalLine = _macd.GetLatestSignal();
            float sma = _sma.TryGetValue(price, volume).Item1;

            if (macd < 0 && macd > signalLine)
            {
                if (_calculateSma)
                {
                    if (price > sma)
                    {
                        return StrategyResult.Buy;
                    }
                    else
                    {
                        return StrategyResult.Hold;
                    }


                }
                return StrategyResult.Buy;

            }
            else if (macd > 0 && macd < signalLine)
            {
                if (_calculateSma)
                {
                    if (price < sma)
                    {
                        return StrategyResult.Sell;
                    }
                    else
                    {
                        return StrategyResult.Hold;
                    }


                }
                return StrategyResult.Sell;
            }

            return StrategyResult.Hold;
        }

    }

}