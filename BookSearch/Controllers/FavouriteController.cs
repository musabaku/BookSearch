using BookSearch.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BookSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
        
    {
        private readonly IFavouriteService _favouriteService;
        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }
        [HttpPost("favourites/{GoogleBookId}")]
        public async Task<IActionResult> AddFavourite([FromRoute] string GoogleBookId)
        {
            var UserIdString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(UserIdString))
            {
                return BadRequest("User not found, Please login");
            }
            if (!int.TryParse(UserIdString, out var UserId))
            {
                return BadRequest("Invalid User ID");
            }

            var response = await _favouriteService.AddFavourite(GoogleBookId, UserId);
            if (!response.IsSuccessful)
            {
                return BadRequest(response.message);
            }
            
            return Ok(response);

        }
        
    }
}
