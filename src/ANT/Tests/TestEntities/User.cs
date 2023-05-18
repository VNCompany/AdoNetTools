using ANT;

namespace Tests.TestEntities;

public class User : DBEntity
{
    [DBField(IsPrimaryKey = true)]
    public int Id { get; set; }
    
    [DBField(DBType = "VARCHAR(64)", IsNotNull = true)]
    public string Login { get; set; } = null!;
    
    [DBField(DBType = "VARCHAR(64)", IsNotNull = true)]
    public string Password { get; set; } = null!;
    
    public int? Description { get; set; }
    
    [DBField("reg_date", IsNotNull = true)]
    public DateTime RegisteredDateTime { get; set; }

    [DBIgnore]
    public string? IgnoredField => string.Empty;
}