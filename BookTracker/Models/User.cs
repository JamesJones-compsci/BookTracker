namespace BookTracker.Models;

using Microsoft.AspNetCore.Identity;

// Extend IdentityUser to add custom properties for our users
public class User : IdentityUser
{
    // For now, no extra fields
    // Later, we can add Profile info, e.g., FullName, Avatar, etc.
}