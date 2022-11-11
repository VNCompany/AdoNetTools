#if SQLITE
using __DbCommand = System.Data.SQLite.SQLiteCommand;
#elif POSTGRES
using __DbCommand = Npgsql.NpgsqlCommand;
#elif MYSQL
using __DbCommand = MySql.Data.MySqlClient.MySqlCommand;
#endif

using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

using Vnc.Ado.Extensions;

namespace Vnc.Ado.Builders
{
    public class InsertCommandBuilder<T> where T : class
    {
        readonly __DbCommand command;
        readonly DbEntityMetadata<T> metadata;

        List<string> fields;
        List<T> values;

        // Extract from metadata all fields
        private void SetFields()
        {
            fields = new List<string>(metadata.FieldNames.Count);
            fields.AddRange(metadata.FieldNames);
        }

        public InsertCommandBuilder(__DbCommand command, T item, DbEntityMetadata<T> meta)
        {
            this.command = command;
            metadata = meta;
            SetFields();

            values = new List<T>(1) { item };
        }
        public InsertCommandBuilder(__DbCommand command, IEnumerable<T> items, DbEntityMetadata<T> meta)
        {
            this.command = command;
            metadata = meta;
            SetFields();

            values = new List<T>(items);
        }

        public InsertCommandBuilder<T> Exclude(IEnumerable<string> excludedFields)
        {
            foreach (string field in excludedFields)
                fields.Remove(field);

            return this;
        }
        public InsertCommandBuilder<T> Exclude(string field)
        {
            fields.Remove(field);

            return this;
        }

        /// <summary>
        /// Build the command
        /// </summary>
        /// <returns>Built command</returns>
        public __DbCommand Build()
        {
            command.CommandText = $"INSERT INTO `{metadata.TableName}`(@fields) VALUES @values;";
            command.SetParameters(fields, values.Select(entity => metadata.GetValues(entity, fields).ToArray()));
            return command;
        }
        /// <summary>
        /// Build the command
        /// </summary>
        /// <param name="autoIncrementField">This field will be excluded</param>
        /// <returns>Built command</returns>
        public __DbCommand Build(string autoIncrementField)
        {
            Exclude(autoIncrementField);
            return Build();
        }
    }
}
