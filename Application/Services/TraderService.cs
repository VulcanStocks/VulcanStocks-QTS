using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TraderService
    {
        private Func<float, string> _strategy;
        public TraderService(Func<float, string> strategy)
        {
            _strategy = strategy;
        }

        public void StartTrader()
        {

        }
    }
}
