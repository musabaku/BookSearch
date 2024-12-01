using BookSearch.Dto;
using BookSearch.Model;

namespace BookSearch.Utility
{
    public class DtoMapper
    {
        public static BookDto BookToDto(Book ApiBook)
        {
            return new BookDto
            {
                GoogleBookId = ApiBook.Id,
                Title = ApiBook.VolumeInfo.Title,
                Authors = ApiBook.VolumeInfo.FirstAuthor,
                ImageLink = ApiBook.VolumeInfo.ImageLinks.Thumbnail,
                PublishedDate = ApiBook.VolumeInfo.PublishedDate

            };

        }

        public static BookStorageModel BookDtoToModel(BookDto Dto)
        {
            return new BookStorageModel
            {
                GoogleBookId = Dto.GoogleBookId,
                Title = Dto.Title,
                Authors = Dto.Authors,
                ImageLinks = Dto.ImageLink,
                PublishedDate = Dto.PublishedDate,
            };
        }

        //public static FavouriteDto FavourtieModelToDto(BookStorageModel bookStorageModel)
        //{
        //    return new FavouriteDto
        //    {
        //        GoogleBookId = bookStorageModel.GoogleBookId,
        //        Title = bookStorageModel.Title,
        //        Authors = bookStorageModel.Authors,
        //        ImageLink = bookStorageModel.ImageLinks,
        //        PublishedDate = bookStorageModel.PublishedDate,

        //    };
        //}
        //public static FavouriteModel ToFavouriteModel(AddFavouriteDto FavouriteDto)
        //{
        //    return new FavouriteModel
        //    {
        //        UserId = FavouriteDto.UserId,
        //        BookId = FavouriteDto.GoogleBookId
        //    };
        //}
    }

}
