using MySql.Data.MySqlClient;

using AdsPortal.Models;
using AdsPortal.ViewModels;

namespace AdsPortal;

public class DbContext : IDisposable
{
    private readonly MySqlConnection _conn;
    
    public DbContext(string connectionString)
    {
        _conn = new MySqlConnection(connectionString);
        _conn.Open();
    }

    public MySqlCommand CreateCommand() => _conn.CreateCommand();

    public MySqlCommand CreateCommand(string command) => new MySqlCommand(command, _conn);

    public User? Login(string phone, string password)
    {
        using var cmd = CreateCommand("SELECT * FROM users WHERE phone=@phone AND password=@password");
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Prepare();
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new User(
                reader.GetUInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5));
        }
        return null;
    }

    public void UpdatePassword(uint id, string newPassword)
    {
        using var cmd = CreateCommand("UPDATE users SET password=@newPassword WHERE id=@id");
        cmd.Parameters.AddWithValue("@newPassword", newPassword);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public void UpdateUserInfo(uint id, string phone, string email, string location)
    {
        using var cmd = CreateCommand("UPDATE users SET phone=@phone, email=@email, location=@location WHERE id=@id");
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public bool Register(string phone, string password)
    {
        using (var cmd = CreateCommand("SELECT COUNT(*) FROM users WHERE phone=@phone"))
        {
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Prepare();
            long usersCount = (long)cmd.ExecuteScalar();
            if (usersCount > 0)
                return false;
        }

        using (var cmd = CreateCommand("INSERT INTO users (phone, password) VALUES (@phone, @password)"))
        {
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            return true;
        }
    }

    public CatalogViewModel GetCatalog()
    {
        CatalogViewModel cvm = new CatalogViewModel();
        using (var cmd = CreateCommand("SELECT * FROM cats"))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cvm.Add(new KeyValuePair<uint, string>(reader.GetUInt32(0), reader.GetString(1)));
                }
            }
        }

        return cvm;
    }

    public void AddPost(uint userId, uint catId, string name, string description, string price, string location, 
        string pictureFileName)
    {
        uint lastId;
        
        using (var cmd = CreateCommand(
                   "INSERT INTO posts (user_id, cat_id, name, description, price, location) " +
                   "VALUES (@userId, @catId, @name, @description, @price, @location);"))
        {
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@catId", catId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            lastId = (uint)cmd.LastInsertedId;
        }

        using (var cmd = CreateCommand("INSERT INTO pictures (name, post_id) VALUES (@name, @postId);"))
        {
            cmd.Parameters.AddWithValue("@name", pictureFileName);
            cmd.Parameters.AddWithValue("@postId", lastId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }

    private Post GetPostFromDataReader(MySqlDataReader reader)
    {
        return new Post(
            reader.GetUInt32(0),
            reader.GetUInt32(1),
            reader.GetUInt32(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetString(5),
            reader.GetString(6),
            reader.GetDateTime(7),
            reader.GetString(8));
    }

    public IEnumerable<Post> GetPosts(uint? catFilter = null, string? searchFilter = null)
    {
        using var cmd = CreateCommand(
            "SELECT p.*, pic.name FROM `posts` p INNER JOIN pictures pic ON pic.post_id = p.id " +
            (catFilter != null ? $"WHERE p.cat_id='{catFilter}'" : "") + ";");
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return GetPostFromDataReader(reader);
        }
    }

    public Post? GetPost(uint postId)
    {
        using var cmd =
            CreateCommand(
                "SELECT p.*, pic.name FROM `posts` p INNER JOIN pictures pic ON pic.post_id = p.id " +
                "WHERE p.id=@postId;");
        cmd.Parameters.AddWithValue("@postId", postId);
        cmd.Prepare();
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return GetPostFromDataReader(reader);
        }

        return null;
    }

    public User? GetUserById(uint userId)
    {
        using var cmd = CreateCommand("SELECT * FROM users WHERE id=@id");
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Prepare();
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new User(
                reader.GetUInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5));
        }
        return null;
    }

    public void Dispose()
    {
        _conn.Dispose();
    }
}