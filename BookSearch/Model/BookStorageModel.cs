using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookSearch.Model
{
    public class BookStorageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        public string GoogleBookId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string PublishedDate { get; set; }
        public string ImageLinks { get; set; }
    }
}
