namespace Entities;

using System;
using System.ComponentModel.DataAnnotations;
using Entities.Models;

public class User
{
    public User()
    {}

    public User(string? firstName, string? lastName, string emailId, string password, string uniqueId)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailId = emailId;
        Password = password;
        UniqueGuidId = uniqueId;
        CreatedOn = DateTime.UtcNow;
        RoleId = 2;
    }
    
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string EmailId { get; set; }

    [Required]
    public string UniqueGuidId { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public bool IsEmailVerified { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; }
}

