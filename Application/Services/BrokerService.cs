using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IServices;

namespace Application.Services
{
    public class BrokerService : IBrokerService
    {
        private readonly bool _simulatedTrading;
        public BrokerService(bool simulatedTrading)
        {
            _simulatedTrading = simulatedTrading;
        }
        public void Buy(float price)
        {
            if (_simulatedTrading)
            {
                SimulatedBrokerService.Buy(price);
            }
        }

        public float GetBalance()
        {
            if (_simulatedTrading)
            {
                return SimulatedBrokerService.Balance;
            }
            else
            {
                return 0;
            }
        }

        public void Sell(float price)
        {
            if (_simulatedTrading)
            {
                SimulatedBrokerService.Sell(price);
            }
        }
    }
}