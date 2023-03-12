using PetShelterDemo.DAL;
namespace PetShelterDemo.Domain
{
    public class Fundraiser :INamedEntity
    { 

        public IRegistry<Person> Persons { get; set; }
        public string Name { get; }
        public string Description { get; }
        public int Target { get; }
        public float Total { get; set; }


        public Fundraiser(string name, string description, int target)
        {
            Name=name;
            Description=description;
            Target=target;
            Total=0f;
            Persons = new Registry<Person>(new Database());

        }

        
    }
}
