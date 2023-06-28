using System.Data.Common;

using ANT.Constructors.Internal;

namespace ANT.Constructors
{
    public static class DbCommandExtensions
    {
        public static T ConstructQuery<T>(this T dbCommand, IQueryConstructor constructor, bool prepare = true) 
            where T : DbCommand
        {
            ConstructorParameters cp = new ConstructorParameters("@__ant_");
            dbCommand.CommandText = constructor.Construct(cp);
            foreach (var (pName, pValue) in cp)
            {
                var dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = pName;
                dbParameter.Value = pValue;
            }
            if (prepare)
                dbCommand.Prepare();
            return dbCommand;
        } 
    }
}