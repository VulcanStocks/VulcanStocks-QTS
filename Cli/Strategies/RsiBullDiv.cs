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
        private float _prevPrice;
        private (float, bool) _prevRsi;

        public HiddenBullishDivergenceStrategy(int rsiPeriod)
        {
            _rsi = new Rsi(rsiPeriod);
        }

        public bool CheckHiddenBullishDivergence(float price)
        {
            var rsiValue = _rsi.TryGetValue(price, 0);
            if(rsiValue.Item2){

            }
            if (!_prevRsi.Item2 || !rsiValue.Item2)
            {
                _prevRsi = rsiValue;
                _prevPrice = price;
                return false;
            }

            bool hiddenBullishDivergence = _prevPrice < price && _prevRsi.Item1 > rsiValue.Item1;
            _prevRsi = rsiValue;
            _prevPrice = price;

            return hiddenBullishDivergence;
        }
    }



}