using BookSearch.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static BookSearch.Model.BookResponse;

namespace BookSearch.Services
{
    public class BookServices : IBookServices
    {
        private readonly HttpClient _httpClient;
        private const string API_KEY = "AIzaSyBoRiNZ7Y9gsGz7qG39btVirkz1u-2G9vo";

        public BookServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Book>> SearchByQuery(string query)
        {
            //checking if query is in right format or not
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("Write correct query", nameof(query));
            }
            //calling api
            string url = $"https://www.googleapis.com/books/v1/volumes?q={query}&key={API_KEY}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            //storing result in respone and throwiing error if not succesffull below
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Process failed try again!!");
            }
            // the response content needs to be converted to string
            var responseString = await response.Content.ReadAsStringAsync();
            //deserialising response and storing in result
            var result = JsonSerializer.Deserialize<BookResponse>(responseString);
            // result is of type bookresponse which has a arraylist field Books, returning if not an empty list;
            return result?.Items ?? new List<Book>();

        }
    }
}

//using BookSearch.Model;
//using Microsoft.Extensions.Logging;
//using System.Net.Http.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using static BookSearch.Model.BookResponse;

//namespace BookSearch.Services
//{
//    public class BookServices : IBookServices
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<BookServices> _logger;
//        private const string API_KEY = "AIzaSyBoRiNZ7Y9gsGz7qG39btVirkz1u-2G9vo";

//        // Constructor with logging support
//        public BookServices(HttpClient httpClient, ILogger<BookServices> logger)
//        {
//            _httpClient = httpClient;
//            _logger = logger;
//        }

//        public async Task<IEnumerable<Book>> SearchByQuery(string query)
//        {
//            // Check if query is null or empty and log
//            if (string.IsNullOrEmpty(query))
//            {
//                _logger.LogWarning("Query parameter is empty or null.");
//                throw new ArgumentException("Write correct query", nameof(query));
//            }

//            try
//            {
//                // Build the API URL and log
//                string url = $"https://www.googleapis.com/books/v1/volumes?q={query}&key={API_KEY}";
//                _logger.LogInformation("Making request to Google Books API with URL: {Url}", url);

//                // Make the request
//                HttpResponseMessage response = await _httpClient.GetAsync(url);

//                // Check if the response is successful
//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogError("Request failed with status code {StatusCode}", response.StatusCode);
//                    throw new Exception("Process failed, try again!");
//                }

//                // Read the response content
//                var responseString = await response.Content.ReadAsStringAsync();
//                _logger.LogInformation("Received response: {ResponseString}", responseString);

//                // Deserialize the response to BookResponse
//                var result = JsonSerializer.Deserialize<BookResponse>(responseString);

//                // Log the number of books found or a message if empty
//                if (result?.Items?.Any() == true)
//                {
//                    _logger.LogInformation("Found {BookCount} books matching the query.", result.Items.Count);
//                }
//                else
//                {
//                    _logger.LogInformation("No books found for the query.");
//                }

//                // Return the list of books or an empty list
//                return result?.Items ?? new List<Book>();
//            }
//            catch (Exception ex)
//            {
//                // Catch all exceptions and log them
//                _logger.LogError(ex, "An error occurred while searching for books.");
//                throw;  // Re-throw the exception to propagate it
//            }
//        }
//    }
//}


//using BookSearch.Model;
//using Microsoft.Extensions.Logging;
//using System.Net.Http.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using static BookSearch.Model.BookResponse;

//namespace BookSearch.Services
//{
//    public class BookServices : IBookServices
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ILogger<BookServices> _logger;
//        private const string API_KEY = "AIzaSyBoRiNZ7Y9gsGz7qG39btVirkz1u-2G9vo";

//        // Constructor with HttpClient and ILogger dependency injection
//        public BookServices(HttpClient httpClient, ILogger<BookServices> logger)
//        {
//            _httpClient = httpClient;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Searches books from the Google Books API based on a query.
//        /// </summary>
//        /// <param name="query">The search query string.</param>
//        /// <returns>A list of books matching the query.</returns>
//        /// <exception cref="ArgumentException">Thrown when the query is null or empty.</exception>
//        /// <exception cref="Exception">Thrown when the API request fails.</exception>
//        public async Task<IEnumerable<Book>> SearchByQuery(string query)
//        {
//            // Validate input and log
//            if (string.IsNullOrWhiteSpace(query))
//            {
//                _logger.LogWarning("Search query is empty or null. A valid query must be provided.");
//                throw new ArgumentException("Query parameter cannot be null or empty.", nameof(query));
//            }

//            try
//            {
//                // Build the API request URL
//                string url = $"https://www.googleapis.com/books/v1/volumes?q={query}&key={API_KEY}";
//                _logger.LogInformation("Requesting data from Google Books API. URL: {Url}", url);

//                // Send GET request to the API
//                HttpResponseMessage response = await _httpClient.GetAsync(url);

//                // Check if the response is successful
//                if (!response.IsSuccessStatusCode)
//                {
//                    _logger.LogError("Failed to retrieve data from API. Status Code: {StatusCode}, Reason: {ReasonPhrase}",
//                        response.StatusCode, response.ReasonPhrase);
//                    throw new Exception($"API request failed with status code {response.StatusCode}.");
//                }

//                // Read the response content as a string
//                string responseString = await response.Content.ReadAsStringAsync();
//                _logger.LogDebug("Raw API response: {ResponseString}", responseString);

//                // Deserialize the response into BookResponse
//                var result = JsonSerializer.Deserialize<BookResponse>(responseString);

//                // Check if items were retrieved and log
//                if (result?.Items?.Any() == true)
//                {
//                    _logger.LogInformation("Successfully retrieved {BookCount} books from the API.", result.Items.Count);
//                }
//                else
//                {
//                    _logger.LogInformation("No books found for the query: {Query}", query);
//                }

//                // Return the list of books or an empty list
//                return result?.Items ?? new List<Book>();
//            }
//            catch (JsonException jsonEx)
//            {
//                _logger.LogError(jsonEx, "Error while deserializing the API response.");
//                throw new Exception("An error occurred while processing the API response. Please try again later.", jsonEx);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An unexpected error occurred while searching for books.");
//                throw; // Re-throw the exception to propagate it
//            }
//        }
//    }
//}
