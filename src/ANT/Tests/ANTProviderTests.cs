using Tests.TestEntities;
using ANT;
using ANT.Model;
using ANT.Configurators;

namespace Tests;

[TestFixture]
public class ANTProviderTests
{
    [OneTimeSetUp]
    public void Init()
    {
        ANTProvider.Use(new MySqlConfigurator());
        ANTProvider.RegisterClass<User>();
    }

    [Test]
    public void ORM_Entity_Naming()
    {
        var e = new User();
        Assert.That(e.Metadata.TableName, Is.EqualTo("users"));
    }

    [Test]
    public void ORM_User_FieldMetadatas()
    {
        DBFieldMetadata m;
        var e = new User();
        
        // Check primary key
        Assert.That(e.Metadata.PrimaryKeys.Count(), Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(e.Metadata.PrimaryKeys.First().PropertyInfo.Name, Is.EqualTo("PrimaryKeyField"));
            Assert.That(e.Metadata.PrimaryKeys.First().Info.FieldName, Is.EqualTo("primary_key_field"));
            Assert.That(e.Metadata.PrimaryKeys.First().Info.IsAuto, Is.False);
            Assert.That(e.Metadata.PrimaryKeys.First().Info.DBType, Is.EqualTo("INT"));
        });

        // Check dbignore field
        Assert.That(e.Metadata.FieldMetadatas.Count, Is.EqualTo(7));
        Assert.Multiple(() =>
        {
            Assert.That(e.Metadata.FieldMetadatas.ContainsKey("IgnoredField"), Is.False);
            Assert.That(e.Metadata.FieldMetadatas.ContainsKey("ignored_field"), Is.False);
        });

        // Check not null field
        Assert.That(e.Metadata.FieldMetadatas.ContainsKey("not_null_field"), Is.True);
        m = e.Metadata.FieldMetadatas["not_null_field"];
        Assert.Multiple(() =>
        {
            Assert.That(m.PropertyInfo.Name, Is.EqualTo("NotNullField"));
            Assert.That(m.Info.FieldName, Is.EqualTo("not_null_field"));
            Assert.That(m.Info.IsPrimaryKey, Is.False);
            Assert.That(m.Info.IsNotNull, Is.True);
            Assert.That(m.Info.DBType, Is.EqualTo("TEXT"));
        });
        
        // Check nullable field
        m = e.Metadata.FieldMetadatas["nullable_field"];
        Assert.Multiple(() =>
        {
            Assert.That(m.Info.IsNotNull, Is.False);
            Assert.That(m.Info.DBType, Is.EqualTo("TEXT"));
        });
        
        // Check field with custom name (nullable)
        m = e.Metadata.FieldMetadatas["custom"];
        Assert.Multiple(() =>
        {
            Assert.That(m.Info.IsNotNull, Is.True);
            Assert.That(m.Info.DBType, Is.EqualTo("TEXT"));
            Assert.That(m.Info.FieldName, Is.EqualTo("custom"));
        });
        
        // Check field with custom type (not nullable)
        m = e.Metadata.FieldMetadatas["field_with_custom_type"];
        Assert.Multiple(() =>
        {
            Assert.That(m.Info.IsNotNull, Is.False);
            Assert.That(m.Info.DBType, Is.EqualTo("CUSTOMTYPE"));
            Assert.That(m.Info.FieldName, Is.EqualTo("field_with_custom_type"));
        });
        
        // Check field with custom name and type (nullable)
        m = e.Metadata.FieldMetadatas["custom2"];
        Assert.Multiple(() =>
        {
            Assert.That(m.Info.IsNotNull, Is.False);
            Assert.That(m.Info.DBType, Is.EqualTo("CUSTOMTYPE"));
            Assert.That(m.Info.FieldName, Is.EqualTo("custom2"));
        });
        
        // Check field with nullable int
        m = e.Metadata.FieldMetadatas["nullable_field_int"];
        Assert.Multiple(() =>
        {
            Assert.That(m.Info.IsNotNull, Is.False);
            Assert.That(m.Info.DBType, Is.EqualTo("INT"));
        });
    }

    [Test]
    public void ORM_User_CheckMapping()
    {
        var u = new User()
        {
            PrimaryKeyField = 10,
            NotNullField = "a",
            NullableField = null,
            FieldWithCustomName = "b",
            FieldWithCustomType = "c",
            FieldWithCustomNameAndType = null,
            NullableFieldInt = 12
        };

        var dataReader = new TestDbDataReader(u.DBEntityExport());
        var nu = new User();
        nu.DBEntityImport(dataReader);
        Assert.Multiple(() =>
        {
            Assert.That(nu.PrimaryKeyField, Is.EqualTo(u.PrimaryKeyField));
            Assert.That(nu.NotNullField, Is.EqualTo(u.NotNullField));
            Assert.That(nu.NullableField, Is.EqualTo(u.NullableField));
            Assert.That(nu.FieldWithCustomName, Is.EqualTo(u.FieldWithCustomName));
            Assert.That(nu.FieldWithCustomType, Is.EqualTo(u.FieldWithCustomType));
            Assert.That(nu.FieldWithCustomNameAndType, Is.EqualTo(u.FieldWithCustomNameAndType));
            Assert.That(nu.NullableFieldInt, Is.EqualTo(u.NullableFieldInt));
        });
    }
}