using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Domain
{
    public class Fundraiser
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public Person Owner { get; set; }
        public decimal GoalValue { get; set; }
        public DateTime DueDate { get; set; }
        public FundraiserStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal DonationAmount { get; set; }
        public ICollection<Person>? Donors { get; set; }
        public Fundraiser(string name, Person owner, decimal goalValue, DateTime dueDate)
        {
            Name = name;
            Owner = owner;
            GoalValue = goalValue;
            DueDate = dueDate;
            if(DateTime.Compare(dueDate, DateTime.Now)>0)
            {
                Status = FundraiserStatus.Active;
            }
            else
            {
                Status = FundraiserStatus.Closed;
            }
            CreationDate = DateTime.Now;
            DonationAmount = 0;
            Donors=new List<Person>();
        }
    }
}
