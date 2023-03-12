using Microsoft.AspNetCore.Mvc;
using PetShelter.BusinessLayer;
using PetShelter.BusinessLayer.Models;
using PetShelter.DataAccessLayer.Models;

namespace PetShelter.Api.Controllers
{
    public class DonationController:ControllerBase

    {
        private readonly IDonationService _donationService;
        private readonly ILogger<DonationController> _logger;

        public DonationController(IDonationService donationService, ILogger<DonationController> logger)
        {
            _donationService = donationService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("Donate")]
        public async Task<IActionResult> Donate([FromBody] AddDonationRequest request)
        {
            await _donationService.AddDonation(request);
            return Ok();
        }

    }
}
