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
    }

    [TestCase(null, ExpectedResult = null)]
    [TestCase("", ExpectedResult = "")]
    [TestCase("\t", ExpectedResult = "\t")]
    [TestCase("    ", ExpectedResult = "    ")]
    [TestCase("hello?", ExpectedResult = "hello?")]
    [TestCase("hello_world", ExpectedResult = "hello_world")]
    [TestCase("Hello!", ExpectedResult = "hello!")]
    [TestCase("HelloWorld", ExpectedResult = "hello_world")]
    [TestCase("worlD", ExpectedResult = "worl_d")]
    [TestCase("HELLOWORLD!", ExpectedResult = "h_e_l_l_o_w_o_r_l_d!")]
    public string? CamelToSnakeAlghoTest(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        char[] ch = new Char[input.Length * 2];
        int j = 0;
        for (int i = 0; i < input.Length; i++, j++)
        {
            if (i == 0) { ch[0] = char.ToLower(input[i]); continue; }

            if (char.IsUpper(input[i]))
                (ch[j++], ch[j]) = ('_', char.ToLower(input[i]));
            else
                ch[j] = input[i];
        }

        return new string(ch, 0, j);
    }
}