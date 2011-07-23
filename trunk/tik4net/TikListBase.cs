using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using Tik4Net.Connector;
using Tik4Net.Logging;
using System.Collections;

namespace Tik4Net
{
    /// <summary>
    /// Base class for list of <see cref="TikEntityBase"/>.
    /// Supports enumeration, Load access (<see cref="LoadAll"/>, <see cref="LoadItem"/>)
    /// and Save access (<see cref="Save"/>).
    /// <para>
    /// Uses <see cref="TikSession"/> given in constructor or obtain by
    /// <see cref="TikSession.ActiveSession"/> call.
    /// </para>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity in list.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class TikListBase<TEntity>: ITikList, IEnumerable<TEntity>, IList, IList<TEntity>
        where TEntity : TikEntityBase, new()
    {
        private readonly object lockObj = new object();
        private readonly List<TEntity> items;
        private readonly TikSession session;
        private readonly ILog logger;

        /// <summary>
        /// Gets the count of items that are <see cref="TikEntityBase.IsMarkedNew"/>.
        /// </summary>
        /// <value>The count of new items.</value>
        public int NewCount
        {
            get { return items.Count(i => i.IsMarkedNew); }
        }

        /// <summary>
        /// Gets the count of items that are <see cref="TikEntityBase.IsMarkedDeleted"/>.
        /// </summary>
        /// <value>The count of deleted items.</value>
        public int DeletedCount
        {
            get { return items.Count(i => i.IsMarkedDeleted); }
        }

        /// <summary>
        /// Gets the count of items that are <see cref="TikEntityBase.IsModified"/> (and are not deleted or new).
        /// </summary>
        /// <value>The count of updated items.</value>
        public int UpdatedCount
        {
            get { return items.Count(i => i.IsModified && !i.IsMarkedNew && !i.IsMarkedDeleted); }
        }

        /// <summary>
        /// Gets the items in list.
        /// </summary>
        /// <value>The items in list.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<TEntity> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Gets the logger object (see <see cref="TikSession.SetLogFactory"/>)
        /// that allows write log messages (debug messages).
        /// </summary>
        /// <value>The logger object.</value>
        protected ILog Logger
        {
            get { return logger; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modified (any item in list is <see cref="TikEntityBase.IsModified"/>).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsModified
        {
            get
            {
                foreach (TEntity item in items)
                {
                    if (item.IsModified)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikListBase&lt;TEntity&gt;"/> class with <see cref="TikSession.ActiveSession"/> session.
        /// </summary>
        protected TikListBase()
            : this( TikSession.ActiveSession)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikListBase&lt;TEntity&gt;"/> class with given <paramref name="session"/>.
        /// </summary>
        /// <param name="session">The session.</param>
        protected TikListBase(TikSession session)
        {
            Guard.ArgumentNotNull(session, "session");

            this.items = new List<TEntity>();
            this.session = session;
            this.logger = TikSession.CreateLogger(GetType());
        }

        #region -- ITEMS --

        /// <summary>
        /// Clears list of items.
        /// </summary>
        protected virtual void Clear()
        {
            items.Clear();
            //TODO Clear changes?
        }

        /// <summary>
        /// Adds the specified entity to the end of item list.
        /// </summary>
        /// <param name="item">The entity.</param>
        /// <remarks>Entity must be <see cref="TikEntityBase.IsMarkedNew"/> (it would be created on mikrotik during <see cref="Save"/>).</remarks>
        public void Add(TEntity item)
        {
            Guard.ArgumentNotNull(item, "item");
            BeforeAdd(item);
            items.Add(item);
        }

        /// <summary>
        /// Verifies given <paramref name="entity"/> if it could be added to list of items.
        /// </summary>
        /// <param name="entity">The entity to be verified.</param>
        protected virtual void BeforeAdd(TEntity entity)
        {
            if (!entity.IsMarkedNew)
                throw new InvalidOperationException("Can not add entity that is not IsMarkedNew.");            
        }

        #endregion

        #region -- LOAD --
        /// <summary>
        /// Loads all items (without filtering).
        /// Uses session from constructor.
        /// </summary>
        public void LoadAll()
        {
            LoadInternal(null);
        }

        /// <summary>
        /// Loads one item by its id.
        /// Uses session from constructor.
        /// </summary>
        /// <param name="id">The item id.</param>
        /// <returns>Loaded item with given id or null.</returns>
        public TEntity LoadItem(string id)
        {
            Guard.ArgumentNotNullOrEmptyString(id, "id");

            TikConnectorQueryFilterDictionary filter = new TikConnectorQueryFilterDictionary();
            filter.Add(".id", id);

            List<TEntity> loadedItems = LoadItemsInternal(filter, session);
            if (loadedItems.Count > 1)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "More than one item with id {0} returned in {1}.", id, GetType()));
            else if (loadedItems.Count == 0)
                return null; 
            else
                return loadedItems[0];
        }

        /// <summary>
        /// Loads the list (with given filter).
        /// </summary>
        /// <param name="filter">The list filter.</param>
        protected void LoadInternal(TikConnectorQueryFilterDictionary filter)
        {
            Clear();
            items.AddRange(LoadItemsInternal(filter, session));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "session")]
        private List<TEntity> LoadItemsInternal(TikConnectorQueryFilterDictionary filter, TikSession session)
        {
            List<TEntity> result = new List<TEntity>();

            TikEntityMetadata entityMetadata = TikEntityMetadata.Get(typeof(TEntity));
            IEnumerable<ITikEntityRow> response;
            if (filter != null)
                response = session.Connector.ExecuteReader(entityMetadata.EntityPath, entityMetadata.PropertyNames, filter);
            else
                response = session.Connector.ExecuteReader(entityMetadata.EntityPath, entityMetadata.PropertyNames);

            VerifyResponseRows(response);

            foreach (ITikEntityRow entityRow in response)
            {
                TEntity item = new TEntity();
                item.LoadFromEntityRow(entityRow);
                result.Add(item);
            }

            logger.DebugFormat("{0} items loaded.", result.Count);
            return result;
        }

        /// <summary>
        /// Verifies the response rows - called after items load.
        /// Method could be overriden to implement verify process.
        /// </summary>
        /// <param name="response">The response rows.</param>
        protected virtual void VerifyResponseRows(IEnumerable<ITikEntityRow> response)
        {
            //dummy
        }
        #endregion

        #region -- SAVE --
        /// <summary>
        /// Saves this instance - saves all entities tha are in <see cref="TikEntityBase.IsModified"/>, 
        /// <see cref="TikEntityBase.IsMarkedDeleted"/> and <see cref="TikEntityBase.IsMarkedNew"/> states.
        /// Uses session from constructor.
        /// </summary>
        public void Save()
        {
            TikEntityMetadata metadata = TikEntityMetadata.Get(typeof(TEntity));

            logger.DebugFormat("Going to save {0}/{1}/{2} new/update/delete items.", NewCount, UpdatedCount, DeletedCount);

            SaveAllNew(metadata); //must be before position change!!!
            AfterSaveAllNew(metadata, session);

            SaveAllUpdated(metadata);
            AfterSaveAllUpdated(metadata, session);

            SaveAllDeleted(metadata); // must be after position changes
            AfterSaveAllDeleted(metadata, session);

            logger.Debug("Successfully saved.");
        }

        /// <summary>
        /// Called after all rows in <see cref="TikEntityBase.IsMarkedNew"/> are saved.
        /// Method could be overriden to implement process.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <param name="session">The session to save with.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "session")]
        protected virtual void AfterSaveAllNew(TikEntityMetadata metadata, TikSession session)
        {
            //dummy
        }

        /// <summary>
        /// Called after all rows in <see cref="TikEntityBase.IsModified"/> are saved.
        /// Method could be overriden to implement process.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <param name="session">The session to save with.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "session")]
        protected virtual void AfterSaveAllUpdated(TikEntityMetadata metadata, TikSession session)
        {
            //dummy
        }

        /// <summary>
        /// Called after all rows in <see cref="TikEntityBase.IsMarkedDeleted"/> are saved.
        /// Method could be overriden to implement process.
        /// </summary>
        /// <param name="metadata">The entity metadata.</param>
        /// <param name="session">The session to save with.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "session")]
        protected virtual void AfterSaveAllDeleted(TikEntityMetadata metadata, TikSession session)
        {
            //dummy
        }

        private void SaveAllNew(TikEntityMetadata metadata)
        {
            for (int i = 0; i < items.Count; i++) //REMARKS msut be (0 -> count) because of order items added to end
            {
                TEntity entity = items[i];
                if (entity.IsMarkedNew)
                {
                    Dictionary<string, string> values = entity.GetAllModifiedProperties();

                    Logger.InfoFormat("CREATE: {0}", entity);

                    string newId = session.Connector.ExecuteCreate(metadata.EntityPath, values);
                    TEntity newEntity = LoadItem(newId);
                    items[i].Assign(newEntity); //put saved&loaded entity DATA (pointer can not be changed) instead of dirty old-ones data 
                }
            }
        }

        private void SaveAllUpdated(TikEntityMetadata metadata)
        {
            for (int i = 0; i < items.Count; i++)
            {
                TEntity entity = items[i];
                if (entity.IsModified)
                {                    
                    Dictionary<string, string> valuesToSet = new Dictionary<string, string>();
                    List<string> propertiesToUnset = new List<string>();
                    foreach(KeyValuePair<string, string> pair in entity.GetAllModifiedProperties())
                    {
                        if (pair.Value == null)
                            propertiesToUnset.Add(pair.Key);
                        else
                            valuesToSet.Add(pair.Key, pair.Value);
                    }

                    Logger.InfoFormat("UPDATE: {0}", entity);
                    
                    if (valuesToSet.Count > 0)
                        session.Connector.ExecuteSet(metadata.EntityPath, entity.Id, valuesToSet);
                    if (propertiesToUnset.Count > 0)
                        session.Connector.ExecuteUnset(metadata.EntityPath, entity.Id, propertiesToUnset);

                    TEntity newEntity = LoadItem(entity.Id);
                    items[i] = newEntity; //put saved&loaded entity into list instead of dirty old-one
                }
            }
        }

        private void SaveAllDeleted(TikEntityMetadata metadata)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                TEntity entity = items[i];
                if (entity.IsMarkedDeleted)
                {
                    Logger.InfoFormat("DELETE: {0}", entity);

                    session.Connector.ExecuteDelete(metadata.EntityPath, entity.Id);
                    items.RemoveAt(i); //remove deleted entity from list
                }
            }
        }
        #endregion

        #region IEnumerable<TEntity> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TEntity> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            TEntity castedValue = (TEntity)value;

            Add(castedValue);
            return IndexOf(castedValue);
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            return ((IList)items).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)items).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            TEntity castedValue = (TEntity)value;

            Insert(index, castedValue);
        }

        /// <summary>
        /// See <see cref="IList.IsFixedSize"/> for details.
        /// </summary>
        public abstract bool IsFixedSize { get; }

        /// <summary>
        /// See <see cref="IList.IsReadOnly"/> for details.
        /// </summary>
        public abstract bool IsReadOnly { get ; }

        void IList.Remove(object value)
        {
            TEntity castedValue = (TEntity)value;

            Remove(castedValue);
        }

        object IList.this[int index]
        {
            get { return items[index]; }
            set 
            { 
                TEntity castedValue = (TEntity)value;
                items[index] = castedValue;            
            }
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// See <see cref="ICollection.CopyTo"/> for details.
        /// </summary>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)items).CopyTo(array, index);
        }

        /// <summary>
        /// See <see cref="ICollection.Count"/> for details.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// See <see cref="ICollection.IsSynchronized"/> for details. Is always false.
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// See <see cref="ICollection.SyncRoot"/> for details. 
        /// </summary>
        public object SyncRoot
        {
            get { return lockObj; }
        }

        #endregion

        #region IList<TEntity> Members

        /// <summary>
        /// See <see cref="IList{T}.IndexOf"/> for details.
        /// </summary>
        public int IndexOf(TEntity item)
        {
            return items.IndexOf(item);
        }

        /// <summary>
        /// See <see cref="IList{T}.Insert"/> for details.
        /// </summary>
        public void Insert(int index, TEntity item)
        {
            OnInsert(index, item);
        }

        /// <summary>
        /// Called from <see cref="Insert"/>. Must be implemented by descendant class.
        /// </summary>
        /// <param name="index">The index where item should be inserted.</param>
        /// <param name="item">The item to be inserted.</param>
        protected abstract void OnInsert(int index, TEntity item);

        /// <summary>
        /// See <see cref="IList{T}.RemoveAt"/> for details.
        /// </summary>
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        /// <summary>
        /// See <see cref="IList{T}"/> for details.
        /// </summary>
        public TEntity this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        #endregion

        #region ICollection<TEntity> Members

        /// <summary>
        /// See <see cref="ICollection{T}.Clear"/> for details.
        /// </summary>
        void ICollection<TEntity>.Clear()
        {
            Clear();
        }

        /// <summary>
        /// See <see cref="ICollection{T}.Contains"/> for details.
        /// </summary>
        public bool Contains(TEntity item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// See <see cref="ICollection{T}.CopyTo"/> for details.
        /// </summary>
        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// See <see cref="ICollection{T}.Remove"/> for details.
        /// </summary>
        public bool Remove(TEntity item)
        {
            return items.Remove(item);
        }

        #endregion
    }
}
