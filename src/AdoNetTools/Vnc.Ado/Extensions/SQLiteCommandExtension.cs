#if SQLITE
using __DbCommand = System.Data.SQLite.SQLiteCommand;
#elif POSTGRES
using __DbCommand = Npgsql.NpgsqlCommand;
#elif MYSQL
using __DbCommand = MySql.Data.MySqlClient.MySqlCommand;
#endif

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Vnc.Ado.Builders;

namespace Vnc.Ado.Extensions
{
    public static class SQLiteCommandExtension
    {
        /// <summary>
        /// Sets command parameters from dictionary
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters">Dictionary with type of keys - string and values - object</param>
        public static void SetParameters(this __DbCommand command, SqlCommandParameters parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            if (command.CommandText.Contains("@fields"))
                command.CommandText = command.CommandText.Replace("@fields",
                    $"`{string.Join("`,`", parameters.Keys)}`");

            if (command.CommandText.Contains("@values"))
                command.CommandText = command.CommandText.Replace("@values", 
                    string.Join(",", parameters.Keys.Select(k => '@' + k)));

            foreach (var parameter in parameters)
                command.Parameters.AddWithValue('@' + parameter.Key, parameter.Value);


            command.Parameters.AddRange(parameters.ToArray());
        }

        /// <summary>
        /// Sets command parameters from list of values array
        /// </summary>
        /// <param name="command"></param>
        /// <param name="fields">Table field names</param>
        /// <param name="values">List of rows with values array</param>
        /// <exception cref="ArgumentException">Throws, if values[i] length doesn't equals fields length</exception>
        public static void SetParameters(this __DbCommand command, IEnumerable<string> fields, IEnumerable<object[]> values)
        {
            string[] fieldsArray = fields.ToArray();
            if (fieldsArray.Length == 0 || values.Count() == 0)
                return;

            if (command.CommandText.Contains("@fields"))
                command.CommandText = command.CommandText.Replace("@fields", 
                    $"`{string.Join("`,`", fieldsArray)}`");

            StringBuilder stringBuilder = new StringBuilder();
            object[][] valuesArray = values.ToArray();

            for (int i = 0; i < valuesArray.Length; i++)
            {
                if (valuesArray[i].Length != fieldsArray.Length)
                    throw new ArgumentException("The length of the string does not match the number of fields", "values");

                string[] rowParameters = fieldsArray.Select(f => $"@{f}_{i}").ToArray();

                string rowParametersString = $"({string.Join(",", rowParameters)})";
                stringBuilder.Append(stringBuilder.Length > 0 ? ',' + rowParametersString : rowParametersString);

                for (int j = 0; j < fieldsArray.Length; j++)
                    command.Parameters.AddWithValue(rowParameters[j], valuesArray[i][j]);
            }
            command.CommandText = command.CommandText.Replace("@values", stringBuilder.ToString());
        }


        /// <summary>
        /// Builds an update command
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="command"></param>
        /// <param name="entity"></param>
        /// <param name="meta">Entity metadata</param>
        /// <param name="fields">Names of included fields</param>
        /// <returns>Builded command, requiring preparing and executing</returns>
        public static __DbCommand BuildUpdateCommand<T>(this __DbCommand command, T entity, DbEntityMetadata<T> meta, IEnumerable<string> fields) where T : class
        {
            const string sqlCommandTemplate = "UPDATE `{0}` SET {1} WHERE {2};";

            List<string> fieldList = new List<string>(fields);
            if (fieldList.Contains(meta.PrimaryKey))
                fieldList.Remove(meta.PrimaryKey);

            string sets = string.Join(",", fieldList.Select(f => $"`{f}`=@{f}")); // SET
            string where = $"`{meta.PrimaryKey}` = @{meta.PrimaryKey}"; // WHERE
            command.CommandText = string.Format(sqlCommandTemplate, meta.TableName, sets, where);

            foreach (string fieldName in fieldList)
                command.Parameters.AddWithValue(fieldName, meta.GetFieldValue(entity, fieldName));
            // Primary key parameter
            command.Parameters.AddWithValue('@' + meta.PrimaryKey, meta.GetFieldValue(entity, meta.PrimaryKey));

            return command;
        }
        /// <summary>
        /// Builds an update command
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="command"></param>
        /// <param name="entity"></param>
        /// <param name="meta">Entity metadata</param>
        /// <returns>Builded command, requiring preparing and executing</returns>
        public static __DbCommand BuildUpdateCommand<T>(this __DbCommand command, T entity, DbEntityMetadata<T> meta) where T : class
            => BuildUpdateCommand<T>(command, entity, meta, meta.FieldNames);

        /// <summary>
        /// Returns insert command builder
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="command"></param>
        /// <param name="entity"></param>
        /// <param name="meta">Entity metadata</param>
        /// <returns>Insert command builder</returns>
        public static InsertCommandBuilder<T> InsertCommand<T>(this __DbCommand command, T entity, DbEntityMetadata<T> meta) where T : class
            => new InsertCommandBuilder<T>(command, entity, meta);

        /// <summary>
        /// Returns insert command builder
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="command"></param>
        /// <param name="entities">Collection on entities</param>
        /// <param name="meta">Entity metadata</param>
        /// <returns>Insert command builder</returns>
        public static InsertCommandBuilder<T> InsertCommand<T>(this __DbCommand command, IEnumerable<T> entities, DbEntityMetadata<T> meta) where T : class
            => new InsertCommandBuilder<T>(command, entities, meta);
    }
}
