using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Domain.Services
{
    public interface IFundraiserService
    {
        Task DonateToFundraiserAsync(int fundraiserId, int donorId, decimal amount);

        Task<Fundraiser> GetFundraiser(int fundraiserId);

        Task<IReadOnlyCollection<Fundraiser>> GetAllFundraisers();

        Task<int> CreateFundraiserAsync(Person owner, Fundraiser fundraiser);

        Task DeleteFundraiserAsync(int fundraiserId);
    }
}
