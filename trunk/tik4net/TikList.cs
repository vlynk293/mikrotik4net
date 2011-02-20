using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    public class TikList<TEntity> : TikListBase<TEntity> 
        where TEntity : TikEntityBase, new()
    {
        private readonly Dictionary<TEntity, TEntity> entityMoves = new Dictionary<TEntity, TEntity>(); //<entityToMove,entityToMoveBefore>

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
        public void Insert(TEntity entity, TEntity entityToInsertBefore)
        {
            Add(entity);

            Move(entity, entityToInsertBefore);
        }
        #endregion

        #region -- MOVE --
        public void Move(TEntity entityToMove, TEntity entityToMoveBefore)
        {
            //TODO remove moved entity from list of moves
            //TODO ensure entity not deleted, etc ...
            //TODO what about newly created entities?

            Guard.ArgumentNotNull(entityToMove, "entityToMove");
            Guard.ArgumentNotNull(entityToMoveBefore, "entityToMoveBefore");

            int entityToMoveBeforeIdx = Items.IndexOf(entityToMoveBefore);
            if (entityToMoveBeforeIdx < 0)
                throw new ArgumentException("Given entity not found in list.", "entityToMoveBefore");

            //move in list
            Items.Remove(entityToMove);
            Items.Insert(entityToMoveBeforeIdx, entityToMove);
            //remember move
            entityMoves.Add(entityToMove, entityToMoveBefore);
        }

        public void MoveToEnd(TEntity entityToMove)
        {
            Guard.ArgumentNotNull(entityToMove, "entityToMove");
            Items.Remove(entityToMove);
            Items.Add(entityToMove);

            entityMoves.Add(entityToMove, null);
        }

        protected override void Clear()
        {
            base.Clear();
            entityMoves.Clear();
        }

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
                    throw new NotImplementedException();//TODO session.Connector.ExecuteMoveToEnd(metadata.EntityPath, pair.Key.Id);
            }
            entityMoves.Clear();
        }
        #endregion
    }
}
