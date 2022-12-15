using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Model;

public class User : Guest
{
    public String Username { get; set; }
    public String Email { get; set; }
    public List<User> Friends { get; set; }

    public User(string username,string email, List<User> friends) : base(username)
    {
        Username = username;
        Email = email;
        Friends = friends;
    }
}