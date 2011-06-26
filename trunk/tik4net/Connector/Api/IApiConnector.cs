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
    public interface IApiConnector: ITikConnector, ILogConnector
    {
        /// <summary>
        /// Executes command via API that does not return values (!done response is expected).
        /// </summary>
        /// <param name="command">The command string in API format (use \n to split rows).</param>
        void ApiExecuteNonQuery(string command);

        /// <summary>
        /// Executes command via API that does not return values (!done response is expected).
        /// </summary>
        /// <param name="command">The command string in API format (use \n to split rows).</param>
        /// <param name="parameters">The parameters (key,value).</param>
        void ApiExecuteNonQuery(string command, Dictionary<string, string> parameters);

        /// <summary>
        /// Executes reader command (API specific).
        /// </summary>
        /// <param name="command">The command (formated).</param>
        /// <returns>List of response data rows.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<ITikEntityRow> ApiExecuteReader(string command);

        /// <summary>
        /// Executes reader command (API specific).
        /// </summary>
        /// <param name="command">The command (formated).</param>
        /// <param name="parameters">The parameters (key, value).</param>
        /// <returns>List of response data rows.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<ITikEntityRow> ApiExecuteReader(string command, Dictionary<string, string> parameters);

        /// <summary>
        /// Executes the specified command and returns mikrotik response. 
        /// </summary>
        /// <param name="command">The command string in API format (use \n to split rows).</param>
        /// <returns>Command response</returns>
        /// <remarks>Doesn't convert !trap to exceptions. Doesn't perform any tasks on result.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<string> ApiExecute(string command);

        /// <summary>
        /// Executes the specified command and returns mikrotik response.
        /// </summary>
        /// <param name="command">The command string in API format (use \n to split rows).</param>
        /// <param name="parameters">The parameters (key, value).</param>
        /// <returns>Command response</returns>
        /// <remarks>Doesn't convert !trap to exceptions. Doesn't perform any tasks on result.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<string> ApiExecute(string command, Dictionary<string, string> parameters);

        /// <summary>
        /// Executes command (API specific) that returns exactly one value as response.
        /// </summary>
        /// <param name="command">The command (formated).</param>
        /// <returns>Response value.</returns>
        string ApiExecuteScalar(string command);

        /// <summary>
        /// Executes command (API specific) that returns exactly one value as response.
        /// </summary>
        /// <param name="command">The command (formated).</param>
        /// <param name="parameters">The parameters (key, value).</param>
        /// <returns>Response value.</returns>
        string ApiExecuteScalar(string command, Dictionary<string, string> parameters);
    }
}
