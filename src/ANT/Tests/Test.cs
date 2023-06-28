using MySql.Data.MySqlClient;
using System.Data.Common;

using ANT.ORM;
using Tests.TestEntities;

namespace Tests;

[TestFixture]
public class Test
{
    [Test]
    public void TestSql()
    {
        AntProvider.RegisterClass<Post>();
        
        using (var conn = new MySqlConnection("Server=127.0.0.1;User=root;Password=root;Database=test"))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT p.*, u.* FROM posts p INNER JOIN users u ON u.id = p.user_id;";
                var posts = cmd.ExecuteQuery<Post>().ToList();
            }
        }
    }
}