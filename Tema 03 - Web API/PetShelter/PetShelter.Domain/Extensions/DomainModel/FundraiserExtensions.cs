using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Domain.Extensions.DomainModel
{
    internal static class FundraiserExtensions
    {
        public static Fundraiser? ToDomainModel(this DataAccessLayer.Models.Fundraiser fundraiser)
        {
            if (fundraiser == null)
            {
                return null;
            }

            var status = Enum.Parse<FundraiserStatus>(fundraiser.Status);
            var owner=fundraiser.Owner;
            Domain.Person domainOwner= null;
            if(owner!=null)
            {
                domainOwner = owner.ToDomainModel();
            }
            else
            {
                domainOwner=new Domain.Person("0000000000000", "anonim", null);
            }
            
            var domainModel = new Fundraiser(fundraiser.Name, domainOwner, fundraiser.GoalValue, fundraiser.DueDate);
            domainModel.Status = status;
            domainModel.Id= fundraiser.Id;
            domainModel.CreationDate= fundraiser.CreationDate;
            domainModel.DonationAmount= fundraiser.DonationAmount;
            if (fundraiser.Donors != null)
            {
                domainModel.Donors = fundraiser.Donors.Select(x => x.ToDomainModel()).ToList();
            }
            else
            {
                domainModel.Donors = new List<Person>();
            }
            return domainModel;
        }
    }
}
