using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public static class SimulatedBrokerService
    {
        public static float Balance { get; set; }
        public static bool HasAsset { get; set; }

        public static void InitSimulatedBroker(float balance)
        {
            Balance = balance;
        }
        public static void Buy(float price)
        {
            if (!HasAsset)
            {
                Balance -= price;
                HasAsset = true;
            }
        }

        public static void Sell(float price)
        {
            if (HasAsset)
            {
                Balance += price;
                HasAsset = false;
            }
        }

    }
}