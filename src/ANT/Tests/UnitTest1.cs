namespace Tests;

[TestFixture]
public class Tests
{
    public string? CamelToSnake(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return input;
    }
    
    [Test]
    public void Convert_CamelCase_To_SnakeCase()
    {
        
    }
}