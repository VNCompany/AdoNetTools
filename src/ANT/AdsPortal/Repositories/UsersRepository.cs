using ANT.Extensions;

using AdsPortal.Models;

namespace AdsPortal.Repositories;

public class UsersRepository : BaseRepository
{
    public List<User> GetAll()
    {
        var users = new List<User>();
        using var cmd = Db.CreateCommand("SELECT * FROM users");
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            users.Add(reader.ToEntity<User>());

        return users;
    }
    
    public User? Get(string phone, string password)
    {
        using var cmd = Db.CreateCommand("SELECT * FROM users WHERE phone=@phone AND password=@password");
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Prepare();
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return reader.ToEntity<User>();
        return null;
    }
    
    public User? Get(uint userId)
    {
        using var cmd = Db.CreateCommand("SELECT * FROM users WHERE id=@id");
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Prepare();
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return reader.ToEntity<User>();
        return null;
    }
    
    public bool Add(string name, string phone, string password)
    {
        using (var cmd = Db.CreateCommand("SELECT COUNT(*) FROM users WHERE phone=@phone"))
        {
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Prepare();
            long usersCount = (long)cmd.ExecuteScalar();
            if (usersCount > 0)
                return false;
        }

        using (var cmd = Db.CreateCommand("INSERT INTO users (name, phone, password) VALUES (@name, @phone, @password)"))
        {
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            return true;
        }
    }

    public void UpdatePassword(uint id, string newPassword)
    {
        using var cmd = Db.CreateCommand("UPDATE users SET password=@newPassword WHERE id=@id");
        cmd.Parameters.AddWithValue("@newPassword", newPassword);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public void UpdateUserInfo(uint id, string name, string phone, string email, string location)
    {
        using var cmd = Db.CreateCommand("UPDATE users SET " +
                                         "name=@name, phone=@phone, email=@email, location=@location WHERE id=@id");
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }
}