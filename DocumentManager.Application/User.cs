using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager;

public class User : UserBase
{
    public string Password { get; set; }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}