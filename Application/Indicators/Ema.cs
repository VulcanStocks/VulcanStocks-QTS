using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class Ema : ITechnicalIndicator
{
    private readonly int _period;
    private readonly Queue<float> _prices;
    private float _previousEma;

    public Ema(int period)
    {
        _period = period;
        _prices = new Queue<float>(period);
        _previousEma = 0f;
    }

    public (float, bool) TryGetValue(float price, float volume)
    {
        _prices.Enqueue(price);

        // If we haven't accumulated enough data, return false
        if (_prices.Count < _period)
        {
            return (0f, false);
        }

        // Remove the oldest price to maintain the period length
        if (_prices.Count > _period)
        {
            _prices.Dequeue();
        }

        // Calculate the multiplier for the EMA
        float multiplier = 2f / (_period + 1f);

        // Calculate the EMA
        float ema;
        if (_previousEma == 0f)
        {
            ema = _prices.Average();
        }
        else
        {
            float previousEma = _previousEma;
            float lastPrice = _prices.Last();
            ema = (lastPrice - previousEma) * multiplier + previousEma;
        }

        _previousEma = ema;
        return (ema, true);
    }
}

}