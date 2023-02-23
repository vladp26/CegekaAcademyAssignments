

namespace PetShelter.DataAccessLayer.Models
{
    public class Fundraiser : IEntity
    {
        public int Id { get; set ; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Target { get; set; }
        public float Total { get; set; }
        public ICollection<Donation> Donations { get; set; }
        
    }
}
