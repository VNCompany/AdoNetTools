using System;
using System.Linq;
using System.Collections.Generic;

namespace ANT.Constructors
{
    public class ValuesCollection : List<object?>
    {
        public ValuesCollection() { }
        public ValuesCollection(int opacity) : base(opacity) { }
        public ValuesCollection(IEnumerable<object?> values) : base(values) { }

        private string _BuildItemString(object? item, string? itemFrame)
        {
            string? result = item?.ToString();
            if (!string.IsNullOrEmpty(itemFrame) && result != null)
                result = string.Concat(itemFrame[0], result, itemFrame[1]);
            return result ?? "NULL";
        }

        public string ToString(string? frame, string? itemFrame, string separator)
        {
            if (frame != null && frame.Length == 2) 
                throw new ArgumentException("Invalid frame", nameof(frame));
            if (itemFrame != null && itemFrame.Length == 2) 
                throw new ArgumentException("Invalid item frame", nameof(itemFrame));

            string combinedString = string.Join(separator, this.Select(i => _BuildItemString(i, itemFrame)));
            if (frame != null)
                combinedString = string.Concat(frame[0], combinedString, frame[1]);
            return combinedString;
        }

        public override string ToString() => ToString("()", "''", ",");
    }
}