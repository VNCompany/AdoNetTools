using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ANT.Model;
using ANT.Model.Data.MappingModels;

namespace ANT.Constructors
{
    public class InsertConstructor : IQueryConstructor
    {
        private readonly InsertConstructorData _data;

        public string ParametersPrefix { get; set; } = "@__";
        
        public string TableName { get; }

        public InsertConstructor(string tableName, InsertConstructorData data)
        {
            TableName = tableName;
            _data = data;
        }
        
        public IEnumerable<KeyValuePair<string, object?>> GetCommandParameters()
        {
            for (int i = 0; i < _data.RowsCount; i++)
            {
                var row = _data[i];
                for (int j = 0; j < row.Length; j++)
                    yield return new KeyValuePair<string, object?>(ParametersPrefix + _data.GetParameterName(i, j), row[j]);
            }
        }

        public string? Build()
        {
            StringBuilder qstr = new StringBuilder($"INSERT INTO `{TableName}`(`");
            qstr.AppendJoin("`,`", _data.Fields);
            qstr.Append("`) VALUES ");

            string[] paramsBuf = new string[_data.Fields.Length];
            for (int i = 0; i < _data.RowsCount; i++)
            {
                for (int j = 0; j < paramsBuf.Length; j++)
                    paramsBuf[j] = ParametersPrefix + _data.GetParameterName(i, j);
                qstr.Append($"({string.Join(",", paramsBuf)})");

                if (i < _data.RowsCount - 1)
                    qstr.Append(',');
            }

            return qstr.ToString();
        }

        public static InsertConstructor CreateFromEntity<T>(T item) where T : IDBEntity
        {
            var dict = item.DBEntityExport().Where(f => !f.Value.Info.IsAuto).Select(f => f.Value);
            var fieldsDataCollection = dict.ToArray();
            return new InsertConstructor(
                item.Metadata.TableName,
                new InsertConstructorData(fieldsDataCollection.Select(f => f.Name), fieldsDataCollection.Select(f => f.Value)));
        }
        
        public static InsertConstructor CreateFromEntity<T>(IEnumerable<T> items) where T : IDBEntity
        {
            IList<T> itemsList = items as IList<T> ?? items.ToArray();
            if (itemsList.Count == 0) throw new InvalidOperationException("items is empty");

            var icd = new InsertConstructorData(
                itemsList[0].Metadata.FieldMetadatas
                    .Where(it => !it.Value.Info.IsAuto)
                    .Select(itt => itt.Key));
            foreach (var item in itemsList)
            {
                var itemDict = item.DBEntityExport();
                object?[] itemBuffer = new object?[icd.Fields.Length];
                for (int i = 0; i < itemBuffer.Length; i++)
                    itemBuffer[i] = itemDict[icd.Fields[i]];
                icd.AddValues(itemBuffer);
            }

            return new InsertConstructor(itemsList[0].Metadata.TableName, icd);
        }
    }
}