namespace PetShelterDemo.Domain
{
    public class Person : INamedEntity
    {
        public string Name { get; }
        public string IdNumber { get; }

        public Person(string name, string idNumber)
        {
            Name = name;
            IdNumber = idNumber;
        }
        public override string? ToString()
        {
            return $"name: {Name}, id:{IdNumber}";
        }
    }
}
