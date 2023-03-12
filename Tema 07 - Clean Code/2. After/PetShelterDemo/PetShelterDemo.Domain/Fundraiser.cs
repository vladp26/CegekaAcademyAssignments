using PetShelterDemo.DAL;
namespace PetShelterDemo.Domain
{
    public class Fundraiser :INamedEntity
    { 

        public IRegistry<Person> Donors { get; set; }
        public string Name { get; }
        public string Description { get; }
        public decimal Target { get; }
        public decimal Total { get; set; }


        public Fundraiser(string name, string description, decimal target)
        {
            Name=name;
            Description=description;
            Target=target;
            Total=0M;
            Donors = new Registry<Person>(new Database());

        }

        
    }
}
