using ANT;

namespace AdsPortal.Models;

public class User : DBEntity
{
    [DBPrimaryKey] public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Location { get; set; } = null!;

    public User() { }

    public User(uint id, string name, string phone, string email, string password, string role, string location)
    {
        Id = id;
        Name = name; 
        Phone = phone;
        Email = email;
        Password = password;
        Role = role;
        Location = location;
    }
}