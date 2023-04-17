using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public abstract class UserBase : IEntity<int>
{
    [Required] public string Name { get; protected set; }

    [Required] [EmailAddress] public string Email { get; protected set; }

    [Key] public int Id { get; protected set; }
    public Guid Guid { get; set; }

    public UserBase(string name, string email)
    {
        Name = name;
        Email = email;
    }
    
    #pragma warning disable CS8618
    protected  UserBase(){}
    #pragma warning restore CS8618
}