using Azure.Core;
using Moq;
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

    public AddDonationTests()
    {
        _mockPersonService= new Mock<IPersonService>();
        _donationRepositoryMock = new Mock<IDonationRepository>();
        _donationServiceSut = new DonationService(_donationRepositoryMock.Object, _mockPersonService.Object, new AddDonationRequestValidator());
    }

    [Fact]
    public async Task GivenValidRequest_WhenAddDonation_DonationIsAdded()
    {
        var request = new AddDonationRequest
        {
            Amount = 10,
            Donor = new PetShelter.BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge),
                IdNumber = "1111222233334",
                Name = "Nume Corect"
            }
        };
        await _donationServiceSut.AddDonation(request);

        _donationRepositoryMock.Verify(x => x.Add(It.Is<Donation>(d => d.Amount == request.Amount)), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithMissingAmount_WhenAddDonation_DonationIsNotAdded()
    {
        var request = new AddDonationRequest
        {
            Donor = new PetShelter.BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge),
                IdNumber = "1111222233334",
                Name = "Nume Corect"
            }
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task GivenRequestWithNegativeOrZeroAmount_WhenAddDonation_DonationIsNotAdded(int amount)
    {
        var request = new AddDonationRequest
        {
            Amount = amount,
            Donor = new PetShelter.BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge),
                IdNumber = "1111222233334",
                Name = "Nume Corect"
            }
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public async Task GivenDonorWithInvalidName_WhenAddDonation_DonationIsNotAdded(string name)
    {
        var request = new AddDonationRequest
        {
            Donor = new PetShelter.BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge),
                IdNumber = "1111222233334",
                Name = name
            }
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Fact]
    public async Task GivenDonorWithInvalidId_WhenAddDonation_DonationIsNotAdded()
    {
        var request = new AddDonationRequest
        {
            Donor = new PetShelter.BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-Constants.PersonConstants.AdultMinAge),
                IdNumber = "1234567890123",
                Name = "Nume Corect"
            }
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
    [Fact]
    public async Task GivenDonorWithInvalidAge_WhenAddDonation_DonationIsNotAdded()
    {
        var request = new AddDonationRequest
        {
            Donor = new PetShelter.BusinessLayer.Models.Person
            {
                DateOfBirth = DateTime.Now,
                IdNumber = "1234567890987",
                Name = "Nume Corect"
            }
        };
        await Assert.ThrowsAsync<ArgumentException>(() => _donationServiceSut.AddDonation(request));

        _donationRepositoryMock.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }
}
