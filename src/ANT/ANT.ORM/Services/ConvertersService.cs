using System;
using System.Collections.Generic;

using ANT.ORM.ValueConverters;

namespace ANT.ORM.Services
{
    internal static class ConvertersService
    {
        private static readonly Dictionary<string, IValueConverter> _converters;

        static ConvertersService()
        {
            _converters = new()
            {
                { typeof(DefaultValueConverter).FullName!, new DefaultValueConverter() }
            };
        }

        public static IValueConverter GetConverter(Type converterType)
        {
            if (_converters.TryGetValue(converterType.FullName!, out var existingConverter))
                return existingConverter;

            IValueConverter newConverter = Activator.CreateInstance(converterType) as IValueConverter ??
                                           throw new InvalidOperationException("Invalid converter");
            
            _converters.Add(converterType.FullName!, newConverter);
            return newConverter;
        }
    }
}