using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    public abstract class TikSingleRowList<TEntity>: TikListBase<TEntity> 
        where TEntity : TikEntityBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TikSingleRowList&lt;TEntity&gt;"/> class.
        /// Default active session (<see cref="TikSession.ActiveSession"/> is used).
        /// </summary>
        protected TikSingleRowList() 
            : base()
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSingleRowList&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="session">The session used to access mikrotik.</param>
        protected TikSingleRowList(TikSession session)
            : base(session)
        {
        }

        protected override void VerifyResponseRows(IEnumerable<ITikEntityRow> response)
        {
            base.VerifyResponseRows(response);

            if (response.Count() > 1)
                throw new InvalidOperationException(string.Format("There is not allowed more than row in result for {0}. Result contains {1} rows.", this, response.Count()));
        }

        protected override void BeforeAdd(TEntity entity)
        {
            base.BeforeAdd(entity);

            if (Items.Count > 0)
                throw new InvalidOperationException(string.Format("Can not add second row to SingleRowList '{0}'.", this));
        }
    }
}
