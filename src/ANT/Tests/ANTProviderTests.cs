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

    [Test]
    public void ANTProvider_Service_CheckMetadataIntegrity()
    {
        // PropertyName, FieldName, DBType, Type, IsPrimaryKey, IsNotNull
        var userTestCases = new (string, string, string, Type, bool, bool)[]
        {
            ("Id", "Id", "INT", typeof(int), true, true),
            ("Login", "Login", "VARCHAR(64)", typeof(string), false, true),
            ("Password", "Password", "VARCHAR(64)", typeof(string), false, true),
            ("Description", "Description", "TEXT", typeof(int), false, false),
            ("RegisteredDateTime", "reg_date", "DATETIME", typeof(int), false, true),
        };
    }
}