using MySql.Data.MySqlClient;

namespace AdsPortal.Repositories;

public abstract class BaseRepository
{
    public DbContext Db { get; init; } = null!;
}