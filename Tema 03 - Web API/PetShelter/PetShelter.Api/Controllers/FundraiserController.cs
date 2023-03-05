using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Resources;
using PetShelter.Api.Resources.Extensions;
using PetShelter.Domain.Services;
using System.Collections.Immutable;

namespace PetShelter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FundraiserController:ControllerBase
    {
        private readonly IFundraiserService _fundraiserService;
        public FundraiserController(IFundraiserService fundraiserService)
        {
            _fundraiserService = fundraiserService;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Fundraiser>> Get(int id)
        {
            var fundraiser = await this._fundraiserService.GetFundraiser(id);
            if (fundraiser is null)
            {
                return this.NotFound();
            }

            return this.Ok(fundraiser);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IReadOnlyList<Fundraiser>>> GetPets()
        {
            var data = await this._fundraiserService.GetAllFundraisers();
            return this.Ok(data.ToImmutableArray());
        }
        [HttpPut("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteFundraiser(int id)
        {
            await this._fundraiserService.DeleteFundraiserAsync(id);

            return this.NoContent();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddFundraiser([FromBody] Fundraiser fundraiser)
        {
            var id = await _fundraiserService.CreateFundraiserAsync(fundraiser.Owner.AsDomainModel(), fundraiser.AsDomainModel());
            return CreatedAtRoute(nameof(AddFundraiser), id);
        }
        [HttpPut("{fundraiserId}/donate/{donorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DonateToFundraiser(int fundraiserId, int donorId, [FromBody] FakeDonation donation)
        {
            await _fundraiserService.DonateToFundraiserAsync(fundraiserId, donorId, donation.Amount);
            return this.NoContent();
        }
    }
}
