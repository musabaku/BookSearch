using BookSearch.Model;
using System.ComponentModel.DataAnnotations;

public class FavouriteModel
{
    [Key]
    public string FavouriteId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }

    // Navigation property for easier queries (optional)
    public User User { get; set; }
}
