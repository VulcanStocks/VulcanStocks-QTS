using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SimulatedBrokerService
    {
        public float Balance { get; set; }
        public bool HasAsset { get; set; }

        public SimulatedBrokerService()
        {   
            Balance = 1000000;
        }
        public void Buy(float price)
        {

            if (!HasAsset)
            {
                Balance -= price;
                HasAsset = true;
            }

        }

        public void Sell(float price)
        {
            if (HasAsset)
            {
                Balance += price;
                HasAsset = false;
            }
        }

    }
}