using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    internal class TikPropertyItem
    {
        private object itemValue;

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

        //public string GetStringRepresentation()
        //{
        //    if (HasValue)
        //    {
        //        //TODO handle true/false vs. yes/no in boolean
        //        if (itemValue is bool?)
        //            return ((bool?)itemValue).Value ? "yes" : "no";
        //        else
        //            return itemValue.ToString();
        //    }
        //    else
        //        return string.Empty;
        //}

        public bool IsDataEqual(TikPropertyItem item)
        {
            return object.Equals(item.itemValue, itemValue);
        }
    }
}
