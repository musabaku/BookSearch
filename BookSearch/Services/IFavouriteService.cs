namespace BookSearch.Services
{
    public interface IFavouriteService
    {
        public Task<ResponseResult> AddFavourite(string BookId, int UserId);
    }
}
