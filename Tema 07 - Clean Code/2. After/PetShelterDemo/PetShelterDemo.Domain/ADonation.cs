using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public abstract class ADonation
    {
        public decimal Amount { get; set; }
        public ADonation(decimal amount) { 
            this.Amount = amount; 
        }
        public abstract decimal GetAmountInRon();
    }
}
