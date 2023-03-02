namespace PetShelterDemo.DAL.Models
{
    public class Person : IEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public override string? ToString()
        {
            return $"name: {Name}, id:{IdNumber}";
        }
    }
}
