using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using Tik4Net.Connector;

namespace Tik4Net
{
    public abstract class TikListBase<TEntity>: ITikList, IEnumerable<TEntity>
        where TEntity : TikEntityBase, new()
    {
        private readonly List<TEntity> items;
        private readonly TikSession session;

        protected List<TEntity> Items
        {
            get { return items; }
        }

        protected TikListBase()
            : this( TikSession.ActiveSession)
        {

        }

        protected TikListBase(TikSession session)
        {
            Guard.ArgumentNotNull(session, "session");

            this.items = new List<TEntity>();
            this.session = session;
        }

        #region -- ITEMS --
        protected virtual void Clear()
        {
            items.Clear();
            //TODO Clear changes
        }

        public void Add(TEntity entity)
        {
            Guard.ArgumentNotNull(entity, "entity");
            BeforeAdd(entity);

            if (!entity.IsMarkedNew)
                throw new InvalidOperationException("Can not add entity that is not IsMarkedNew.");            
            items.Add(entity);
        }

        protected virtual void BeforeAdd(TEntity entity)
        {
            //dummy
        }

        #endregion

        #region -- LOAD --
        public void LoadAll()
        {
            LoadInternal(null);
        }

        public TEntity LoadItem(string id)
        {
            Guard.ArgumentNotNullOrEmptyString(id, "id");

            Dictionary<string, string> filter = new Dictionary<string, string>(1);
            filter.Add(".id", id);

            List<TEntity> loadedItems = LoadItemsInternal(filter, session);
            if (loadedItems.Count > 1)
                throw new InvalidOperationException(string.Format("More than one item with id {0} returned in {1}.", id, GetType()));
            else if (loadedItems.Count == 0)
                return null; //TODO or exception?
            else
                return loadedItems[0];
        }

        private void LoadInternal(Dictionary<string, string> filter)
        {
            Clear();
            items.AddRange(LoadItemsInternal(filter, session));
        }

        private List<TEntity> LoadItemsInternal(Dictionary<string, string> filter, TikSession session)
        {
            List<TEntity> result = new List<TEntity>();

            TikEntityMetadata entityMetadata = TikEntityMetadata.Get(typeof(TEntity));
            IEnumerable<ITikEntityRow> response;
            if (filter != null)
                response = session.Connector.QueryDataRows(entityMetadata.EntityPath, entityMetadata.PropertyNames, filter);
            else
                response = session.Connector.QueryDataRows(entityMetadata.EntityPath, entityMetadata.PropertyNames);

            VerifyResponseRows(response);

            foreach (ITikEntityRow entityRow in response)
            {
                TEntity item = new TEntity();
                item.LoadFromEntityRow(entityRow);
                result.Add(item);
            }

            return result;
        }

        protected virtual void VerifyResponseRows(IEnumerable<ITikEntityRow> response)
        {
            //dummy
        }
        #endregion

        #region -- SAVE --
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

        protected virtual void AfterSaveAllNew(TikEntityMetadata metadata, TikSession session)
        {
            //dummy
        }

        protected virtual void AfterSaveAllUpdated(TikEntityMetadata metadata, TikSession session)
        {
            //dummy
        }

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
                    Dictionary<string, string> values = entity.GetAllModifiedProperties();

                    session.Connector.ExecuteUpdate(metadata.EntityPath, entity.Id, values);
                    TEntity newEntity = LoadItem(entity.Id);
                    items[i] = newEntity; //put saved&loaded entity into list instead of dirty old-one

                    //TODO have to add support for unset command!!!
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
