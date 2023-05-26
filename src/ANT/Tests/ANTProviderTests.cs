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
    }

    [Test]
    public void ORM_Entity_Naming()
    {
        var e = new User();
        Assert.That(e.Metadata.TableName, Is.EqualTo("users"));
    }
}