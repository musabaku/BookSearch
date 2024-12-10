using BookSearch.Data;
using BookSearch.Model;
using BookSearch.Utility;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<FavouriteService> _logger;
        public FavouriteService(ApplicationDbContext dbContext, HttpClient httpClient, ILogger<FavouriteService> logger)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ResponseResult> AddFavourite(string GoogleBookId, int UserId)
        {
            var bookExist = await _dbContext.Books.AnyAsync(b => b.GoogleBookId == GoogleBookId);
            if (bookExist == false)
            {
                string url = $"https://www.googleapis.com/books/v1/volumes/{GoogleBookId}?key={API_KEY}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                _logger.LogInformation($"Book received from api is {response}");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseResult(false, "Failed to fetch book id from google");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Book converted to string is {responseString}");

                var result = JsonSerializer.Deserialize<Book>(responseString);
                _logger.LogInformation($"Book deserialized {result}");

                if (result == null)
                {
                    return new ResponseResult(false, "Failed to deserialize book");
                }
                var bookDto = DtoMapper.BookToDto(result);
                _logger.LogInformation($"Book converted to Bookdto {bookDto}");

                var bookStorageModel = DtoMapper.BookDtoToModel(bookDto);
                _logger.LogInformation($"Book converted to bookStorageModel {bookStorageModel}");

                await _dbContext.Books.AddAsync(bookStorageModel);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Book added to database");

            }


            //var favouriteExist = await _dbContext.Favorites.FirstOrDefaultAsync(b => b.BookId == GoogleBookId && b.UserId == UserId);
            //if(favouriteExist != null)
            //{
            //    return new ResponseResult(false, "This book is already in your favorites");
            //}

            //var favouriteAdd = new FavouriteModel { UserId = UserId , BookId = GoogleBookId};
            //  await _dbContext.Favorites.AddAsync(favouriteAdd);
            //  await _dbContext.SaveChangesAsync();

            //return new ResponseResult(true,"This book is added to your favourites");

            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.GoogleBookId == GoogleBookId);
            if (book == null)
            {
                return new ResponseResult(false, "Failed to find the book in the database.");
            }

            var favouriteExist = await _dbContext.Favorites.FirstOrDefaultAsync(b => b.BookId == book.BookId && b.UserId == UserId);
            if (favouriteExist != null)
            {
                return new ResponseResult(false, "This book is already in your favorites");
            }

            var favouriteAdd = new FavouriteModel { UserId = UserId, BookId = book.BookId };  // Use the BookId from the database, not GoogleBookId
            await _dbContext.Favorites.AddAsync(favouriteAdd);
            await _dbContext.SaveChangesAsync();

            return new ResponseResult(true, "This book is added to your favourites");



        }
        public async Task<IEnumerable<BookStorageModel>> GetFavourite(int UserId)
        {

          var favouriteList = await  _dbContext.Favorites.Where(f => f.UserId == UserId).Select(f => f.BookId).ToListAsync();

            var books = await _dbContext.Books.Where(b=> favouriteList.Contains(b.BookId)).ToListAsync();

            return books;

        }
        public async Task<ResponseResult> DeleteFavourite(string GoogleBookId, int UserId)
        {
          var bookIdDelete =  await _dbContext.Books.Where(b => b.GoogleBookId == GoogleBookId).Select(b=>b.BookId).FirstOrDefaultAsync();
            if (bookIdDelete == 0)
            {
                return new ResponseResult(false, "Book not found");
            }
            var favouriteDelete = await _dbContext.Favorites.FirstOrDefaultAsync(f => f.BookId == bookIdDelete && f.UserId == UserId);
            if(favouriteDelete == null)
            {
                return new ResponseResult(false, "Favourite record not found");
            }
             _dbContext.Remove(favouriteDelete);
            await _dbContext.SaveChangesAsync();
            return new ResponseResult(true, "Favourite Deleted Successfully!!");
        }
    }
}
