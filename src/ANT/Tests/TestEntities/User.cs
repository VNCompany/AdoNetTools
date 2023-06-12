using ANT;

namespace Tests.TestEntities;

public class User : DBEntity
{
    [DBPrimaryKey(true)] public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string? Password { get; set; } = null!;
}