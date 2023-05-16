using System;
using System.Collections.Generic;

using ANT.Model;

namespace ANT
{
    public static class ANTProvider
    {
        private static readonly List<IDBEntity> _registeredClasses;

        static ANTProvider()
        {
            _registeredClasses = new List<IDBEntity>();
        }

        public static int RegisterClass<T>() where T: IDBEntity
        {
            
            return _registeredClasses.Count - 1;
        }
    }
}