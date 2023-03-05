using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Repository
{
    public class FundraiserRepository : BaseRepository<Fundraiser>, IFundraiserRepository
    {
        public FundraiserRepository(PetShelterContext context) : base(context)
        {
        }

        public async Task DeleteFundraiser(int id)
        {
            var fundraiser = await _context.Fundraisers.FirstOrDefaultAsync(p => p.Id == id);
            fundraiser.Status = "Closed";
            await Update(fundraiser);

        }

        public async Task DonateToFundraiser(int id, Person donor, decimal value)
        {
            var fundraiser = await _context.Fundraisers.FirstOrDefaultAsync(p => p.Id == id);
            fundraiser.DonationAmount += value;
            fundraiser.Donors.Add(donor);
            if(fundraiser.DonationAmount>fundraiser.GoalValue)
            {
                fundraiser.Status = "Closed";
            }
            await Update(fundraiser);
        }

        public async Task<List<Person>?> GetDonors(int fundraiserId)
        {
            var fundraiser = await _context.Fundraisers.FirstOrDefaultAsync(p => p.Id == fundraiserId);
            return await _context.Persons.Where(p => p.FundraiserWhichGotTheDonationId==fundraiserId).ToListAsync();
        }

        public async Task<Fundraiser?> GetFundraiserById(int id)
        {
            return await _context.Fundraisers.FirstOrDefaultAsync(p => p.Id==id);
        }
    }
}
