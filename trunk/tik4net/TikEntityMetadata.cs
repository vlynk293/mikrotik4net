using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Tik4Net
{
    public sealed class TikEntityMetadata
    {
        private static object lockObj = new object();
        private static Dictionary<Type, TikEntityMetadata> cache = new Dictionary<Type,TikEntityMetadata>();

        public static TikEntityMetadata Get(Type entityType)
        {
            Guard.ArgumentNotNull(entityType, "entityType");

            TikEntityMetadata result;
            bool found;
            lock(lockObj)
            {
                found = cache.TryGetValue(entityType, out result);
            }

            if (!found)
            {
                //resolve out of the lock
                result = new TikEntityMetadata(entityType);                
            }

            lock(lockObj)
            {
                if (!cache.ContainsKey(entityType))
                    cache.Add(entityType, result);
            }

            return result;
        }

        private TikEntityAttribute entityInfo;
        private Dictionary<string, TikPropertyAttribute> propertiesInfo;

        public string EntityPath
        {
            get { return entityInfo.EntityPath; }
        }

        public IEnumerable<string> PropertyNames
        {
            get { return propertiesInfo.Keys; }
        }

        public IEnumerable<KeyValuePair<string, TikPropertyAttribute>> Properties
        {
            get { return propertiesInfo; }
        }

        private TikEntityMetadata(Type entityType)
        {
            Guard.ArgumentNotNull(entityType, "entityType");

            ParseMetadata(entityType);
        }

        private void ParseMetadata(Type entityType)
        {
            //type info
            TikEntityAttribute entityAttr = entityType.GetCustomAttributes(false).FirstOrDefault(a => a is TikEntityAttribute) as TikEntityAttribute;
            if (entityAttr != null)
                entityInfo = entityAttr;
            else
                throw new InvalidOperationException(string.Format("Entity {0} is not marked by {1} attribute.", entityType, typeof(TikEntityAttribute)));

            //properties info
            propertiesInfo = new Dictionary<string, TikPropertyAttribute>();
            foreach (PropertyInfo propInfo in entityType.GetProperties())
            {
                TikPropertyAttribute propAttr = propInfo.GetCustomAttributes(false).FirstOrDefault(a => a is TikPropertyAttribute) as TikPropertyAttribute;
                if (propAttr != null)
                {
                    Type propType = propAttr.PropertyType;
                    if (propType != propInfo.PropertyType)
                    {
                        throw new InvalidProgramException(string.Format(CultureInfo.CurrentCulture, "Inconsistent property '{0} type in {1}. Real type {2}, declared in TikPropertyAttribute {3}.",
                            propInfo.Name, entityType, propInfo.PropertyType, propType));
                    }
                    propertiesInfo.Add(propAttr.PropertyName, propAttr);
                }
            }
        }
    }
}
