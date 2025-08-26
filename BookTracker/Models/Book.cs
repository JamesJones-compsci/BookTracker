namespace BookTracker.Models;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Book
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    public string Genre { get; set; }
    public int Year { get; set; }
    
    // Navigation Properties
    public string UserId { get; set; }
    public User User { get; set; }
    
    public List<Review> Reviews {get; set;}
}