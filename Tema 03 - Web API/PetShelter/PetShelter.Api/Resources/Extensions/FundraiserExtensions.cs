using PetShelter.Domain;

namespace PetShelter.Api.Resources.Extensions
{
    public static class FundraiserExtensions
    {
        public static Domain.Fundraiser AsDomainModel(this Fundraiser fundraiser)
        {
            return new Domain.Fundraiser(fundraiser.Name, fundraiser.Owner.AsDomainModel(), fundraiser.GoalValue, fundraiser.DueDate);
        }
        public static Fundraiser AsResource(this Domain.Fundraiser fundraiser)
        {
            return new Fundraiser
            {
                Name=fundraiser.Name,
                Owner=fundraiser.Owner.AsResource(),
                GoalValue=fundraiser.GoalValue,
                DueDate=fundraiser.DueDate,
                //Status=fundraiser.Status,
                //CreationDate=fundraiser.CreationDate,
                //DonationAmount=fundraiser.DonationAmount,
                //Donors=fundraiser.Donors.Select(x => x.AsResource()).ToList(),

    };
        }
    }
}
