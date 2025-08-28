namespace BookTracker.Models;

using System;
using System.ComponentModel.DataAnnotations;
public class Review
{
    public int Id { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    [Required(ErrorMessage = "Rating is required.")]
    public int Rating { get; set; }

    public string Comment { get; set; } // Optional

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now; // Default for migrations

    // Foreign key to Book
    [Required]
    public int BookId { get; set; }

    public Book Book { get; set; }

    // Foreign key to User
    [Required]
    public string UserId { get; set; }

    public User User { get; set; }
}