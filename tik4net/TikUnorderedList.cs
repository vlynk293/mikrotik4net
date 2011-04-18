using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// <see cref="TikListBase{TEntity}"/> type that contains unordered items 
    /// (moving items up/down doesn't make sense).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="TikListMode.NotOrdered"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TikUnorderedList<TEntity>: TikListBase<TEntity> 
        where TEntity : TikEntityBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TikUnorderedList&lt;TEntity&gt;"/> class.
        /// Default active session (<see cref="TikSession.ActiveSession"/> is used).
        /// </summary>
        protected TikUnorderedList() 
            : base()
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikUnorderedList&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="session">The session used to access mikrotik.</param>
        protected TikUnorderedList(TikSession session)
            : base(session)
        {
        }

        private static bool EntityKeyEqual(TEntity e1, TEntity e2, Func<TEntity, object> keyExtractor)
        {
            return object.Equals(keyExtractor(e1), keyExtractor(e2));
        }

        /// <summary>
        /// Merges the <paramref name="data"/> into given <paramref name="subset"/> (part of this list).
        /// <para>New items from <paramref name="data"/> are added into this list</para>
        /// <para>items that are in <paramref name="subset"/> but are missing in data are <see cref="TikEntityBase.MarkDeleted"/>.</para> 
        /// <para>Items with the same key (<paramref name="keyExtractor"/>) are updated by <paramref name="updateDataAction"/>.</para>
        /// <example>
        /// //update mikrotik router to state in database
        /// listInMikrotik = LoadListFromMikrotik();
        /// listInDb = LoadListFromMikrotik();
        /// //merge the whole list
        /// listInMikrotik.MergeSubset(listInMikrotik, listInDb, i =&gt; i.Id, (dst, src) => { dst.Name = src.Name; dst.Priority = src.Priority; } );
        /// listInMikrotik.Save();
        /// </example>
        /// </summary>
        /// <param name="subset">The subset in this list (the same filter as in <paramref name="data"/>).</param>
        /// <param name="data">The data to be metged into this list.</param>
        /// <param name="keyExtractor">The key extractor - should return key from given entity (not .id property) - items with the same key are treated as the same instances.</param>
        /// <param name="updateDataAction">The update data action - called to assign entity data from <paramref name="data"/> item into <paramref name="subset"/> item.</param>
        /// <remarks><see cref="TikUnorderedList{TEntity}"/> is not ordered -} method doesn't care about order of items.</remarks>
        /// <seealso cref="Merge"/>
        public void MergeSubset(IEnumerable<TEntity> subset, IEnumerable<TEntity> data, Func<TEntity, object> keyExtractor, Action<TEntity, TEntity> updateDataAction)
        {
            Guard.ArgumentNotNull(subset, "subset");
            Guard.ArgumentNotNull(data, "data");
            Guard.ArgumentNotNull(keyExtractor, "keyExtractor");
            Guard.ArgumentNotNull(updateDataAction, "updateDataAction");

            List<TEntity> subsetList = subset.ToList();
            List<TEntity> dataList = data.ToList();

            foreach (TEntity subsetEntity in subsetList)
                subsetEntity.MarkDeleted(); //will be cleared if found in dataList

            foreach (TEntity dataEntity in dataList)
            {
                TEntity subsetEntity = subsetList.FirstOrDefault(e => EntityKeyEqual(e, dataEntity, keyExtractor));
                if (subsetEntity != null)
                {
                    updateDataAction(subsetEntity, dataEntity);
                    subsetEntity.MarkClear();
                }
                else
                    Add(dataEntity);
            }
        }

        /// <summary>
        /// Merges the <paramref name="data"/> into given <paramref name="subset"/> this list.
        /// <para>New items from <paramref name="data"/> are added into this list</para>
        /// <para>items that are in <paramref name="subset"/> but are missing in data are <see cref="TikEntityBase.MarkDeleted"/>.</para> 
        /// <para>Items with the same key (<paramref name="keyExtractor"/>) are updated by <paramref name="updateDataAction"/>.</para>
        /// <example>
        /// //update mikrotik router to state in database
        /// listInMikrotik = LoadListFromMikrotik();
        /// listInDb = LoadListFromMikrotik();
        /// //merge the whole list
        /// listInMikrotik.Merge(listInDb, i =&gt; i.Id, (dst, src) => { dst.Name = src.Name; dst.Priority = src.Priority; } );
        /// listInMikrotik.Save();
        /// </example>
        /// </summary>
        /// <param name="data">The data to be metged into this list.</param>
        /// <param name="keyExtractor">The key extractor - should return key from given entity (not .id property) - items with the same key are treated as the same instances.</param>
        /// <param name="updateDataAction">The update data action - called to assign entity data from <paramref name="data"/> item into <paramref name="subset"/> item.</param>
        /// <remarks><see cref="TikUnorderedList{TEntity}"/> is not ordered -} method doesn't care about order of items.</remarks>
        /// <seealso cref="MergeSubset"/>
        public void Merge(IEnumerable<TEntity> data, Func<TEntity, object> keyExtractor, Action<TEntity, TEntity> updateDataAction)
        {
            MergeSubset(this, data, keyExtractor, updateDataAction);
        }
    }
}
