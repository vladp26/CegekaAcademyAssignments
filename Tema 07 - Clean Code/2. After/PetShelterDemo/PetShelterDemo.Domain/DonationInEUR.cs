using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public class DonationInEUR : ADonation
    {
        public DonationInEUR(decimal amount) : base(amount) {
        }

        public override decimal GetAmountInRon()
        {
            return Amount * Constants.EURtoRON;
        }
    }
}
