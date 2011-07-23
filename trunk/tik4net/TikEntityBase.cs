using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Tik4Net
{
    /// <summary>
    /// Base class for all strongly-typed Mikrotik entities.
    /// </summary>
    public abstract class TikEntityBase: ITikEntity
    {
        private bool isMarkedDeleted = false;
        private bool isMarkedNew = false;
        private TikPropertyList properties = new TikPropertyList();

        /// <summary>
        /// List of attributes in entity.
        /// </summary>
        protected TikPropertyList Properties
        {
            get { return properties; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modified (any attribute has been modified).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get { return properties.IsModified; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is marked as deleted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is marked as deleted; otherwise, <c>false</c>.
        /// </value>
        /// <seealso cref="MarkDeleted"/>
        public bool IsMarkedDeleted 
        {
            get { return isMarkedDeleted; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is marked as new.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is marked as new; otherwise, <c>false</c>.
        /// </value>
        /// <seealso cref="MarkNew"/>
        public bool IsMarkedNew
        {
            get { return isMarkedNew; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikEntityBase"/> class.
        /// </summary>
        protected TikEntityBase()
        {
            MarkNew();
        }

        #region ITikEntity Members
        /// <summary>
        /// Gets the id of entity.
        /// </summary>
        /// <value>The entity id or null for new instance.</value>
        [TikProperty(".id", typeof(string), true, TikPropertyEditMode.ReadOnly)]
        public string Id
        {
            get { return properties.GetAsStringOrNull(".id"); }
        }       

        /// <summary>
        /// See <see cref="ITikEntity.LoadFromEntityRow"/> for details.
        /// Calls <see cref="OnCustomLoadFromEntityRow"/>.        
        /// </summary>
        /// <remarks>Sets object state to unmodified by <see cref="MarkClear"/> call!</remarks>
        public void LoadFromEntityRow(ITikEntityRow entityRow)
        {
            //attributes.CreateAttribute(".id", entityRow.GetValue(".Id"));
            TikEntityMetadata entityMetadata = TikEntityMetadata.Get(GetType());
            foreach (KeyValuePair<string, TikPropertyAttribute> propPair in entityMetadata.Properties)
            {
                if (propPair.Value.PropertyType == typeof(string))
                    Properties. CreatePropertyWithValue(propPair.Key, entityRow.GetStringValueOrNull(propPair.Key, propPair.Value.Mandatory));
                else if ((propPair.Value.PropertyType == typeof(long)) || propPair.Value.PropertyType == typeof(long?))
                    Properties.CreatePropertyWithValue(propPair.Key, entityRow.GetInt64ValueOrNull(propPair.Key, propPair.Value.Mandatory)); //long.Parse(entityRow.GetValue(propPair.Key), System.Globalization.CultureInfo.CurrentCulture)
                else if ((propPair.Value.PropertyType == typeof(bool)) || (propPair.Value.PropertyType == typeof(bool?)))
                    Properties.CreatePropertyWithValue(propPair.Key, entityRow.GetBoolValueOrNull(propPair.Key, propPair.Value.Mandatory)); //string.Equals(entityRow.GetValue(propPair.Key), "true", StringComparison.OrdinalIgnoreCase)
                else
                    throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "Not supported property '{0}' type '{1}' in {2}.", propPair.Key, propPair.Value.PropertyType, this));
                //catch (FormatException ex)
                //{
                //    throw new FormatException(string.Format("Value '{0}' of property '{1}' can not be parsed to type '{2}' in {3}",
                //        entityRow.GetStringValue(propPair.Key), propPair.Key, propPair.Value.PropertyType, this), ex);
                //}
            }

            OnCustomLoadFromEntityRow(entityRow);

            MarkClear();
        }

        /// <summary>
        /// Called to perform additional custom load entity state (properties) from given <paramref name="entityRow"/>.
        /// </summary>
        /// <param name="entityRow">The entity row.</param>
        protected virtual void OnCustomLoadFromEntityRow(ITikEntityRow entityRow)
        {
            //dummy
        }

        /// <summary>
        /// See <see cref="ITikEntity.IsDataEqual"/> for details.
        /// </summary>
        public bool IsDataEqual(ITikEntity entity)
        {
            if (entity == null)
                return false;
            else if (entity.GetType() != GetType())
                return false;
            else
            {
                return properties.IsDataEqual(((TikEntityBase)entity).Properties);
            }
        }

        #endregion

        /// <summary>
        /// Marks this instance as deleted. Flag could be cleared by <see cref="MarkClear"/> call.
        /// </summary>
        public void MarkDeleted()
        {
            isMarkedNew = false;
            isMarkedDeleted = true;
        }

        /// <summary>
        /// Marks this instance as new. Flag could be cleared by <see cref="MarkClear"/> call.
        /// </summary>
        public void MarkNew()
        {
            //TODO clear id?
            isMarkedDeleted = false;
            isMarkedNew = true;
        }

        /// <summary>
        /// Clears both flags setup by either <see cref="MarkDeleted"/> or <see cref="MarkNew"/> call.
        /// </summary>
        public void MarkClear()
        {
            isMarkedNew = false;
            isMarkedDeleted = false;
            properties.MarkClear();
        }

        internal Dictionary<string, string> GetAllModifiedProperties()
        {
            TikEntityMetadata metadata = TikEntityMetadata.Get(GetType());
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (KeyValuePair<string, TikPropertyAttribute> propPair in metadata.Properties)
            {
                bool found;
                bool modified;
                bool hasValue;
                properties.GetPropertyState(propPair.Key, out found, out modified, out hasValue);
                if ((propPair.Value.EditMode == TikPropertyEditMode.Editable) && found && modified)
                {
                    string valueAsText;
                    if (!hasValue)
                        valueAsText = "";
                    else
                    {
                        if (propPair.Value.PropertyType == typeof(string))
                            valueAsText = properties.GetAsString(propPair.Key);
                        else if ((propPair.Value.PropertyType == typeof(long)) || propPair.Value.PropertyType == typeof(long?))
                            valueAsText = properties.GetAsInt64(propPair.Key).ToString(CultureInfo.InvariantCulture);
                        else if ((propPair.Value.PropertyType == typeof(bool)) || (propPair.Value.PropertyType == typeof(bool?)))
                            valueAsText = properties.GetAsBoolean(propPair.Key) ? "yes" : "no";
                        else
                            throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "Not supported property '{0}' type '{1}' in {2}.", propPair.Key, propPair.Value.PropertyType, this));
                    }
                    result.Add(propPair.Key, valueAsText);
                }
            }

            return result;
        }

        /// <summary>
        /// Assign property values and state flags from given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">Source entity.</param>
        public void Assign(TikEntityBase entity)
        {
            //assign flags
            isMarkedDeleted = entity.isMarkedDeleted;
            isMarkedNew = entity.isMarkedNew;

            //assign property values
            properties = entity.Properties; //not very clear ... copy of values could be better solution!
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            List<string> modificators = new List<string>();
            if (IsMarkedDeleted)
                modificators.Add("D");
            if (IsMarkedNew)
                modificators.Add("I");
            string modificator = string.Join(",", modificators.ToArray());
            if (!string.IsNullOrEmpty(modificator))
                modificator = "[" + modificator + "]";

            return string.Format(CultureInfo.CurrentCulture, "{0}{1}: {2}", GetType().Name, modificator, Properties.ToString());
        }
    }
}
