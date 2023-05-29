using ANT.Model;
using ANT.Constructors;
using ANT.Constructors.QueryParts;

namespace Tests;

[TestFixture]
public class ANTConstructorTests
{
    [Test]
    public void Constructor_BaseLogic_Test()
    {
        IQueryConstructor queryConstructor = new QueryConstructor();
        
        Assert.Multiple(() =>
        {
            Assert.That(queryConstructor.CommandParameters?.Count, Is.EqualTo(0));
            Assert.That(queryConstructor.Build(), Is.Null);
        });

        
        queryConstructor = new QueryConstructor(true, true);
        
        Assert.Multiple(() =>
        {
            Assert.That(queryConstructor.CommandParameters, Is.Null);
            Assert.That(queryConstructor.Build(), Is.Null);
        });


        QueryConstructor constructor = new QueryConstructor();
        
        Assert.Multiple(() =>
        {
            Assert.That(constructor.CommandParameters, Is.Not.Null);
            Assert.That(constructor.CommandParts, Is.Not.Null);
        });

        constructor.CommandParameters!.Add("age", 12);
        constructor.CommandParts!.AddRange(new string[]
        {
            "INSERT INTO", "`table`", "(`name`, `age`)", "VALUES", "('vasya', @age)"
        });
        
        Assert.That(constructor.Build(), Is.EqualTo("INSERT INTO `table` (`name`, `age`) VALUES ('vasya', @age);"));
    }

    [Test]
    public void QueryParts_ValuesCollection_Test()
    {
        ValuesCollection valuesCollection = new ValuesCollection(new object?[]
        {
            "'artur'", "`vasya`", 12, null
        });
        
        Assert.That(valuesCollection.ToString(), Is.EqualTo("('artur',`vasya`,12,NULL)"));
        
        
        valuesCollection.Clear();
        
        Assert.That(valuesCollection.ToString(), Is.EqualTo("()"));
    }

    [Test]
    public void QueryParts_KeyValuePairsCollection_Test()
    {
        KeyValuePairsCollection keyValuePairsCollection = new KeyValuePairsCollection(new[]
        {
            new KeyValuePair<string, object?>("`name`", "'vasya'"),
            new KeyValuePair<string, object?>("`age`", 13),
            new KeyValuePair<string, object?>("`desc`", null)
        });
        
        Assert.That(keyValuePairsCollection.ToString(), Is.EqualTo("`name`='vasya',`age`=13,`desc`=NULL"));
        
        
        keyValuePairsCollection.Clear();
        
        Assert.That(keyValuePairsCollection.ToString(), Is.Empty);
    }
}