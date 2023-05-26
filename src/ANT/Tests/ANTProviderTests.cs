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
}