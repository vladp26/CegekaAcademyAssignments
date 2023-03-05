using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using PetShelter.Domain.Exceptions;
using PetShelter.Domain.Extensions.DataAccess;
using PetShelter.Domain.Extensions.DomainModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Domain.Services
{
    public class FundraiserService : IFundraiserService
    {
        private readonly IFundraiserRepository _fundraiserRepository;
        private readonly IPersonRepository _personRepository;
        public FundraiserService(IFundraiserRepository fundraiserRepository, IPersonRepository personRepository)
        {
            _fundraiserRepository = fundraiserRepository;
            _personRepository = personRepository;
        }

        public async Task<int> CreateFundraiserAsync(Person owner, Fundraiser fundraiser)
        {
            var person = await _personRepository.GetOrAddPersonAsync(owner.FromDomainModel());
            var addedFundraiser = new DataAccessLayer.Models.Fundraiser
            {
                Name = fundraiser.Name,
                Owner=person,
                OwnerId=person.Id,
                GoalValue=fundraiser.GoalValue,
                DueDate=fundraiser.DueDate,
                Status=fundraiser.Status.ToString(),
                CreationDate=fundraiser.CreationDate,
                DonationAmount=fundraiser.DonationAmount,
                Donors=fundraiser.Donors.Select(x => x.FromDomainModel()).ToList()
            };
            await _fundraiserRepository.Add(addedFundraiser);
            return addedFundraiser.Id;
    }

        public async Task DeleteFundraiserAsync(int fundraiserId)
        {
            var fundraiser = await _fundraiserRepository.GetById(fundraiserId);
            if(fundraiser==null)
            {
                throw new NotFoundException($"Fundraiser with id {fundraiserId} not found.");
            }
            fundraiser.Status = "Closed";
            _fundraiserRepository.Update(fundraiser);
        }

        public async Task DonateToFundraiserAsync(int fundraiserId, int donorId, decimal amount)
        {
            var fundraiser = await _fundraiserRepository.GetById(fundraiserId);
            if(fundraiser==null)
            {
                throw new NotFoundException($"Fundraiser with id {fundraiserId} not found.");
            }
            var person = await _personRepository.GetById(donorId);
            if(person==null)
            {
                throw new NotFoundException($"Person with id {donorId} not found.");
            }
            if(fundraiser.Status=="Closed")
            {
                throw new NotFoundException($"Fundraiser with id {fundraiserId} is closed.");
            }
            person.FundraiserWhichGotTheDonationId = fundraiserId;
            fundraiser.DonationAmount += amount;
            if(fundraiser.DonationAmount>fundraiser.GoalValue)
            {
                fundraiser.Status = "Closed";
            }
            _fundraiserRepository.Update(fundraiser);
            _personRepository.Update(person);
        }

        public async Task<IReadOnlyCollection<Fundraiser>> GetAllFundraisers()
        {
            var fundraisers = await _fundraiserRepository.GetAll();
            return fundraisers.Select(async p => { p.Owner = await _personRepository.GetById(p.OwnerId);
                p.Donors = await _fundraiserRepository.GetDonors(p.Id); return p; })
                .Select(p => p.Result)
                .Select(p => p.ToDomainModel())
                .ToImmutableArray();
        }

        public async Task<Fundraiser> GetFundraiser(int fundraiserId)
        {
            var fundraiser = await _fundraiserRepository.GetById(fundraiserId);
            if (fundraiser == null)
            {
                return null;
            }
            fundraiser.Owner = await _personRepository.GetById(fundraiser.OwnerId);
            fundraiser.Donors = await _fundraiserRepository.GetDonors(fundraiserId);
            return fundraiser.ToDomainModel(); ;
        }
    }
}
