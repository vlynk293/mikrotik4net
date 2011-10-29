using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Exposed by all <see cref="ITikList"/> classes
    /// that support save access. List entity have to implement 
    /// <see cref="ITikEntityWithId"/> in this case.
    /// </summary>
    /// <seealso cref="ITikEntityWithId"/>
    public interface IEditableTikList: ITikList
    {
        /// <summary>
        /// Saves this instance - saves all entities tha are in <see cref="TikEntityBase.IsModified"/>, 
        /// <see cref="TikEntityBase.IsMarkedDeleted"/> and <see cref="TikEntityBase.IsMarkedNew"/> states.
        /// Uses session from constructor.
        /// </summary>
        void Save();
    }
}
