using ANT;

namespace Tests.TestEntities;

[DBEntity("posts")]
public class Post : DBEntity
{
    [DBField(IsPrimaryKey = true)]
    public int Id { get; set; }
    
    [DBField(IsPrimaryKey = true)]
    public ulong Id2 { get; set; }
    
    public int? Age { get; set; }
    
    public string? Content { get; set; }
}