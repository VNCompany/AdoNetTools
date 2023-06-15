using System.Text;
using ANT.Extensions;

using AdsPortal.Models;

namespace AdsPortal.Repositories;

public class PostsRepository : BaseRepository
{
    public List<(uint, string)> GetCatalog()
    {
        var categories = new List<(uint, string)>();
        using (var cmd = Db.CreateCommand("SELECT * FROM cats"))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    categories.Add((reader.GetUInt32(0), reader.GetString(1)));
                }
            }
        }

        return categories;
    }

    public List<(uint, string)> GetPictures(uint? postId = null)
    {
        var list = new List<(uint, string)>();
        string commandText = "SELECT post_id, name FROM pictures";
        if (postId != null)
            commandText += $" WHERE post_id={postId}";
        using (var cmd = Db.CreateCommand(commandText + ";"))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    list.Add((reader.GetUInt32(0), reader.GetString(1)));
            }
        }

        return list;
    }

    public List<Post> GetAll(uint? catFilter = null, string? searchFilter = null)
    {
        var pictures = GetPictures();
        var users = Db.Users.GetAll();
        var posts = new List<Post>();
        using var cmd = Db.CreateCommand();
        StringBuilder queryStringBuilder = new StringBuilder("SELECT * FROM posts");
        if (catFilter != null || searchFilter != null)
        {
            queryStringBuilder.Append(" WHERE ");
            if (catFilter != null)
            {
                cmd.Parameters.AddWithValue("@catId", catFilter);
                queryStringBuilder.Append("cat_id=@catId");
            }
            else if (searchFilter != null)
            {
                cmd.Parameters.AddWithValue("@searchText", '%' + searchFilter + '%');
                queryStringBuilder.Append("name LIKE @searchText");
            }
            cmd.Prepare();
        }

        queryStringBuilder.Append(';');
        cmd.CommandText = queryStringBuilder.ToString();
        
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var post = reader.ToEntity<Post>();
            post.Owner = users.First(u => u.Id == post.UserId);
            post.Pictures = pictures.Where(p => p.Item1 == post.Id).Select(p => p.Item2).ToList();
            posts.Add(post);
        }
        
        return posts;
    }

    public Post? Get(uint postId)
    {
        Post? post = null;

        using (var cmd = Db.CreateCommand("SELECT * FROM posts WHERE id=@postId;"))
        {
            cmd.Parameters.AddWithValue("@postId", postId);
            cmd.Prepare();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                post = reader.ToEntity<Post>();
            }
        }

        if (post != null)
        {
            post.Owner = Db.Users.Get(post.UserId) ?? throw new NullReferenceException("Owner");
            post.Pictures = GetPictures(post.Id).Select(p => p.Item2).ToList();
        }

        return post;
    }

    public List<(uint, string)> GetUserPosts(uint userId)
    {
        var list = new List<(uint, string)>();
        using (var cmd = Db.CreateCommand("SELECT id, name FROM posts WHERE user_id=@userId"))
        {
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Prepare();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    list.Add((reader.GetUInt32(0), reader.GetString(1)));
            }
        }

        return list;
    }

    public void AddPicture(uint postId, string fileName)
    {
        using var cmd = Db.CreateCommand("INSERT INTO pictures (name, post_id) VALUES " +
                                         "(@fileName, @postId)");
        cmd.Parameters.AddWithValue("@fileName", fileName);
        cmd.Parameters.AddWithValue("@postId", postId);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }
    
    public uint Add(uint userId, uint catId, string name, string description, string price, string location)
    {
        using (var cmd = Db.CreateCommand(
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
            return (uint)cmd.LastInsertedId;
        }
    }

    public void Update(uint postId, uint catId, string name, string description, string price, string location)
    {
        using (var cmd = Db.CreateCommand(
                   "UPDATE posts SET " +
                   "name=@name,description=@description,price=@price,location=@location,cat_id=@catId " +
                   "WHERE id=@postId"))
        {
            cmd.Parameters.AddWithValue("@postId", postId);
            cmd.Parameters.AddWithValue("@catId", catId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }

    public void Delete(uint postId)
    {
        using var cmd = Db.CreateCommand("DELETE FROM posts WHERE id=@postId");
        cmd.Parameters.AddWithValue("@postId", postId);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public void DeletePictures(uint postId)
    {
        using var cmd = Db.CreateCommand("DELETE FROM pictures WHERE post_id=@postId");
        cmd.Parameters.AddWithValue("@postId", postId);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }
}