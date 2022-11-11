using System.Reflection;

using Vnc.Ado.Interfaces;

namespace Vnc.Ado.Internal
{
    public class DbFieldInfo
    {
        public string Name { get; set; }
        public DbFieldAttribute Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public IDbValueConverter Converter { get; set; }
    }
}
