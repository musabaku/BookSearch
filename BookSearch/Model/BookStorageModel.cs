using System.ComponentModel.DataAnnotations;
namespace BookSearch.Model
{
    public class BookStorageModel
    {
        [Key]
        public string BookId { get; set; }
        public string GoogleBookId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string PublishedDate { get; set; }
        public string ImageLinks { get; set; }
    }
}
