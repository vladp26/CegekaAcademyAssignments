using PetShelter.BusinessLayer.Models;

namespace PetShelter.BusinessLayer;

public class AddDonationRequest
{
    public decimal Amount { get; set; }

    public Person Donor { get; set; }
}