//// See https://aka.ms/new-console-template for more information
//// Syntactic sugar: Starting with .Net 6, Program.cs only contains the code that is in the Main method.
//// This means we no longer need to write the following code, but the compiler still creates the Program class with the Main method:
//// namespace PetShelterDemo
//// {
////    internal class Program
////    {
////        static void Main(string[] args)
////        { actual code here }
////    }
//// }

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetShelterDemo.DAL;
using PetShelterDemo.DAL.Models;
using PetShelterDemo.DAL.Repository;
using PetShelterDemo.Domain;
using System;
using System.Drawing;
using System.Text;

var shelter = new PetShelter();

Console.WriteLine("Hello, Welcome the the Pet Shelter!");

var exit = false;
try
{
    while (!exit)
    {
        PresentOptions(
            "Here's what you can do.. ",
            new Dictionary<string, Action>
            {
                { "Register a newly rescued pet", RegisterPet },
                { "Donate", Donate },
                { "See current donations total", SeeDonations },
                { "See our residents", SeePets },
                { "Register a fundraiser", CreateFundraiser },
                { "See fundraisers", SeeFundraisers },
                { "Donate to fundraiser", DonateToFundraiser },
                { "Leave:(", Leave }
            }, true
        );
    }
}
catch (Exception e)
{
    Console.WriteLine($"Unfortunately we ran into an issue: {e.Message}.");
    Console.WriteLine("Please try again later.");
}


void RegisterPet()
{
    var name = ReadString("Name?");
    var description = ReadString("Description?");

    var pet = new Pet();
    pet.Name = name;
    pet.Description = description;
    shelter.RegisterPetAsync(pet);
}

Donation getDonation(int donorId, int? fundraiserId)
{
    Console.WriteLine("How much would you like to donate?");
    var amount = ReadInteger();
    float realAmount = (float)amount;
    Console.WriteLine("What currency? 0 for RON, 1 for EUR, 2 for USD");
    var currencyNumber = ReadInteger(2);
    Donation.CurrencyType currency=0;
    switch(currencyNumber)
    {
        case 0:
            currency = Donation.CurrencyType.RON;
            break;
        case 1:
            currency = Donation.CurrencyType.EUR;
            break;
        case 2:
            currency = Donation.CurrencyType.USD;
            break;
        default:
            break;

    }
    var donation = new Donation();
    donation.Amount = amount;
    donation.Currency = currency;
    donation.FundraiserId = fundraiserId;
    donation.DonorId=donorId;
    return donation;
}
void Donate()
{

    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();

    Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();
    var person = new Person();
    person.Name = name;
    person.IdNumber = id;
    var donation = getDonation(person.Id, null);
    donation.Donor = person;
    shelter.Donate(person, donation);
}

void SeeDonations()
{

    Console.WriteLine($"Our current donation total is {shelter.GetTotalDonations()}RON");
    Console.WriteLine("Special thanks to our donors:");
    var donors = shelter.GetAllDonors();
    foreach (var donor in donors)
    {
        Console.WriteLine(donor.Name);
    }
}

void SeePets()
{

    var pets = shelter.GetAllPets();

    var petOptions = new Dictionary<string, Action>();
    foreach (var pet in pets)
    {
        petOptions.Add(pet.Name, () => SeePetDetailsByName(pet.Name));
    }

    PresentOptions("We got..", petOptions, false);
}

void SeePetDetailsByName(string name)
{
    var pet = shelter.GetByName(name);
    Console.WriteLine($"A few words about {pet.Name}: {pet.Description}");
}

 void CreateFundraiser()
{
    var name = ReadString("What is the name of this new fundraiser?");
    var description = ReadString("How about the description?");
    var target = ReadInteger(header:"But its target?");
    var fundraiser = new Fundraiser();
    fundraiser.Description = description;
    fundraiser.Target=target; 
    fundraiser.Name = name;
    shelter.RegisterFundraiserAsync(fundraiser);
}

void SeeFundraiserDetailsByName(string name)
{
    var fundraiser= shelter.GetFundraiserByName(name);
    StringBuilder donors= new StringBuilder();
    
    if(shelter.GetDonorsForFundraiser(fundraiser).Count==0 || shelter.GetDonorsForFundraiser(fundraiser) ==null)
    {
        donors.Append( "There are no donors:(");
    }
    else
    {
       foreach(var person in shelter.GetDonorsForFundraiser(fundraiser))
        {
            donors.Append( person.ToString());
            donors.Append('\n');
        }
    }
    Console.WriteLine($"A few words about {fundraiser.Name}\n description: {fundraiser.Description}\n target:{ fundraiser.Target}\n" +
        $"total: {fundraiser.Total}\n donors:\n{donors.ToString()} ");
}
void SeeFundraisers()
{
    var fundraisers = shelter?.GetAllFundraisers();

    var fundraiserOptions = new Dictionary<string, Action>();
    foreach (var fundraiser in fundraisers)
    {
        fundraiserOptions.Add(fundraiser.Name, () => SeeFundraiserDetailsByName(fundraiser.Name));
    }

    PresentOptions("We got..", fundraiserOptions, false);
}
void DonateToFundraiser()
{
    var fundraisers = shelter?.GetAllFundraisers();

    var fundraiserOptions = new Dictionary<string, Action<string, int>>();
    
    if (fundraisers!=null && fundraisers.Count>0)
    {

        Console.WriteLine("What's your name? (So we can credit you.)");
        var name = ReadString();

        Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
        var id = ReadString();
        var person = new Person();
        person.Name = name;
        person.IdNumber = id;
        foreach (var fundraiser in fundraisers)
        {
            fundraiserOptions.Add(fundraiser.Name, (name, id) => {
                Console.WriteLine(fundraiser.Name);
                id=shelter.GetFundraiserByName(name).Id;
            });
        }


        Console.WriteLine("How much would you like to donate?");
        var amount = ReadInteger();
        float realAmount = (float)amount;
        Console.WriteLine("What currency? 0 for RON, 1 for EUR, 2 for USD");
        var currencyNumber = ReadInteger(2);
        Donation.CurrencyType currency = 0;
        switch (currencyNumber)
        {
            case 0:
                currency = Donation.CurrencyType.RON;
                break;
            case 1:
                currency = Donation.CurrencyType.EUR;
                break;
            case 2:
                currency = Donation.CurrencyType.USD;
                break;
            default:
                break;

        }
        var donation = new Donation();
        donation.Amount = amount;
        donation.Currency = currency;
        donation.DonorId = person.Id;


        Console.WriteLine("We are currently organising the following fundraisers, which one do you want to choose?");
        for (var index = 0; index < fundraiserOptions.Count; index++)
        {
            Console.WriteLine(index + 1 + ". " + fundraiserOptions.ElementAt(index).Key);
        }
        var userInput = ReadInteger(fundraiserOptions.Count);
        int fundraiserId=0;
        fundraiserOptions.ElementAt(userInput - 1).Value(fundraiserOptions.ElementAt(userInput-1).Key,fundraiserId);
        donation.FundraiserId= fundraiserId;
        shelter.Donate(person, donation);

    }
    else
    {
        Console.WriteLine("We have no fundraisers you can donate to"); 
    }
}
void Leave()
{
    Console.WriteLine("Good bye!");
    exit = true;
}

void PresentOptions(string header, IDictionary<string, Action> options, bool aux)
{

    Console.WriteLine(header);

    for (var index = 0; index < options.Count; index++)
    {
        Console.WriteLine(index + 1 + ". " + options.ElementAt(index).Key);
        if (!aux)
        {
            options.ElementAt(index).Value();
        }
    }
    if (aux)
    {
        var userInput = ReadInteger(options.Count);
        options.ElementAt(userInput - 1).Value();
    }
}

string ReadString(string? header = null)
{
    if (header != null) Console.WriteLine(header);

    var value = Console.ReadLine();
    Console.WriteLine("");
    return value;
}

int ReadInteger(int maxValue = int.MaxValue, string? header = null)
{
    if (header != null) Console.WriteLine(header);

    var isUserInputValid = int.TryParse(Console.ReadLine(), out var userInput);
    if (!isUserInputValid || userInput > maxValue)
    {
        Console.WriteLine("Invalid input");
        Console.WriteLine("");
        return ReadInteger(maxValue, header);
    }

    Console.WriteLine("");
    return userInput;
}