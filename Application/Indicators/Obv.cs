using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Indicators
{
    public class Obv : ITechnicalIndicator
    {
        private float _previousPrice;
        private float _obv;
        private bool _isValid;

        public Obv()
        {
            _previousPrice = 0;
            _obv = 0;
            _isValid = false;
        }

        public (float, bool) TryGetValue(float price, float volume)
        {
            float currentObv;

            if (_previousPrice == 0) // first time
            {
                currentObv = 0; // no value yet
                _isValid = false; // not ready yet
            }
            else
            {
                if (price > _previousPrice) // price increased
                {
                    _obv += volume; // add volume to previous OBV
                }
                else if (price < _previousPrice) // price decreased
                {
                    _obv -= volume; // subtract volume from previous OBV
                }

                currentObv = _obv; // return current OBV value
                _isValid = true; // ready to use
            }

            _previousPrice = price; // update previous price

            return (currentObv, _isValid);
        }

    }
}