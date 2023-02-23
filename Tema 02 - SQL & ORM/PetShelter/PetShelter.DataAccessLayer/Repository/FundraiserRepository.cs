
using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;

namespace PetShelter.DataAccessLayer.Repository
{
    public class FundraiserRepository : BaseRepository<Fundraiser>, IFundraiserRepository
    {
        public FundraiserRepository(PetShelterContext context) : base(context)
        {
        }

        public async Task<decimal> GetTotal(int FundraiserId)
        {
            //var amounts= await _context.Donations.Where(x => x.FundraiserId == FundraiserId).Select(x => x.Amount).ToListAsync();
            //return amounts.Sum();
            return (await _context.Donations.Where(x => x.FundraiserId == FundraiserId).Select(x => x.Amount).ToListAsync()).Sum();
        }
    }
}
