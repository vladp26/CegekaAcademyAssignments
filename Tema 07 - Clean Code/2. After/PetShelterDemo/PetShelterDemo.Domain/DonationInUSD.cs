using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public class DonationInUSD : ADonation
    {
        public DonationInUSD(decimal amount) : base(amount) {
        }

        public override decimal GetAmountInRon()
        {
            return Amount * Constants.USDtoRON;
        }
    }
}
