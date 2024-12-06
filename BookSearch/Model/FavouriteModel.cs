using BookSearch.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class FavouriteModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public string FavouriteId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }

    // Navigation property for easier queries (optional)
    public User User { get; set; }
}
