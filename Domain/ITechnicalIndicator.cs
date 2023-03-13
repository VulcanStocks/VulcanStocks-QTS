using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public interface ITechnicalIndicator
    {
        public (float, bool) TryGetValue(float price, float volume);
    }
}