
using Microsoft.EntityFrameworkCore;
using PetShelterDemo.DAL;
using PetShelterDemo.DAL.Models;

namespace PetShelterDemo.DAL.Repository
{
    public class FundraiserRepository : BaseRepository<Fundraiser>, IFundraiserRepository
    {
        public FundraiserRepository(PetShelterContext context) : base(context)
        {
        }

        public async Task<Fundraiser> GetByName(String name)
        {
            return await _context.Set<Fundraiser>().SingleOrDefaultAsync(x => x.Name == name);
        }
        public async Task<ICollection<Person>> GetPersons(Fundraiser fundraiser)
        {
            var personIds=await _context.Donations.Where(x => x.FundraiserId == fundraiser.Id).Select(x => x.DonorId).ToListAsync();
            return await _context.Persons.Where(x => personIds.Contains(x.Id)).ToListAsync();
        }
        
    }
}
