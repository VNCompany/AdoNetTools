using Tests.TestEntities;
using ANT;
using ANT.Model;

namespace Tests;

[TestFixture]
public class ANTProviderTests
{
    [OneTimeSetUp]
    public void Init()
    {
        ANTProvider.RegisterClass<User>();
        ANTProvider.RegisterClass<Post>();
    }

    [Test]
    public void ANTProvider_Service_CheckRegisteredClassesCount()
    {
        var user = ANTProvider.GetEntityMetadata<User>();
        Assert.That(ANTProvider.RegisteredClassesCount, Is.EqualTo(2));
    }

    [Test]
    public void ANTProvider_Service_CheckDBEntityAttributeProcessing()
    {
        string userTableName = new User().Metadata.TableName;

        var postMetadata = ANTProvider.GetEntityMetadata<Post>();
        Assert.That(postMetadata, Is.Not.Null);

        string postTableName = postMetadata!.TableName;

        Assert.Multiple(() =>
        {
            Assert.That(userTableName, Is.EqualTo("User"));
            Assert.That(postTableName, Is.EqualTo("posts"));
        });
    }

    [Test]
    public void ANTProvider_Service_PrimaryKeys()
    {
        int? userPrimaryKeys = ANTProvider.GetEntityMetadata<User>()?.PrimaryKeys.Count;
        int? postPrimaryKeys = ANTProvider.GetEntityMetadata<Post>()?.PrimaryKeys.Count;

        Assert.Multiple(() =>
        {
            Assert.That(userPrimaryKeys, Is.EqualTo(1));
            Assert.That(postPrimaryKeys, Is.EqualTo(2));
        });
    }

    private record __entityMetadataTestCase(string PropertyName, string FieldName, string DBType, Type Type,
        bool IsPrimaryKey, bool IsNotNull);

    [Test]
    public void ANTProvider_Service_CheckMetadataIntegrity()
    {
        var cases = new ValueTuple<DBEntityMetadata?, __entityMetadataTestCase[]>[2];
        cases[0] = new(
            ANTProvider.GetEntityMetadata<User>(),
            new __entityMetadataTestCase[]
            {
                new("Id", "Id", "INT", typeof(int), true, true),
                new("Login", "Login", "VARCHAR(64)", typeof(string), false, true),
                new("Password", "Password", "VARCHAR(64)", typeof(string), false, true),
                new("Description", "Description", "TEXT", typeof(string), false, false),
                new("RegisteredDateTime", "reg_date", "DATETIME", typeof(DateTime), false, true)
            });
        cases[1] = new(
            ANTProvider.GetEntityMetadata<Post>(),
            new __entityMetadataTestCase[]
            {
                new("Id", "Id", "INT", typeof(int), true, true),
                new("Id2", "Id2", "BIGINT UNSIGNED", typeof(ulong), true, true),
                new("Age", "Age", "INT", typeof(object), false, false),
                new("Content", "Content", "TEXT", typeof(string), false, false)
            });

        foreach (var testCase in cases)
        {
            var metadata = testCase.Item1;
            Assert.That(metadata, Is.Not.Null);
        
            foreach (var userTestCase in testCase.Item2)
            {
                Assert.That(metadata!.FieldMetadatas.TryGetValue(userTestCase.FieldName, out var fieldMetadata), Is.True, userTestCase.FieldName);
            
                Assert.Multiple(() =>
                {
                    Assert.That(fieldMetadata!.PropertyInfo.Name, Is.EqualTo(userTestCase.PropertyName));
                    Assert.That(fieldMetadata!.FieldName, Is.EqualTo(userTestCase.FieldName));
                    Assert.That(fieldMetadata!.DBType, Is.EqualTo(userTestCase.DBType));
                    if (userTestCase.Type != typeof(object))
                        Assert.That(fieldMetadata!.PropertyInfo.PropertyType, Is.EqualTo(userTestCase.Type));
                    Assert.That(fieldMetadata!.IsPrimaryKey, Is.EqualTo(userTestCase.IsPrimaryKey));
                    Assert.That(fieldMetadata!.IsNotNull, Is.EqualTo(userTestCase.IsNotNull));
                });
            }
        }
    }

    [Test]
    public void ANTProvider_ORM_Mapper()
    {
        DateTime now = DateTime.Now;

        TestDbDataReader dataReader = new TestDbDataReader(new Dictionary<string, object>()
        {
            ["Id"] = 132,
            ["Login"] = "admin",
            ["Password"] = "qwerty",
            ["Description"] = DBNull.Value,
            ["reg_date"] = now,
            ["IgnoredField"] = "some"
        });

        var obj1 = ANTProvider.DBMapToEntity<User>(dataReader);
        Assert.That(obj1, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(obj1!.Id, Is.EqualTo(132));
            Assert.That(obj1!.Login, Is.EqualTo("admin"));
            Assert.That(obj1!.Password, Is.EqualTo("qwerty"));
            Assert.That(obj1!.Description, Is.Null);
            Assert.That(obj1!.RegisteredDateTime, Is.EqualTo(now));
            Assert.That(obj1!.IgnoredField, Is.EqualTo(string.Empty));
        });
    }
}