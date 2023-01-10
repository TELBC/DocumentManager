﻿using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class User : UserBase
{
    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    #pragma warning disable CS8618
    protected User() { }
    #pragma warning restore CS8618
    
    [Required]
    [MaxLength(255)]
    private string Password { get; set; }
}

//updated to what is required in the project