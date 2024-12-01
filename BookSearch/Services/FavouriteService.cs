using BookSearch.Data;
using BookSearch.Model;
using BookSearch.Utility;
using Microsoft.EntityFrameworkCore;

using System.Text.Json;
using System.Text.Json.Serialization;
namespace BookSearch.Services
{

    public class ResponseResult
    {

        public bool IsSuccessful { get; set; }
        public string message { get; set; }


        public ResponseResult(bool IsSuccessful,string message) {
        this.IsSuccessful = IsSuccessful;
            this.message = message; 
        }
   
    }

    public class FavouriteService : IFavouriteService
    {
        private const string API_KEY = "AIzaSyBoRiNZ7Y9gsGz7qG39btVirkz1u-2G9vo";
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;
        public FavouriteService(ApplicationDbContext dbContext, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
        }
        public async Task<ResponseResult> AddFavourite(string GoogleBookId, int UserId)
        {
            var bookExist = await _dbContext.Books.AnyAsync(b => b.GoogleBookId == GoogleBookId);
            if (bookExist == false)
            {
                string url = $"https://www.googleapis.com/books/v1/volumes/{GoogleBookId}?key={API_KEY}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseResult(false, "Failed to fetch book id from google");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Book>(responseString);
                if (result == null)
                {
                    return new ResponseResult(false, "Failed to deserialize book");
                }
                var bookDto = DtoMapper.BookToDto(result);
                var bookStorageModel = DtoMapper.BookDtoToModel(bookDto);
                await _dbContext.Books.AddAsync(bookStorageModel);
                await _dbContext.SaveChangesAsync();

            }
       

           var favouriteExist = await _dbContext.Favorites.FirstOrDefaultAsync(b => b.BookId == GoogleBookId && b.UserId == UserId);
            if(favouriteExist != null)
            {
                return new ResponseResult(false, "This book is already in your favorites");
            }

            var favouriteAdd = new FavouriteModel { UserId = UserId , BookId = GoogleBookId};
              await _dbContext.Favorites.AddAsync(favouriteAdd);
              await _dbContext.SaveChangesAsync();

            return new ResponseResult(true,"This book is added to your favourites");
            



        }
    }
}
