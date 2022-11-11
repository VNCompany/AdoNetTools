using System;
using System.Data.Common;
using System.Reflection;

using Vnc.Ado.Interfaces;

namespace Vnc.Ado.Converters
{
    internal class DefaultDbConverter : IDbValueConverter
    {
        public static DefaultDbConverter Create(Type fieldType)
        {
            Type readerType = typeof(DbDataReader);
            MethodInfo method = readerType.GetMethod($"Get{fieldType.Name.Replace("Single", "Float")}");
            return method == null ? null : new DefaultDbConverter(method);
        }

        readonly MethodInfo readerMethod;
        public DefaultDbConverter(MethodInfo dbReaderMethodInfo)
        {
            readerMethod = dbReaderMethodInfo;
        }

        public object Convert(DbDataReader reader, int i) => readerMethod.Invoke(reader, new object[] { i });

        public object ConvertBack(object value) => value;
    }
}