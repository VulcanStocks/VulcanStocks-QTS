using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class Rsi : ITechnicalIndicator
    {
        private readonly int _period;

        private readonly Queue<float> _gains;

        private readonly Queue<float> _losses;

        private float _lastPrice;

        public Rsi(int period)
        {
            _period = period;
            _gains = new Queue<float>(period);
            _losses = new Queue<float>(period);
            _lastPrice = 0f;
        }

        public (float, bool) TryGetValue(float price, float volume)
        {
            // RSI requires at least two data points
            if (_lastPrice == 0f)
            {
                _lastPrice = price;
                return (0f, false);
            }

            // Calculate the price change and store it as either a gain or loss
            float priceChange = price - _lastPrice;
            float gain = priceChange > 0f ? priceChange : 0f;
            float loss = priceChange < 0f ? -priceChange : 0f;
            _gains.Enqueue (gain);
            _losses.Enqueue (loss);

            // If we haven't accumulated enough data, return false
            if (_gains.Count < _period)
            {
                _lastPrice = price;
                return (0f, false);
            }

            // Remove the oldest gain/loss to maintain the period length
            if (_gains.Count > _period)
            {
                _gains.Dequeue();
                _losses.Dequeue();
            }

            // Calculate the average gain and loss over the period
            float avgGain = _gains.Average();
            float avgLoss = _losses.Average();

            // Calculate the relative strength (RS)
            float rs = avgGain / avgLoss;

            // Calculate the RSI
            float rsi = 100f - (100f / (1f + rs));

            _lastPrice = price;
            return (rsi, true);
        }
    }
}
