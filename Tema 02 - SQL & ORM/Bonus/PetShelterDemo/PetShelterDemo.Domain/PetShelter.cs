using PetShelterDemo.DAL;
using PetShelterDemo.DAL.Models;
using PetShelterDemo.DAL.Repository;

namespace PetShelterDemo.Domain;

public class PetShelter
{
    private readonly PetShelterContext context=new PetShelterContext();
    private readonly PetRepository _petRepository;
    private readonly PersonRepository _personRepository;
    private readonly FundraiserRepository _fundraiserRepository;
    private readonly DonationRepository _donationRepository;
    private float donationsInRON;

    public PetShelter()
    {
        _petRepository = new PetRepository(context);
        _personRepository = new PersonRepository(context);
        _fundraiserRepository= new FundraiserRepository(context);
        _donationRepository= new DonationRepository(context);
    }

    public async Task RegisterPetAsync(Pet pet)
    {
        await _petRepository.Add(pet);
    }

    public IReadOnlyList<Pet> GetAllPets()
    {
        return _petRepository.GetAll().Result;
    }

    public Pet GetByName(string name)
    {
        return _petRepository.GetPetByName(name).Result;
    }

    public void Donate(Person donor, Donation donation)
    {
        _personRepository.Add(donor);
        _donationRepository.Add(donation);
        donationsInRON += donation.calculateAmountRON();
    }

    public float GetTotalDonations()
    {
        return donationsInRON;
    }

    public IReadOnlyList<Person> GetAllDonors()
    {
        return _personRepository.GetAll().Result;
    }
    public async Task RegisterFundraiserAsync(Fundraiser fundraiser)
    {
        await _fundraiserRepository.Add(fundraiser);
    }
    public Fundraiser GetFundraiserByName(string name)
    {
        return _fundraiserRepository.GetByName(name).Result;
    }
    public void DonateToFundraiser(Person donor, Donation donation, string name)
    {
        var fundraiser = _fundraiserRepository.GetByName(name).Result;
        fundraiser.Total += donation.calculateAmountRON();
    }
    public IReadOnlyList<Fundraiser> GetAllFundraisers()
    {
        return _fundraiserRepository.GetAll().Result;
    }
    public IReadOnlyList<Person>GetDonorsForFundraiser(Fundraiser fundraiser)
    {
        return (IReadOnlyList<Person>)_fundraiserRepository.GetPersons(fundraiser).Result;
    }
}