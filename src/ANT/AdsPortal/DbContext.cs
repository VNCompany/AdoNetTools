using MySql.Data.MySqlClient;
using ANT;
using ANT.Configurators;

using AdsPortal.Models;
using AdsPortal.Repositories;

namespace AdsPortal;

public class DbContext : IDisposable
{
    private readonly MySqlConnection _conn;

    static DbContext()
    {
        ANTProvider.Configure(new MySqlConfigurator());
        ANTProvider.RegisterClass<User>();
        ANTProvider.RegisterClass<Post>();
    }
    
    public DbContext(string connectionString)
    {
        _conn = new MySqlConnection(connectionString);
        _conn.Open();
    }

    public MySqlCommand CreateCommand() => _conn.CreateCommand();

    public MySqlCommand CreateCommand(string command) => new MySqlCommand(command, _conn);

    private UsersRepository? _usersRepository;

    public UsersRepository Users
    {
        get
        {
            if (_usersRepository == null)
                _usersRepository = new UsersRepository() { Db = this };
            return _usersRepository;
        }
    }

    private PostsRepository? _postsRepository;

    public PostsRepository Posts
    {
        get
        {
            if (_postsRepository == null)
                _postsRepository = new PostsRepository() { Db = this };
            return _postsRepository;
        }
    }

    public void Dispose()
    {
        _conn.Dispose();
    }
}