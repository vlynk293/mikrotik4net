using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net
{
    /// <summary>
    /// Describes query filter - list of propertyName-propertyValue pairs.
    /// Operator AND is used between items.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    public class TikConnectorQueryFilterDictionary: Dictionary<string, string>
    {
    }
}
