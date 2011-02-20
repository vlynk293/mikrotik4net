using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
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
    }
}
