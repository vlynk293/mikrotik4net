using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections;

namespace Tik4Net
{
    /// <summary>
    /// <see cref="TikListBase{TEntity}"/> type that contains maximally one row.
    /// </summary>
    /// <seealso cref="TikListMode.SingleRow"/>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
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

        /// <summary>
        /// See <see cref="TikListBase{TEntity}.VerifyResponseRows"/> for details.
        /// Ensures that <paramref name="response"/> contains maximally one row.
        /// </summary>
        protected override void VerifyResponseRows(IEnumerable<ITikEntityRow> response)
        {
            base.VerifyResponseRows(response);

            if (response.Count() > 1)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "There is not allowed more than row in result for {0}. Result contains {1} rows.", this, response.Count()));
        }

        /// <summary>
        /// See <see cref="TikListBase{TEntity}.BeforeAdd"/> for details.
        /// Ensures that list contains maximally one row (must be empty before call).
        /// </summary>
        protected override void BeforeAdd(TEntity entity)
        {
            base.BeforeAdd(entity);

            if (Items.Count > 0)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Can not add second row to SingleRowList '{0}'.", this));
        }

        /// <summary>
        /// See <see cref="IList.IsReadOnly"/> for details. Returns false.
        /// </summary>
        /// <value>Always false.</value>
        public override bool IsReadOnly
        {            
            get { return false; }
        }

        /// <summary>
        /// See <see cref="IList.IsFixedSize"/> for details. Returns true.
        /// </summary>
        /// <value>Always true.</value>
        public override bool IsFixedSize
        {
            get { return true; }
        }

        /// <summary>
        /// See <see cref="TikListBase{TEntity}.OnInsert"/> for details.
        /// Calls <see cref="TikListBase{TEntity}.BeforeAdd"/> and later <see cref="TikListBase{TEntity}.Add(TEntity)"/>.
        /// </summary>
        protected override void OnInsert(int index, TEntity item)
        {
            BeforeAdd(item);
            Add(item);
        }
    }
}
