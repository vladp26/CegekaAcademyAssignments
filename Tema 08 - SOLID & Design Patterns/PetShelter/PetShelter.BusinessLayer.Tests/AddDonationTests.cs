using Azure.Core;
using Moq;
using PetShelter.BusinessLayer.Builder;
using PetShelter.BusinessLayer.Validators;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using System.Drawing;

namespace PetShelter.BusinessLayer.Tests;

public class AddDonationTests
{
    private readonly Mock<IDonationRepository> _donationRepositoryMock;
    private readonly DonationService _donationServiceSut;
    private readonly Mock<IPersonService> _mockPersonService;
    private readonly IPersonBuilder _personBuilder;
    private readonly PersonDirector _personDirector;

    public AddDonationTests()
    {
        _mockPersonService= new Mock<IPersonService>();
        _donationRepositoryMock = new Mock<IDonationRepository>();
        _donationServiceSut = new DonationService(_donationRepositoryMock.Object, _mockPersonService.Object, new AddDonationRequestValidator());
        _personBuilder=new PersonBuilder();
        _personDirector=new PersonDirector();
    }

    [Fact]
    public async Task GivenValidRequest_WhenAddDonation_DonationIsAdded()
    {
        _personDirector.PersonBuilder = _personBuilder;
        _personDirector.BuildPersonWithAllProperties("Nume Corect", "1111222233334", DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge));
        var request = new AddDonationRequest
        {
            Amount = 10,
            Donor = _personBuilder.GetPerson()
        };
        await _donationServiceSut.AddDonation(request);

        _donationRepositoryMock.Verify(x => x.Add(It.Is<Donation>(d => d.Amount == request.Amount)), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithMissingAmount_WhenAddDonation_DonationIsNotAdded()
    {
        _personDirector.PersonBuilder = _personBuilder;
        _personDirector.BuildPersonWithAllProperties("Nume Corect", "1111222233334", DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge));

        var request = new AddDonationRequest
        {
            Donor = _personBuilder.GetPerson()
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task GivenRequestWithNegativeOrZeroAmount_WhenAddDonation_DonationIsNotAdded(int amount)
    {
        _personDirector.PersonBuilder = _personBuilder;
        _personDirector.BuildPersonWithAllProperties("Nume Corect", "1111222233334", DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge));
        var request = new AddDonationRequest
        {
            Amount = amount,
            Donor = _personBuilder.GetPerson()
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public async Task GivenDonorWithInvalidName_WhenAddDonation_DonationIsNotAdded(string name)
    {
        _personDirector.PersonBuilder = _personBuilder;
        _personDirector.BuildPersonWithAllProperties(name, "1111222233334", DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge));

        var request = new AddDonationRequest
        {
            Amount = 100,
            Donor = _personBuilder.GetPerson()
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Fact]
    public async Task GivenDonorWithInvalidId_WhenAddDonation_DonationIsNotAdded()
    {
        _personDirector.PersonBuilder = _personBuilder;
        _personDirector.BuildPersonWithAllProperties("Nume Corect", "123456789012", DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge));

        var request = new AddDonationRequest
        {
            Amount=100,
            Donor = _personBuilder.GetPerson()
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Fact]
    public async Task GivenDonorWithInvalidAge_WhenAddDonation_DonationIsNotAdded()
    {
        _personDirector.PersonBuilder = _personBuilder;
        _personDirector.BuildPersonWithAllProperties("Nume Corect", "1234567890987", DateTime.Now);
        var request = new AddDonationRequest
        {
            Amount = 100,
            Donor = _personBuilder.GetPerson()
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
}
