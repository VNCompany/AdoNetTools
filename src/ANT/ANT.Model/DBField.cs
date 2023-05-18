namespace ANT.Model
{
    public class DBField
    {
        public string Name { get; }
        public object? Value { get; }

        public DBField(string name, object? value)
        {
            Name = name;
            Value = value;
        }
    }
}