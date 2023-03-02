using PetShelterDemo.DAL.Models;

namespace PetShelterDemo.DAL.Repository;
public interface IPersonRepository: IBaseRepository<Person>
{
    Task<Person?> GetPersonByIdNumber(string idNumber);
}