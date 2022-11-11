using System;
using System.Data.Common;

namespace Vnc.Ado.Interfaces
{
    public interface IDbValueConverter
    {
        object Convert(DbDataReader reader, int i);
        object ConvertBack(object value);
    }
}