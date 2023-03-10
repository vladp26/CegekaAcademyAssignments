using PetShelterDemo.DAL;

namespace PetShelterDemo.Domain;

public class PetShelter
{
    private readonly IRegistry<Pet> petRegistry;
    private readonly IRegistry<Person> donorRegistry;
    private readonly IRegistry<Fundraiser> fundraiserRegistry;
    private float donationsInRON;

    public PetShelter()
    {
        donorRegistry = new Registry<Person>(new Database());
        petRegistry = new Registry<Pet>(new Database());
        fundraiserRegistry = new Registry<Fundraiser>(new Database());
    }

    public void RegisterPet(Pet pet)
    {
        petRegistry.Register(pet);
    }

    public IReadOnlyList<Pet> GetAllPets()
    {
        return petRegistry.GetAll().Result; // Actually blocks thread until the result is available.
    }

    public Pet GetByName(string name)
    {
        return petRegistry.GetByName(name).Result;
    }

    public void Donate(Person donor, Donation donation)
    {
        donorRegistry.Register(donor);
        donationsInRON += donation.calculateAmountRON();
    }

    public float GetTotalDonations()
    {
        return donationsInRON;
    }

    public IReadOnlyList<Person> GetAllDonors()
    {
        return donorRegistry.GetAll().Result;
    }
    public void RegisterFundraiser(Fundraiser fundraiser)
    {
        fundraiserRegistry.Register(fundraiser);
    }
    public Fundraiser GetFundraiserByName(string name)
    {
        return fundraiserRegistry.GetByName(name).Result;
    }
    public void DonateToFundraiser(Person donor, Donation donation, string name)
    {
        var fundraiser = fundraiserRegistry.GetByName(name).Result;
        fundraiser.Total += donation.calculateAmountRON();
        fundraiser.Persons.Register(donor);
    }
    public IReadOnlyList<Fundraiser> GetAllFundraisers()
    {
        return fundraiserRegistry.GetAll().Result;
    }
}