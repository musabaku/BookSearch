using BookSearch.Model;
using BookSearch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSearchController : ControllerBase
    {
        private readonly  IBookServices _bookServices;
        public BookSearchController(IBookServices bookServices) {
            _bookServices = bookServices;
   
        }
        [HttpGet("search")]   
        public async Task<ActionResult<IEnumerable<Book>>> SearchByQuery([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Give query Parameter");
            }
            var books = await _bookServices.SearchByQuery(query);
            if (books == null || !books.Any())
            {
                return NotFound("No books found matching the query.");
            }
            return Ok(books);
        }
    }
}
