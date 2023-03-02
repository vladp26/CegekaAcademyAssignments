using Microsoft.EntityFrameworkCore;
using PetShelterDemo.DAL.Models;

namespace PetShelterDemo.DAL.Repository;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    public PetRepository(PetShelterContext context) : base(context)
    {
    }

    public async Task<Pet?> GetPetByName(string name)
    {
        return await _context.Pets.FirstOrDefaultAsync(p => p.Name.Equals(name));
    }
}