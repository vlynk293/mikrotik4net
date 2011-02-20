using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Mikrotik entity (<see cref="ITikEntity"/>) list.
    /// </summary>
    public interface ITikList
    {
        ///// <summary>
        ///// Gets the entity path (in API format - /ip/firewall/address-list) to access entity in list.
        ///// Is specific for each entity type.
        ///// </summary>
        ///// <value>The entity path.</value>
        //string EntityPath { get; }

        ///// <summary>
        ///// Gets the all properties of inner object.
        ///// </summary>
        ///// <returns></returns>
        //Dictionary<string, TikPropertyAttribute> GetTikProperties();

        ///// <summary>
        ///// Adds the new item into list and fills it from given <paramref name="entityRow"/>.
        ///// </summary>
        ///// <param name="entityRow">The entity data row.</param>
        ///// <returns>Newly created and added instace of inner item.</returns>
        //ITikEntity AddNew(ITikEntityRow entityRow);
    }
}
