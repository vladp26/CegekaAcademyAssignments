using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public class Donation
    {
        public enum CurrencyType { RON, USD, EUR}
        public readonly static float USDtoRON = 4.58f;
        public readonly static float EURtoRON = 4.9f;
        public int Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public Donation(int amount, CurrencyType currency) 
        {
            this.Amount = amount;
            this.Currency = currency;
        }
        public float calculateAmountRON()
        {
            float amountRON = (float)Amount;
            if (this.Currency == CurrencyType.EUR)
            {
                amountRON *= EURtoRON;
            }
            else if (Currency == CurrencyType.USD)
            {
                amountRON *= USDtoRON;
            }
            return amountRON;
        }
    }
}
