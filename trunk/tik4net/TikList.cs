using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;
using System.Globalization;

namespace Tik4Net
{
    /// <summary>
    /// <see cref="TikListBase{TEntity}"/> type that contains ordered items 
    /// (moving items up/down by <see cref="Move"/> and <see cref="MoveToEnd"/> does make sense).
    /// </summary>
    /// <seealso cref="TikListMode.Ordered"/>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TikList<TEntity> : TikListBase<TEntity> 
        where TEntity : TikEntityBase, new()
    {
        private readonly Dictionary<TEntity, TEntity> entityMoves = new Dictionary<TEntity, TEntity>(); //<entityToMove,entityToMoveBefore>


        /// <summary>
        /// List is modified if  <see cref="TikListBase{TEntity}.IsModified"/> or any move has been done.
        /// </summary>
        public override bool IsModified
        {
            get
            {
                if (entityMoves.Count > 0)
                    return true;
                else
                    return base.IsModified;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikList&lt;TEntity&gt;"/> class.
        /// Default active session (<see cref="TikSession.ActiveSession"/> is used).
        /// </summary>
        protected TikList() 
            : base()
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikList&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="session">The session used to access mikrotik.</param>
        protected TikList(TikSession session)
            : base(session)
        {
        }

        #region -- ITEMS --
        /// <summary>
        /// Inserts the specified entity before <paramref name="entityToInsertBefore"/> entity.
        /// <para>
        /// Calls <see cref="TikListBase{TEntity}.Add"/> and <see cref="Move"/> methods.
        /// </para>
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="entityToInsertBefore">The entity before which is <paramref name="entity"/> inserted.</param>
        /// <remarks>Use <see cref="TikListBase{TEntity}.Add"/> to add item to the end of list.</remarks>
        public void Insert(TEntity entity, TEntity entityToInsertBefore)
        {
            Add(entity);

            Move(entity, entityToInsertBefore);
        }
        #endregion

        #region -- MOVE --
        /// <summary>
        /// Gets the count of moves that will be performed during save.
        /// </summary>
        /// <value>The count of moves.</value>
        public int MovesCount
        {
            get { return entityMoves.Count; }
        }

        /// <summary>
        /// Moves the specified <paramref name="entityToMove"/> before another <paramref name="entityToMoveBefore"/>.
        /// <para>
        /// Entity is moved in internal list of items and info about move is stored in internal list of moves 
        /// to enable perform this move during <see cref="TikListBase{TEntity}.Save"/> process.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Entity is moved maximally once - if you move entity more than once, only last move is performed.
        /// It can produce some strange behavior if you move entity B somewhere, than move entity A before B and than move entity B somewhere else.
        /// </remarks>
        /// <param name="entityToMove">The entity to move.</param>
        /// <param name="entityToMoveBefore">The entity before which is given <paramref name="entityToMove"/> moved.</param>
        public void Move(TEntity entityToMove, TEntity entityToMoveBefore)
        {
            //TODO ensure entity not deleted, etc ...
            //TODO what about newly created entities?

            Guard.ArgumentNotNull(entityToMove, "entityToMove");
            Guard.ArgumentNotNull(entityToMoveBefore, "entityToMoveBefore");

            EnsureEntityInList(entityToMove, "entityToMove");
            EnsureEntityInList(entityToMoveBefore, "entityToMoveBefore");

            int entityToMoveIdx = Items.IndexOf(entityToMove);
            int entityToMoveBeforeIdx = Items.IndexOf(entityToMoveBefore);

            if (entityToMoveIdx != entityToMoveBeforeIdx - 1) //not on final position
            {
                //move in list
                Items.Remove(entityToMove);
                Items.Insert(entityToMoveBeforeIdx, entityToMove);

                //remember move (insert or update)
                entityMoves[entityToMove] = entityToMoveBefore;
            }
        }

        /// <summary>
        /// Calls <see cref="Move"/> or <see cref="MoveToEnd"/> if <paramref name="entityToMoveBefore"/> is null.
        /// </summary>
        /// <param name="entity">The entity to be moved.</param>
        /// <param name="entityToMoveBefore">The entity to move before (or null for move to end).</param>
        public void MoveOrMoveToEnd(TEntity entity, TEntity entityToMoveBefore)
        {
            Guard.ArgumentNotNull(entity, "entity");

            if (entityToMoveBefore == null)
                MoveToEnd(entity);
            else
                Move(entity, entityToMoveBefore);
        }

        /// <summary>
        /// The same as <see cref="Move"/> but moves given <paramref name="entityToMove"/> to the end of list.
        /// </summary>        
        public void MoveToEnd(TEntity entityToMove)
        {
            Guard.ArgumentNotNull(entityToMove, "entityToMove");
            EnsureEntityInList(entityToMove, "entityToMove");

            if (Items.IndexOf(entityToMove) < Items.Count - 1) //is not at the end
            {
                Items.Remove(entityToMove);
                Items.Add(entityToMove);

                entityMoves.Add(entityToMove, null);
            }
        }

        private void EnsureEntityInList(TEntity entity, string argumentName)
        {
            if (Items.IndexOf(entity) < 0)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "Given entity '{0}' not found in list.", entity), argumentName);
        }

        //protected override void AddEntityBeforeRef(TEntity entity, TEntity entityToMoveBefore)
        //{
        //    base.AddEntityBeforeRef(entity, entityToMoveBefore);

        //    if (entityToMoveBefore != null)
        //        Move(entity, entityToMoveBefore);
        //}

        /// <summary>
        /// See <see cref="TikListBase{TEntity}.Clear"/>. 
        /// Additionaly clears list of moves.
        /// </summary>
        protected override void Clear()
        {
            base.Clear();
            ClearMoves();
        }

        /// <summary>
        /// Clears the moves collection (no moves will be performed during Save).
        /// </summary>
        /// <remarks>
        /// Move method call changes order of items in collection and <see cref="ClearMoves"/>
        /// does not rostore previous order!
        /// </remarks>
        public void ClearMoves()
        {
            entityMoves.Clear();
        }

        /// <summary>
        /// See <see cref="TikListBase{TEntity}.AfterSaveAllNew"/>. 
        /// Additionaly save all moves to mikrotik (see <see cref="ITikConnector.ExecuteMove"/>).
        /// </summary>
        protected override void AfterSaveAllNew(TikEntityMetadata metadata, TikSession session)
        {
            base.AfterSaveAllNew(metadata, session);

            SaveAllMoves(metadata, session);
        }

        private void SaveAllMoves(TikEntityMetadata metadata, TikSession session)
        {
            foreach (KeyValuePair<TEntity, TEntity> pair in entityMoves)
            {
                if (pair.Value != null)
                    session.Connector.ExecuteMove(metadata.EntityPath, pair.Key.Id, pair.Value.Id);
                else
                    session.Connector.ExecuteMoveToEnd(metadata.EntityPath, pair.Key.Id);
            }
            entityMoves.Clear();
        }
        #endregion

        /// <summary>
        /// Gets the first row that matches <paramref name="firstItemPredicate"/>
        /// and than scrolls down the list and returns all its siblings that matches <paramref name="siblingPredicate"/>.
        /// Stops on the first item that doesn't match given <paramref name="siblingPredicate"/>.
        /// <example>
        /// //Gets all 'jump' firewall mangle items that are in the block that starts with 'jump' item with specified comment.
        /// IEnumerable{FirewalMangle} jumps = firewallMangleList.GetAllSiblings(i=&gt;i.Commen=="MARKING_COMMENT", i=&gt;i.Action=="jump");
        /// </example>
        /// </summary>
        /// <param name="firstItemPredicate">The first item predicate (how to find beginig of block).</param>
        /// <param name="siblingPredicate">The sibling predicate (how to identify each item in block).</param>
        /// <returns>
        /// Items from block (block starts with <paramref name="firstItemPredicate"/> item) that each match given <paramref name="siblingPredicate"/>.
        /// </returns>
        /// <remarks>Typycal usage is together with <see cref="MergeSubset"/> to get block subset to be destionation of merge.</remarks>
        public IEnumerable<TEntity> GetAllSiblings(Func<TEntity, bool> firstItemPredicate, Func<TEntity, bool> siblingPredicate)
        {
            TEntity firstItem = Items.FirstOrDefault(i => firstItemPredicate(i) && siblingPredicate(i));
            if (firstItem != null)
            {
                if (!siblingPredicate(firstItem))
                    System.Diagnostics.Debug.Assert(false, string.Format(CultureInfo.CurrentCulture, "Item '{0}' match firstItemPredicate but doesn't match siblingPredicate.", firstItem)); //could not be reached!
                else
                {
                    yield return firstItem;
                    for (int i = Items.IndexOf(firstItem) + 1; i < Items.Count; i++)
                    {
                        TEntity item = this.Items[i];
                        if (!siblingPredicate(item))
                            yield break;
                        else
                            yield return item;
                    }
                }
            }
            else
                yield break;
        }

        private static bool EntityKeyEqual(TEntity e1, TEntity e2, Func<TEntity, object> keyExtractor)
        {
            return object.Equals(keyExtractor(e1), keyExtractor(e2));
        }

        private void AddEntityBeforeRef(TEntity entity, TEntity entityToMoveBefore)
        {
            Guard.ArgumentNotNull(entity, "entity");

            //add
            Add(entity);

            //move
            MoveOrMoveToEnd(entity, entityToMoveBefore);
        }

        /// <summary>
        /// Merges the <paramref name="data"/> into given <paramref name="subset"/> (part of this list).
        /// <para>New items from <paramref name="data"/> are added into this list</para>
        /// 	<para>items that are in <paramref name="subset"/> but are missing in data are <see cref="TikEntityBase.MarkDeleted"/>.</para>
        /// 	<para>Items with the same key (<paramref name="keyExtractor"/>) are updated by <paramref name="updateDataAction"/>.</para>
        /// 	<example>
        /// //update mikrotik router to state in database
        /// listInMikrotik = LoadListFromMikrotik();
        /// listInDb = LoadListFromMikrotik();
        /// //merge the whole list
        /// listInMikrotik.MergeSubset(listInMikrotik, listInDb, null, i =&gt; i.Id, (dst, src) =&gt; { dst.Name = src.Name; dst.Priority = src.Priority; } );
        /// listInMikrotik.Save();
        /// </example>
        /// </summary>
        /// <param name="subset">The subset in this list (the same filter as in <paramref name="data"/>).</param>
        /// <param name="data">The data to be metged into this list.</param>
        /// <param name="entityToInsertBeforeFallback">Item are inserted BEFORE this item, if <paramref name="subset"/> is empty (all items from <paramref name="data"/> should be inserted. If is null than items are added at the end of the list (if <paramref name="subset"/> is empty).</param>
        /// <param name="keyExtractor">The key extractor - should return key from given entity (not .id property) - items with the same key are treated as the same instances.</param>
        /// <param name="updateDataAction">The update data action - called to assign entity data from <paramref name="data"/> item into <paramref name="subset"/> item. (dst,src)</param>
        /// <remarks>Methods does take care about order of items and performs <see cref="Move"/> methods to reorder <paramref name="subset"/> to the same order as are in <paramref name="data"/>.</remarks>
        /// <seealso cref="Merge"/>
        public void MergeSubset(IEnumerable<TEntity> subset, IEnumerable<TEntity> data, TEntity entityToInsertBeforeFallback,
            Func<TEntity, object> keyExtractor, Action<TEntity, TEntity> updateDataAction)
        {
            Guard.ArgumentNotNull(subset, "subset");
            Guard.ArgumentNotNull(data, "data");
            Guard.ArgumentNotNull(keyExtractor, "keyExtractor");
            Guard.ArgumentNotNull(updateDataAction, "updateDataAction");

            List<TEntity> subsetList = subset.ToList();
            List<TEntity> dataList = data.ToList();

            //entity to insert before
            TEntity positionRefEntity;
            if (subsetList.Count <= 0)
            {
                positionRefEntity = entityToInsertBeforeFallback;
                //if ((Items.Count <= 0) || addToEndIfRefPointNotFound)
                //    positionRefEntity = null;
                //else
                //    positionRefEntity = Items[0];
            }
            else
            {
                int firstItemAfterSubsetIdx = Items.IndexOf(subsetList[subsetList.Count - 1]);
                if (firstItemAfterSubsetIdx + 1 < Items.Count)
                    positionRefEntity = Items[firstItemAfterSubsetIdx + 1];
                else
                    positionRefEntity = null; //add to end
            }

            //merge
            if (subsetList.Count == 0)
            {
                for (int i = dataList.Count - 1; i >= 0; i--)
                {
                    AddEntityBeforeRef(dataList[i], positionRefEntity);
                    positionRefEntity = dataList[i];
                }
            }
            else
            {
                foreach (TEntity subsetEntity in subsetList)
                    subsetEntity.MarkDeleted(); //will be cleared if found in dataList

                for (int i = dataList.Count - 1; i >= 0; i--)
                {
                    TEntity dataEntity = dataList[i];
                    TEntity subsetEntity;
                    if ((i < subsetList.Count) && EntityKeyEqual(dataEntity, subsetList[i], keyExtractor))
                    {
                        subsetEntity = subsetList[i]; //the same on the same position
                        subsetEntity.MarkClear();
                    }
                    else
                    {
                        //Try to find
                        subsetEntity = subsetList.FirstOrDefault(e => EntityKeyEqual(e, dataEntity, keyExtractor) /*object.Equals(keyExtractor(e), keyExtractor(dataEntity)*/);
                        if (subsetEntity != null) //move
                        {
                            MoveOrMoveToEnd(subsetEntity, positionRefEntity);
                            subsetEntity.MarkClear();
                        }
                        else
                        {
                            //add new
                            AddEntityBeforeRef(dataEntity, positionRefEntity);
                            subsetEntity = dataEntity;
                        }
                    }

                    updateDataAction(subsetEntity, dataEntity);

                    positionRefEntity = subsetEntity;
                }
            }
        }


        /// <summary>
        /// Merges the <paramref name="data"/> into this list.
        /// <para>New items from <paramref name="data"/> are added into this list</para>
        /// <para>items that are in <paramref name="subset"/> but are missing in data are <see cref="TikEntityBase.MarkDeleted"/>.</para>
        /// <para>Items with the same key (<paramref name="keyExtractor"/>) are updated by <paramref name="updateDataAction"/>.</para>
        ///	<example>
        /// //update mikrotik router to state in database
        /// listInMikrotik = LoadListFromMikrotik();
        /// listInDb = LoadListFromMikrotik();
        /// //merge the whole list
        /// listInMikrotik.Merge(listInDb, i =&gt; i.Id, (dst, src) =&gt; { dst.Name = src.Name; dst.Priority = src.Priority; } );
        /// listInMikrotik.Save();
        /// </example>
        /// </summary>
        /// <param name="data">The data to be metged into this list.</param>
        /// <param name="keyExtractor">The key extractor - should return key from given entity (not .id property) - items with the same key are treated as the same instances.</param>
        /// <param name="updateDataAction">The update data action - called to assign entity data from <paramref name="data"/> item into <paramref name="subset"/> item. (dst,src)</param>
        /// <remarks>Methods does take care about order of items and performs <see cref="Move"/> methods to reorder <paramref name="subset"/> to the same order as are in <paramref name="data"/>.</remarks>
        /// <seealso cref="MergeSubset"/>
        public void Merge(IEnumerable<TEntity> data, Func<TEntity, object> keyExtractor, Action<TEntity, TEntity> updateDataAction)
        {
            MergeSubset(this, data, null, keyExtractor, updateDataAction);
        }
    }
}
