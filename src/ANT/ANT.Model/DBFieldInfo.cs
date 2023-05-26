using System;

namespace ANT.Model
{
    public class DBFieldInfo : IFreezable, ICloneable
    {
        public bool IsFrozen { get; private set; }
        public void Freeze() => IsFrozen = true;

        public DBFieldInfo Clone()
        {
            var newObj = (DBFieldInfo)MemberwiseClone();
            newObj.IsFrozen = false;
            return newObj;
        }
        object ICloneable.Clone() => Clone();

        private string fieldName = String.Empty;
        public string FieldName
        {
            get => fieldName;
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("You can't change the properties of the frozen object");
                fieldName = value;
            }
        }
        
        private string dbType = String.Empty;
        public string DBType
        {
            get => dbType;
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("You can't change the properties of the frozen object");
                dbType = value;
            }
        }

        private bool isNotNull;
        public bool IsNotNull
        {
            get => isNotNull;
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("You can't change the properties of the frozen object");
                isNotNull = value;
            }
        }

        private bool isPrimaryKey;
        public bool IsPrimaryKey
        {
            get => isPrimaryKey;
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("You can't change the properties of the frozen object");
                isPrimaryKey = value;
            }
        }

        private bool isAuto;
        public bool IsAuto
        {
            get => isAuto;
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("You can't change the properties of the frozen object");
                isAuto = value;
            }
        }
        
        private string? customAttributes;
        public string? CustomAttributes
        {
            get => customAttributes;
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("You can't change the properties of the frozen object");
                customAttributes = value;
            }
        }
    } 
}