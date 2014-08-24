using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    internal class TikPropertyItem
    {
        private object itemValue;
        private object itemOriginalValue;

        public bool IsModified { get; private set; }

        public bool HasValue 
        { 
            get {  return itemValue != null; }
        }

        public TikPropertyItem()
        {
            IsModified = false;
        }

        public TikPropertyItem(bool? value)
            : this()
        {
            itemValue = value;
        }

        public TikPropertyItem(long? value)
            : this()
        {
            itemValue = value;
        }

        public TikPropertyItem(string value)
            : this()
        {
            itemValue = value;
        }

        public void SetValue(bool? value)
        {
            if (!object.Equals(value, itemValue))
            {
                itemValue = value;
                IsModified = true;
            }
        }

        public void SetValue(long? value)
        {
            if (!object.Equals(value, itemValue))
            {
                itemValue = value;
                IsModified = true;
            }
        }

        public void SetValue(string value)
        {
            if (!object.Equals(value, itemValue))
            {
                itemValue = value;
                IsModified = true;
            }
        }

        private void EnsureHasValue()
        {
            if (itemValue == null)
                throw new InvalidOperationException("AttributeItem has no value");
        }

        public bool GetAsBool()
        {
            EnsureHasValue();
            return ((bool?)itemValue).Value;
        }

        public long GetAsInt64()
        {
            EnsureHasValue();
            return ((long?)itemValue).Value;
        }

        public string GetAsString()
        {
            EnsureHasValue();
            return (string)itemValue;
        }

        public object GetOriginalValue()
        {
            return itemOriginalValue;
        }

        public bool IsDataEqual(TikPropertyItem item)
        {
            return object.Equals(item.itemValue, itemValue);
        }

        public override string ToString()
        {
            string originalVal = itemOriginalValue == null ? "null" : itemOriginalValue.ToString();
            string val = itemValue == null ? "null" : itemValue.ToString();

            return IsModified ? originalVal + "->" + val : val;
        }

        public void MarkClear()
        {
            IsModified = false;
            itemOriginalValue = itemValue;
        }
    }
}
