using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Tik4Net
{
    /// <summary>
    /// Metadata for <see cref="ITikEntity"/> (<see cref="TikEntityBase"/>) cache parsed from attributes 
    /// that is decorating entity.
    /// </summary>
    /// <seealso cref="TikEntityAttribute"/>
    /// <seealso cref="TikPropertyAttribute"/>
    public sealed class TikEntityMetadata
    {
        private static object lockObj = new object();
        private static Dictionary<Type, TikEntityMetadata> cache = new Dictionary<Type,TikEntityMetadata>();

        /// <summary>
        /// Gets the metadata for specified entity type (if metadata are not present in cache,
        /// than it reads them by attribute reflection).
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>Metadata for given type read by reflection form entity attributes.</returns>
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

        /// <summary>
        /// Gets the entity path in API notation 
        /// (parsed from <see cref="TikEntityAttribute.EntityPath"/> on entity object).
        /// </summary>
        /// <value>The entity path.</value>
        public string EntityPath
        {
            get { return entityInfo.EntityPath; }
        }

        /// <summary>
        /// Gets the read flags (modifies ExecuteReader behavior).
        /// </summary>
        /// <value>The read flags (modifies ExecuteReader behavior).</value>
        public Tik4Net.Connector.ExecuteReaderBehaviors ReaderBehavior
        {
            get { return entityInfo.ReaderBehavior; }
        }

        /// <summary>
        /// Gets the list of property (mikrotik properties) names in entity.
        /// All properties should be marked by <see cref="TikPropertyAttribute"/>.
        /// </summary>
        /// <value>The property names in entity.</value>
        public IEnumerable<string> PropertyNames
        {
            get { return propertiesInfo.Keys; }
        }

        /// <summary>
        /// Gets the list of properties (mikrotik properties) in entity and their infos.
        /// All properties should be marked by <see cref="TikPropertyAttribute"/>.
        /// </summary>
        /// <value>The property-propInfo pair list.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
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
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Entity {0} is not marked by {1} attribute.", entityType, typeof(TikEntityAttribute)));

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
