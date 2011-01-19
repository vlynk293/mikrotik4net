using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    internal class AttributeItem
    {
        private object itemValue;

        public bool IsModified { get; private set; }

        public bool HasValue 
        { 
            get {  return itemValue != null; }
        }

        public AttributeItem()
        {
            IsModified = false;
        }

        public AttributeItem(bool value)
            : this()
        {
            itemValue = value;
        }

        public AttributeItem(long value)
            : this()
        {
            itemValue = value;
        }

        public AttributeItem(string value)
            : this()
        {
            itemValue = value;
        }

        public void SetValue(bool value)
        {
            if (object.Equals(value, itemValue))
            {
                itemValue = value;
                IsModified = true;
            }
        }

        public void SetValue(long value)
        {
            if (object.Equals(value, itemValue))
            {
                itemValue = value;
                IsModified = true;
            }
        }

        public void SetValue(string value)
        {
            if (object.Equals(value, itemValue))
            {
                itemValue = value;
                IsModified = true;
            }
        }

        public bool GetAsBool()
        {
            if (itemValue == null)
                return false;
            else
                return (bool)itemValue;
        }

        public long GetAsInt64()
        {
            if (itemValue == null)
                return 0;
            else
                return (long)itemValue;
        }

        public string GetAsString()
        {
            if (itemValue == null)
                return string.Empty;
            else
                return (string)itemValue;
        }
    }
}
