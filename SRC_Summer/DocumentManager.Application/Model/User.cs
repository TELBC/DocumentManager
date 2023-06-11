using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DocumentManager.Model;

public class User : UserBase
{
    public User(string name, string email, string password) : base(name,email)
    {
        Name = name;
        Email = email;
        SetPassword(password);
    }
#pragma warning disable CS8618
    protected User()
    {
    }
#pragma warning restore CS8618
    public string Salt { get; set; }
    public string PasswordHash { get; set; }

    [MemberNotNull(nameof(Salt), nameof(PasswordHash))]  
    public void SetPassword(string password)
    {
        Salt = GenerateRandomSalt();
        PasswordHash = CalculateHash(password, Salt);
    }
    public bool CheckPassword(string password) => PasswordHash == CalculateHash(password, Salt);
    private string GenerateRandomSalt(int length = 128)
    {
        byte[] salt = new byte[length / 8];
        using (System.Security.Cryptography.RandomNumberGenerator rnd =
               System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rnd.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }
    private string CalculateHash(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        System.Security.Cryptography.HMACSHA256 myHash =
            new System.Security.Cryptography.HMACSHA256(saltBytes);
        byte[] hashedData = myHash.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashedData);
    }
}