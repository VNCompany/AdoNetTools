using ANT.ORM;

namespace Tests.TestEntities;

public class Post : DbEntity
{
    [DbField(IsPrimaryKey = true)]
    public uint Id { get; set; }
    
    public string? Title { get; set; }
    
    public uint UserId { get; set; }
}