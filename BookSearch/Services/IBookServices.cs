using BookSearch.Model;

namespace BookSearch.Services
{
    public interface IBookServices
    {
        Task<IEnumerable<Book>> SearchByQuery(string query);
    }
}
