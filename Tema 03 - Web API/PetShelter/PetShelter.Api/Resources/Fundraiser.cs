using PetShelter.Domain;

namespace PetShelter.Api.Resources
{
    public class Fundraiser
    {
        public string Name { get; set; }
        public Person Owner { get; set; }
        public decimal GoalValue { get; set; }
        public DateTime DueDate { get; set; }
       // public FundraiserStatus Status { get; set; }
        //public DateTime CreationDate { get; set; }
        //public decimal DonationAmount { get; set; }
       // public ICollection<Person>? Donors { get; set; }
    }
}
