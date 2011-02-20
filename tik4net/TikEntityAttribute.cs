using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TikEntityAttribute: Attribute
    {
        public string EntityPath { get; private set; }

        public TikEntityEditMode EditMode { get; private set; }

        public TikEntityAttribute(string entityPath, TikEntityEditMode editMode)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            EntityPath = entityPath;
            EditMode = editMode;
        }
    }
}
