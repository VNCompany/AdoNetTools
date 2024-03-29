using ANT.ORM;

namespace Tests.TestEntities;

public class User : DbEntity
{
    [DbField(IsPrimaryKey = true)]
    public uint Id { get; set; }

    public string Name { get; set; } = null!;
}