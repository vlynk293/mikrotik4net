using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Connector.Api
{
    /// <summary>
    /// <see cref="ITikConnector"/> special for mikrotik API with methods, 
    /// that are specific for API protocol.
    /// </summary>
    public interface IApiConnector: ITikConnector
    {
        /// <summary>
        /// Executes command via API that does not return values (!done response is expected).
        /// </summary>
        /// <param name="command">The command string in API format (use \n to split rows).</param>
        void ExecuteNonQuery(string command);

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Command response</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<string> Execute(string command);
    }
}
