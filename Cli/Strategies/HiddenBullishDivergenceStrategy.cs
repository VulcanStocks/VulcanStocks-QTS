using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Indicators;

namespace Cli.Strategies
{
    public class HiddenBullishDivergenceStrategy
    {
        private readonly Rsi _rsi;
        private readonly Sma _sma;
        private float _prevPrice;
        private (float, bool) _prevRsi;
        private int _confirmationPeriod;

        public HiddenBullishDivergenceStrategy(int rsiPeriod, int smaPeriod, int confirmationPeriod)
        {
            _rsi = new Rsi(rsiPeriod);
            _sma = new Sma(smaPeriod);
            _confirmationPeriod = confirmationPeriod;
        }

        public bool CheckHiddenBullishDivergence(float price)
        {
            var rsiValue = _rsi.TryGetValue(price, 0);
            var smaValue = _sma.TryGetValue(price, 0);

            if (!_prevRsi.Item2 || !rsiValue.Item2 || !smaValue.Item2)
            {
                _prevRsi = rsiValue;
                _prevPrice = price;
                return false;
            }

            bool hiddenBullishDivergence = _prevPrice < price && _prevRsi.Item1 > rsiValue.Item1;
            _prevRsi = rsiValue;
            _prevPrice = price;

            if (hiddenBullishDivergence)
            {
                // Confirm the trend change to bullish using the SMA
                var smaPreviousValue = _sma.TryGetValue(price, _confirmationPeriod);
                if (smaPreviousValue.Item2 && smaValue.Item1 > smaPreviousValue.Item1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
