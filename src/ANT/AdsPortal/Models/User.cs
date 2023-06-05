namespace AdsPortal.Models;

public class User
{
    public uint Id { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Location { get; set; }

    public User(uint id, string phone, string email, string password, string role, string location)
    {
        Id = id;
        Phone = phone;
        Email = email;
        Password = password;
        Role = role;
        Location = location;
    }
}