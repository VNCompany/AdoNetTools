using System;
using System.Collections.Generic;

using ANT.Model;

namespace ANT
{
    public abstract class DBEntity : IDBEntity
    {
        private DBEntityMetadata? metadata;

        public DBEntityMetadata Metadata
            => metadata ??= ANTProvider.GetEntityMetadata(this.GetType())
                            ?? throw new InvalidOperationException(
                                "Entity class is not registered in ANTProvider");


        public void SetFieldValue(string fieldName, object? value)
        {
            throw new System.NotImplementedException();
        }

        public DBField? GetFieldValue(string fieldName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, DBField>> GetFields()
        {
            throw new System.NotImplementedException();
        }
    }
}