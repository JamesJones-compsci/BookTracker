namespace BookTracker.Models;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Author is required.")]
    public string Author { get; set; }

    public string Genre { get; set; }  // Optional

    // Make Year nullable to allow empty form submission
    [Range(0, 9999, ErrorMessage = "Enter a valid year.")]
    public int? Year { get; set; }

    // UserId is required and links to currently logged-in user
    [Required]
    public string UserId { get; set; }

    // Navigation property
    public User User { get; set; }

    // Navigation to related Reviews
    public List<Review> Reviews { get; set; } = new List<Review>();
}