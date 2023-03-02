using FluentValidation;
using PetShelter.BusinessLayer.Constants;
using PetShelter.BusinessLayer;
namespace PetShelter.BusinessLayer.Validators;

public class AddDonationRequestValidator: AbstractValidator<AddDonationRequest>
{
	public AddDonationRequestValidator()
	{
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Donor).SetValidator(new PersonValidator());
    }
}
