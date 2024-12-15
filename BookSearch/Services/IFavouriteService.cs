using BookSearch.Model;

namespace BookSearch.Services
{
    public interface IFavouriteService
    {
        public Task<ResponseResult> AddFavourite(string BookId, int UserId);
        //public Task<IEnumerable<BookStorageModel>> GetFavourite(int UserId);
        public Task<IEnumerable<BookStorageModel>> GetFavourite(int UserId);

        public Task<ResponseResult> DeleteFavourite(string GoogleBookId, int BookId);
    }
}
