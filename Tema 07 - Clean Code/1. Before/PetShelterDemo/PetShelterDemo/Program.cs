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

using PetShelterDemo.DAL;
using PetShelterDemo.Domain;
using System;
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
                { "Break our database connection", BreakDatabaseConnection },
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

    var pet = new Pet(name, description);

    shelter.RegisterPet(pet);
}

Donation getDonation()
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
    var donation = new Donation(amount, currency);
    return donation;
}
void Donate()
{
    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();

    Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();
    var person = new Person(name, id);

    var donation = getDonation();
    shelter.Donate(person, donation);
}

void SeeDonations()
{

    Console.WriteLine($"Our current donation total is {shelter.GetTotalDonations()}RON");
    
    //StringBuilder donations= new StringBuilder();
    //foreach(var key in shelter.GetTotalDonations().Keys)
    //{
    //    donations.Append(key + " ");
    //    donations.Append(shelter.GetTotalDonations()[key] + "\n");
    //}
    //Console.WriteLine($"Our current donation total is\n{donations.ToString()}");
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

void BreakDatabaseConnection()
{
    Database.ConnectionIsDown = true;
}
 void CreateFundraiser()
{
    var name = ReadString("What is the name of this new fundraiser?");
    var description = ReadString("How about the description?");
    var target = ReadInteger(header:"But its target?");
    var fundraiser = new Fundraiser(name, description, target);

    shelter.RegisterFundraiser(fundraiser);
}

void SeeFundraiserDetailsByName(string name)
{
    var fundraiser= shelter.GetFundraiserByName(name);
    StringBuilder donors= new StringBuilder();
    if(fundraiser.Persons.GetAll().Result.Count==0 || fundraiser.Persons==null)
    {
        donors.Append( "There are no donors:(");
    }
    else
    {
       // Console.WriteLine(fundraiser.Persons.GetAll().Result.ToString());
       foreach(var person in fundraiser.Persons.GetAll().Result)
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

    var fundraiserOptions = new Dictionary<string, Action<Person, float>>();
    
    if (fundraisers!=null && fundraisers.Count>0)
    {

        Console.WriteLine("What's your name? (So we can credit you.)");
        var name = ReadString();

        Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
        var id = ReadString();
        var person = new Person(name, id);
        var donation=getDonation();
        foreach (var fundraiser in fundraisers)
        {
            fundraiserOptions.Add(fundraiser.Name, (person, amount) => {
                Console.WriteLine(fundraiser.Name);
                fundraiser.Total += amount;
                fundraiser.Persons.Register(person);
            });
        }
        Console.WriteLine("We are currently organising the following fundraisers, which one do you want to choose?");
        for (var index = 0; index < fundraiserOptions.Count; index++)
        {
            Console.WriteLine(index + 1 + ". " + fundraiserOptions.ElementAt(index).Key);
        }
        var userInput = ReadInteger(fundraiserOptions.Count);
        fundraiserOptions.ElementAt(userInput - 1).Value(person, donation.calculateAmountRON());

    }
    else
    {
        Console.WriteLine("We have no fundraisers you can donate to"); 
    }
    
    //shelter.Donate(person, amountInRon);
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
        //aici am vrut sa apelez metoda seePetDetailsByName
        if (!aux)
        {
            options.ElementAt(index).Value();
        }
    }

    //aici aparea o problema
    //metoda era apelata si in SeePets() iar optiunile erau reprezentate de afisari ale animalelor
    //problema era faptul ca dupa ce se afisau animalele se astepta un input de la utilizator
    //inainte sa se ajunga la meniul principal
    //de aceea am folosit parametrul aux, pe care il setez ca true/false dupa caz
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