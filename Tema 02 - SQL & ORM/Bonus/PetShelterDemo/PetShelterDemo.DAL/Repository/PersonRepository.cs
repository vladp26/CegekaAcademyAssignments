using Microsoft.EntityFrameworkCore;
using PetShelterDemo.DAL.Models;

namespace PetShelterDemo.DAL.Repository;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{

    public PersonRepository(PetShelterContext context) : base(context)
    {
    }

    public async Task<Person?> GetPersonByIdNumber(string idNumber)
    {
        return await _context.Persons.SingleOrDefaultAsync(p => p.IdNumber == idNumber);
    }
}