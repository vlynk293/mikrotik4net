using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using Tik4Net.Connector;

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
    public abstract class TikListBase<TEntity>: ITikList, IEnumerable<TEntity>
        where TEntity : TikEntityBase, new()
    {
        private readonly List<TEntity> items;
        private readonly TikSession session;

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
        /// <param name="entity">The entity.</param>
        /// <remarks>Entity must be <see cref="TikEntityBase.IsMarkedNew"/> (it would be created on mikrotik during <see cref="Save"/>).</remarks>
        public void Add(TEntity entity)
        {
            Guard.ArgumentNotNull(entity, "entity");
            BeforeAdd(entity);
            items.Add(entity);
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

        private void LoadInternal(TikConnectorQueryFilterDictionary filter)
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

            SaveAllNew(metadata); //must be before position change!!!
            AfterSaveAllNew(metadata, session);

            SaveAllUpdated(metadata);
            AfterSaveAllUpdated(metadata, session);

            SaveAllDeleted(metadata); // must be after position changes
            AfterSaveAllDeleted(metadata, session);
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
    }
}
