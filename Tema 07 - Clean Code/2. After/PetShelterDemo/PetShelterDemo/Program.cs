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
using System.Runtime.Intrinsics.X86;
using System.Text;

var shelter = new PetShelter();

Console.WriteLine("Hello, Welcome the the Pet Shelter!");

var exit = false;
try
{
    while (!exit)
    {
        PresentOptionsWithFollowingUserInput(
            "Here's what you can do.. ",
            new Dictionary<string, Action>
            {
                { "Register a newly rescued pet", RegisterPet },
                { "Donate", DonateToShelter },
                { "See current donations total", SeeDonations },
                { "See our residents", SeePets },
                { "Break our database connection", BreakDatabaseConnection },
                { "Register a fundraiser", CreateFundraiser },
                { "See fundraisers", SeeFundraisers },
                { "Donate to fundraiser", DonateToFundraiser },
                { "Leave:(", Leave }
            }
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

decimal GetDonationAmount()
{
    Console.WriteLine("How much would you like to donate?");
    var amount = ReadInteger();
    decimal decimalAmount = (decimal)amount;
    return decimalAmount;
}
int GetDonationCurrency()
{
    Console.WriteLine("What currency? 0 for RON, 1 for EUR, 2 for USD");
    var currencyNumber = ReadInteger(2);
    return currencyNumber;
}
ADonation CreateDonation(decimal amount, int currencyNumber)
{
    ADonation donation = null;
    switch (currencyNumber)
    {
        case 0:
            donation = new DonationInRON(amount);
            break;
        case 1:
            donation = new DonationInEUR(amount);
            break;
        case 2:
            donation = new DonationInUSD(amount);
            break;
        default:
            break;
    }
    return donation;
}
ADonation GetDonation()
{
    var amount = GetDonationAmount();
    var currencyNumber=GetDonationCurrency();
    ADonation donation=CreateDonation(amount, currencyNumber);
    return donation;
}

Person CreatePerson()
{
    Console.WriteLine("What's your name? (So we can credit you.)");
    var name = ReadString();
    Console.WriteLine("What's your personal Id? (No, I don't know what GDPR is. Why do you ask?)");
    var id = ReadString();
    var person = new Person(name, id);
    return person;
}
void DonateToShelter()
{
    var person = CreatePerson();
    var donation = GetDonation();
    shelter.Donate(person, donation);
}

void SeeDonations()
{
    Console.WriteLine($"Our current donation total is {shelter.GetTotalDonations()} RON");
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
    PresentOptionsWithoutFollowingUserInput("We got..", petOptions);
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
    if(fundraiser.Donors == null || fundraiser.Donors.GetAll().Result.Count==0)
    {
        donors.Append( "There are no donors:(");
    }
    else
    {
       foreach(var person in fundraiser.Donors.GetAll().Result)
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
    PresentOptionsWithoutFollowingUserInput("We got..", fundraiserOptions);
}

Dictionary<string, Action<Person, decimal>> CreateFundraiserOptions(IReadOnlyList<Fundraiser> fundraisers)
{
    var fundraiserOptions = new Dictionary<string, Action<Person, decimal>>();
    foreach (var fundraiser in fundraisers)
    {
        fundraiserOptions.Add(fundraiser.Name, (person, amount) => {
            Console.WriteLine(fundraiser.Name);
            fundraiser.Total += amount;
            fundraiser.Donors.Register(person);
        });
    }
    return fundraiserOptions;
}
void ShowCurrentFundraisersForDonation(Dictionary<string, Action<Person, decimal>> fundraiserOptions)
{
    Console.WriteLine("We are currently organising the following fundraisers, which one do you want to choose?");
    for (var index = 0; index < fundraiserOptions.Count; index++)
    {
        Console.WriteLine(index + 1 + ". " + fundraiserOptions.ElementAt(index).Key);
    }
}
void DonateToFundraiser()
{
    var fundraisers = shelter?.GetAllFundraisers();
    if (fundraisers!=null && fundraisers.Count>0)
    {
        var person = CreatePerson();
        var donation = GetDonation();
        var fundraiserOptions=CreateFundraiserOptions(fundraisers);
        ShowCurrentFundraisersForDonation(fundraiserOptions);
        var userInput = ReadInteger(fundraiserOptions.Count);
        fundraiserOptions.ElementAt(userInput - 1).Value(person, donation.GetAmountInRon());
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

void PresentOptionsWithoutFollowingUserInput(string header, IDictionary<string, Action> options)
{
    Console.WriteLine(header);
    for (var index = 0; index < options.Count; index++)
    {
        Console.WriteLine(index + 1 + ". " + options.ElementAt(index).Key);
        options.ElementAt(index).Value();
    }
}
void PresentOptionsWithFollowingUserInput(string header, IDictionary<string, Action> options)
{
    Console.WriteLine(header);
    for (var index = 0; index < options.Count; index++)
    {
        Console.WriteLine(index + 1 + ". " + options.ElementAt(index).Key);
    }
    var userInput = ReadInteger(options.Count);
    options.ElementAt(userInput - 1).Value();
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