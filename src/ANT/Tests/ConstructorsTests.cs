using Tests.TestEntities;
using ANT.Constructors;
using ANT;

using Sql = System.Data.Common;

namespace Tests;

[TestFixture]
public class ConstructorsTests
{
    [OneTimeSetUp]
    public void Init()
    {
        ANTProvider.Configure(new ANT.Configurators.MySqlConfigurator());
        ANTProvider.RegisterClass<User>();
    }

    [Test]
    public void Constructors_InsertConstructor_FromCollections()
    {
        string[] fields = { "field1", "field2" };

        Assert.Throws<InvalidOperationException>(() =>
        {
            new InsertConstructor("table1",
                new InsertConstructorData(fields, new[] { "value1", "value2", "value3" }));
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            new InsertConstructor("table1",
                new InsertConstructorData(fields, new[] { "value1" }));
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            new InsertConstructor("table1",
                new InsertConstructorData(Enumerable.Empty<string>(), Enumerable.Empty<object?>()));
        });
        
        {
            var values = new object?[][]
            {
                new object?[] { "value1", "value2" },
                new object?[] { "value3", "value4" }
            };
            
            var ctor = new InsertConstructor("table1", new InsertConstructorData(fields, values));
            Dictionary<string, object?> p = new Dictionary<string, object?>(ctor.GetCommandParameters()); 
            Assert.Multiple(() =>
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < fields.Length; j++)
                    {
                        string paramName = $"@__f{i}_{fields[j]}";
                        bool keyExists = p.TryGetValue(paramName, out var paramValue);
                        Assert.That(keyExists, Is.True, $"{paramName} doesn't exists");
                        if (keyExists)
                            Assert.That(paramValue, Is.EqualTo(values[i][j]), $"{paramName} hasn't valid value");
                    }
                }

                Assert.That(ctor.Build(), Is.EqualTo("INSERT INTO `table1`(`field1`,`field2`) " +
                                                     "VALUES (@__f0_field1,@__f0_field2)," +
                                                     "(@__f1_field1,@__f1_field2)"));
            });
        }
    }

    [Test]
    public void Constructors_InsertConstructor_FromEntity()
    {
        var item = new User() { Id = 1, Login = "admin", Password = "qwerty" };

        var ctor = InsertConstructor.CreateFromEntity(item);
        var p = new Dictionary<string, object?>(ctor.GetCommandParameters());
        Assert.Multiple(() =>
        {
            Assert.That(p["@__f0_login"], Is.EqualTo(item.Login));
            Assert.That(p["@__f0_password"], Is.EqualTo(item.Password));

            Assert.That(ctor.Build(), Is.EqualTo("INSERT INTO `users`(`login`,`password`) " +
                                                 "VALUES (@__f0_login,@__f0_password)"));
        });
    }

    [Test]
    public void Constructors_InsertConstructor_FromEntities()
    {
        var items = new[]
        {
            new User() { Id = 1, Login = "admin", Password = "qwerty" },
            new User() { Id = 2, Login = "victor", Password = "123456" }
        };
        
        var constructor = UpdateConstructor
            .CreateFromEntity(items[0])
            .Where("`login`=@login", ("@login", "admin"));
        
        constructor.Build(); // UPDATE `users` SET `id`=@__id, `login`=@__login, `password`=@__password WHERE `login`=@login;
        constructor.GetCommandParameters(); /*
        {
            { "@__id", 1 }
            { "@__login", "admin" }
            { "@__password", "qwerty" }
            { "@login", "admin" }
        }
        */
        

        var ctor = InsertConstructor.CreateFromEntity(items);
        var p = new Dictionary<string, object?>(ctor.GetCommandParameters());
        Assert.Multiple(() =>
        {
            for (int i = 0; i < items.Length; i++)
            {
                Assert.That(p[$"@__f{i}_login"], Is.EqualTo(items[i].Login));
                Assert.That(p[$"@__f{i}_password"], Is.EqualTo(items[i].Password));
            }

            Assert.That(ctor.Build(), Is.EqualTo("INSERT INTO `users`(`login`,`password`) VALUES " +
                                                 "(@__f0_login,@__f0_password)," +
                                                 "(@__f1_login,@__f1_password)"));
        });
    }
}