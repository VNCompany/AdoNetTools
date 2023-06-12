using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using ANT.Model;

namespace ANT.Constructors
{
    public enum UpdateFilterType { Include, Exclude }
    
    public class UpdateConstructor : IQueryConstructor
    {
        private readonly IDBEntity? _entity;
        
        private string _tableName = null!;
        private KeyValuePair<string, object?>[] _fields = null!;
        private Dictionary<string, object?>? _params;
        private WhereCondition? _whereCondition;

        public string ParametersPrefix { get; set; } = "@__";

        private void Init(string tableName, IEnumerable<KeyValuePair<string, object?>> fields)
        {
            _fields = fields.ToArray();
            if (_fields.Length == 0)
                throw new InvalidOperationException("fields is empty");
            _tableName = tableName;
        }

        public UpdateConstructor(string tableName, IEnumerable<KeyValuePair<string, object?>> fields)
        {
            Init(tableName, fields);
        }

        private UpdateConstructor(IDBEntity entity, IEnumerable<string>? filter,
            UpdateFilterType filterType)
        {
            if (filterType == UpdateFilterType.Include || filter == null)
                Init(entity.Metadata.TableName, entity.DBEntityExport(filter)
                    .Select(f => new KeyValuePair<string, object?>(f.Key, f.Value.Value)));
            else
                Init(entity.Metadata.TableName, entity.DBEntityExport(
                        entity.Metadata.FieldMetadatas
                            .Select(fm => fm.Value.PropertyInfo.Name)
                            .Where(propName => filter.Contains(propName) == false))
                    .Select(f => new KeyValuePair<string, object?>(f.Key, f.Value.Value)));
            
            _entity = entity;
        }
        
        public UpdateConstructor Where(WhereCondition cond)
        {
            _whereCondition = cond;
            return this;
        }
        
        public UpdateConstructor Where(WhereCondition cond, IEnumerable<KeyValuePair<string, object?>> parameters)
        {
            cond.Parameters = parameters;
            return Where(cond);
        }

        public UpdateConstructor Where(WhereCondition cond, params (string, object?)[] parameters)
        {
            return Where(cond, parameters.Select(p => new KeyValuePair<string, object?>(p.Item1, p.Item2)));
        }

        public string? Build()
        {
            StringBuilder sb = new StringBuilder($"UPDATE `{_tableName}` SET ");
            _params = new Dictionary<string, object?>();
            
            for (int i = 0; i < _fields.Length; i++)
            {
                string paramName = ParametersPrefix + _fields[i].Key;
                sb.AppendFormat("`{0}`={1}", _fields[i].Key, paramName);
                
                _params.Add(paramName, _fields[i].Value);
                
                if (i != _fields.Length - 1)
                    sb.Append(',');
            }

            if (_whereCondition == null && _entity != null)
            {
                var conditionParts = new List<(string, string, object?)>(
                    _entity.Metadata.PrimaryKeys
                        .Select(pk => (pk.Info.FieldName,
                            $"{ParametersPrefix}pk_{pk.Info.FieldName}",
                            pk.PropertyInfo.GetValue(_entity))));

                _whereCondition = new WhereCondition(
                    condition: string.Join(" AND ", conditionParts.Select(p => $"`{p.Item1}`={p.Item2}")),
                    parameters: conditionParts.Select(p => new KeyValuePair<string,object?>(p.Item2, p.Item3)));
            }

            if (_whereCondition != null)
            {
                sb.Append(" WHERE " + _whereCondition.ToString());
                if (_whereCondition.Parameters != null)
                {
                    foreach (var condParam in _whereCondition.Parameters)
                        _params.Add(condParam.Key, condParam.Value);
                }
            }

            return sb.ToString();
        }

        public IEnumerable<KeyValuePair<string, object?>> GetCommandParameters()
            => _params ?? throw new NullReferenceException("Parameters not added");

        public static UpdateConstructor CreateFromEntity(IDBEntity entity)
            => new UpdateConstructor(entity, null, UpdateFilterType.Include);

        public static UpdateConstructor CreateFromEntity(IDBEntity entity,
            IEnumerable<string> filter, UpdateFilterType filterType = UpdateFilterType.Exclude)
            => new UpdateConstructor(entity, filter, filterType);

        public static UpdateConstructor CreateFromEntity<T>(
            T entity,
            Expression<Func<T, object?[]>> filter,
            UpdateFilterType filterType = UpdateFilterType.Exclude) where T : IDBEntity
        {
            if (filter.Body is NewArrayExpression nae)
            {
                List<string> propNames = new List<string>(nae.Expressions.Count);
                foreach (string? propName in nae.Expressions
                             .Select(e => ((e as MemberExpression)?.Member as PropertyInfo)?.Name))
                    propNames.Add(propName ?? throw new InvalidOperationException("Invalid filter expression element"));

                return CreateFromEntity(entity, propNames, filterType);
            }
            throw new InvalidOperationException("Invalid filter expression");
        }
    }
}