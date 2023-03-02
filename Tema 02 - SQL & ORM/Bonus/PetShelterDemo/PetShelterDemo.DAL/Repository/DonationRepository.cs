using PetShelterDemo.DAL.Models;

namespace PetShelterDemo.DAL.Repository;

public class DonationRepository : BaseRepository<Donation>, IDonationRepository
{
    public DonationRepository(PetShelterContext context): base(context)
    {
    }
}