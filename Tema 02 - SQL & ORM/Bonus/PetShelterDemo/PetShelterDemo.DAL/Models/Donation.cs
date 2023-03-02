using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.DAL.Models
{
    public class Donation : IEntity
    {
        public enum CurrencyType { RON, USD, EUR }
        public readonly static float USDtoRON = 4.58f;
        public readonly static float EURtoRON = 4.9f;
        public int Amount { get; set; }
        public CurrencyType Currency { get; set; }

        public string? Name {get;set; }

        public int Id {get; set; }

        public int DonorId { get; set; }

        public Person Donor { get; set; }

        public int? FundraiserId { get; set; }
        public Fundraiser? ReceivingFundraiser { get; set; }

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
