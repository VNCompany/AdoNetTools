using ANT;

namespace Tests.TestEntities;

public class User : DBEntity
{
    [DBPrimaryKey] public int PrimaryKeyField { get; set; }

    [DBIgnore] public string IgnoredField { get; set; } = string.Empty;

    [DBNotNull] public string NotNullField { get; set; } = null!;

    public string? NullableField { get; set; }

    [DBNotNull("custom")] public string FieldWithCustomName { get; set; } = null!;
    
    [DBField(DBType = "CUSTOMTYPE")] public string? FieldWithCustomType { get; set; }
    
    [DBField(fieldName: "custom2", dbType: "CUSTOMTYPE")]
    public DateTime? FieldWithCustomNameAndType { get; set; }
    
    public int? NullableFieldInt { get; set; }
}