using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;

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
        /// Moves the specified <paramref name="entityToMove"/> before another <paramref name="entityToMoveBefore"/>.
        /// <para>
        /// Entity is moved in internal list of items and info about move is stored in internal list of moves 
        /// to enable perform this move during <see cref="TikListBase{TEntity}.Save"/> process.
        /// </para>
        /// </summary>
        /// <param name="entityToMove">The entity to move.</param>
        /// <param name="entityToMoveBefore">The entity before which is given <paramref name="entityToMove"/> moved.</param>
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

        /// <summary>
        /// The same as <see cref="Move"/> but moves given <paramref name="entityToMove"/> to the end of list.
        /// </summary>        
        public void MoveToEnd(TEntity entityToMove)
        {
            Guard.ArgumentNotNull(entityToMove, "entityToMove");
            Items.Remove(entityToMove);
            Items.Add(entityToMove);

            entityMoves.Add(entityToMove, null);
        }

        /// <summary>
        /// See <see cref="TikListBase{TEntity}.Clear"/>. 
        /// Additionaly clears list of moves.
        /// </summary>
        protected override void Clear()
        {
            base.Clear();
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
                    throw new NotImplementedException();//TODO session.Connector.ExecuteMoveToEnd(metadata.EntityPath, pair.Key.Id);
            }
            entityMoves.Clear();
        }
        #endregion
    }
}
