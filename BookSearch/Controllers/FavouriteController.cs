using BookSearch.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace BookSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        private readonly ILogger<FavouriteController> _logger;

        public FavouriteController(IFavouriteService favouriteService, ILogger<FavouriteController> logger)
        {
            _favouriteService = favouriteService;
            _logger = logger;
        }

        [HttpPost("favourites/{GoogleBookId}")]
        public async Task<IActionResult> AddFavourite([FromRoute] string GoogleBookId)
        {
            // Log the incoming request headers for debugging
            _logger.LogInformation("Incoming request headers: {@Headers}", Request.Headers);

            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized("User is not authenticated");
            }

            // Log all claims for debugging
            _logger.LogInformation("Claims from the JWT token:");
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            // Get the user ID from the token
            //var UserIdString = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            // Try to get User ID from the "nameidentifier" claim (since it's present in the JWT token)
            var UserIdString = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            // Log the User ID (if available)
            _logger.LogInformation("User ID from 'nameidentifier' claim: {UserId}", UserIdString);

            // Log the user ID (if available)
            _logger.LogInformation("User ID from token: {UserId}", UserIdString);

            // Check if the User ID is null or empty
            if (string.IsNullOrEmpty(UserIdString))
            {
                _logger.LogWarning("User ID is null or empty. User may not be logged in.");
                return BadRequest("User not found, Please login");
            }

            // Try to parse the user ID
            if (!int.TryParse(UserIdString, out var UserId))
            {
                _logger.LogWarning("Failed to parse User ID: {UserIdString}", UserIdString);
                return BadRequest("Invalid User ID");
            }

            // Call the favourite service to add the favourite
            var response = await _favouriteService.AddFavourite(GoogleBookId, UserId);
            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to add favourite. Error: {ErrorMessage}", response.message);
                return BadRequest(response.message);
            }

            _logger.LogInformation("Successfully added favourite.");
            return Ok(response);
        }
        [HttpGet("favourites/allfavourite")]
        public async Task<IActionResult> GetFavourite()
        {
            const response = _favouriteService.GetFavourite();
          }
    }
   

}
