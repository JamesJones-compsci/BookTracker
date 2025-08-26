namespace BookTracker.Models;

using System;
using System.ComponentModel.DataAnnotations;
public class Review
{
    public int Id { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    public string Comment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation Properties
    public int BookId { get; set; }
    public Book Book { get; set; }
    
    public string UserId { get; set; }
    public User User { get; set; }
}