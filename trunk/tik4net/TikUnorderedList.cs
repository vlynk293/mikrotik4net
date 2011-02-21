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
    }
}
