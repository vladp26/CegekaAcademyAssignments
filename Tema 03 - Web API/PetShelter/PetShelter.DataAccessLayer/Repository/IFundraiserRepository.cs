using PetShelter.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Repository
{
    public interface IFundraiserRepository:IBaseRepository<Fundraiser>
    {
        Task<Fundraiser?> GetFundraiserById(int id);
        Task DeleteFundraiser(int id);
        Task DonateToFundraiser(int id, Person donor, decimal value);
        Task<List<Person>?> GetDonors(int fundraiserId);
    }

}
