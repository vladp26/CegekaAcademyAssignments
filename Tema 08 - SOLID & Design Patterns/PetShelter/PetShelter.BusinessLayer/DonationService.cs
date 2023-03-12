using Azure.Core;
using FluentValidation;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;

namespace PetShelter.BusinessLayer;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _donationRepository;
    private readonly IValidator<AddDonationRequest> _donationValidator;
    private readonly IPersonService _personService;

    public DonationService(IDonationRepository donationRepository, IPersonService personService, IValidator<AddDonationRequest> validator)
    {
        _donationValidator = validator;
        _donationRepository = donationRepository;
        _personService = personService;
    }

    public async Task AddDonation(AddDonationRequest addDonationRequest)
    {
        var validationResult = await _donationValidator.ValidateAsync(addDonationRequest);
        if (!validationResult.IsValid) { throw new ArgumentException(); }
        var donor = await _personService.GetPerson(addDonationRequest.Donor);

        await _donationRepository.Add(new DataAccessLayer.Models.Donation
        {
            Amount = addDonationRequest.Amount,
            Donor=donor
        });
    }
}