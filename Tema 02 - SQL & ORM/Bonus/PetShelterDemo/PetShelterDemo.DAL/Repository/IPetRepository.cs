using PetShelterDemo.DAL.Models;

namespace PetShelterDemo.DAL.Repository;

public interface IPetRepository: IBaseRepository<Pet>
{
    Task<Pet?> GetPetByName(string name);

}