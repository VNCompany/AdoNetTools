using ANT;

namespace Tests.TestEntities;

public class User : DBEntity
{
    [DBPrimaryKey]
    public int Id { get; set; }
}