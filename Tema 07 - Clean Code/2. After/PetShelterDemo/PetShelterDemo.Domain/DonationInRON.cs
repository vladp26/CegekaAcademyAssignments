using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public class DonationInRON : ADonation
    {
        public DonationInRON(decimal amount) : base(amount) {
        }

        public override decimal GetAmountInRon()
        {
            return Amount;
        }
    }
}
