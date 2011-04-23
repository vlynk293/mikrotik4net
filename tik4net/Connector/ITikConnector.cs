using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tik4Net.Logging;

namespace Tik4Net.Connector
{
    /// <summary>
    /// Mikrotik session. Main object to access mikrotik router.
    /// Implementation of interface depends on technology that 
    /// is used to access mikrotik (API, SSH, TELNET, ...).
    /// <example>
    /// using(ITikConnector session = new XYZSession())
    /// {
    ///     session.Open("192.168.1.1");
    ///     session.LogOn("user", "pass");
    ///     // ... do work ... //Query ...
    ///     session.LogOff();
    /// }
    /// </example>
    /// </summary>
    public interface ITikConnector
    {
        /// <summary>
        /// Gets a value indicating whether is logged on (<see cref="Open(string, int, string, string)"/>).
        /// </summary>
        /// <value><c>true</c> if is logged on; otherwise, <c>false</c>.</value>
        bool LoggedOn { get; }

        /// <summary>
        /// Sets the logger object (if all commands should be logged).
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <seealso cref="TikSession.AllowConnectorLogging"/>
        void SetLogger(ILog logger);        

        /// <summary>
        /// Creates the <see cref="ITikConnector"/> implementation specific <see cref="ITikEntityRow"/>.
        /// (Factory method).
        /// </summary>
        /// <param name="entityRowData">The entity row data.</param>
        /// <returns><see cref="ITikEntityRow"/> implementation instance specific for <see cref="ITikConnector"/> type.</returns>
        ITikEntityRow CreateEntityRow(string entityRowData);

        /// <summary>
        /// Opens connection to the specified mikrotik host on default port (depends on technology) and perform the logon operation.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <seealso cref="Close"/>
        void Open(string host, string user, string password);

        /// <summary>
        /// Opens connection to the specified mikrotik host on specified port and perform the logon operation.
        /// </summary>
        /// <param name="host">The host (name or ip).</param>
        /// <param name="port">TCPIP port.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <seealso cref="Close"/>
        void Open(string host, int port, string user, string password);

        /// <summary>
        /// Performs the logoff operation and closes connection.
        /// </summary>
        /// <seealso cref="Open(string, int, string, string)"/>
        void Close();

        ///// <summary>
        ///// Executes given <paramref name="command"/> and reads the response.
        ///// Could be used to very low-level usage of connector.
        ///// </summary>
        ///// <param name="command">The command to be executed.</param>
        ///// <returns>List of response rows.</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        //List<string> ExecuteAndReadResponse(string command);

        //List<string> ExecuteAndReadResponse(string command, bool exactlyOneRow);

        //List<string> ExecuteAndReadResponse(string command, Regex exactlyOneRowRegex);

        //List<string> ExecuteAndReadResponse(string command, bool exactlyOneRow,
        //    Regex firstRowRegex, Regex lastRowRegex, Regex otherRowRegex);

        /// <summary>
        /// Queries the data rows (rows from which data entities can be constructed).
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z API notation).</param>
        /// <returns>List of parsed data rows.</returns>
        IEnumerable<ITikEntityRow> ExecuteReader(string entityPath);

        /// <summary>
        /// Version of <see cref="ExecuteReader(string)"/> with list of properties to be read.
        /// </summary>
        IEnumerable<ITikEntityRow> ExecuteReader(string entityPath, IEnumerable<string> propertyList);

        /// <summary>
        /// Version of <see cref="ExecuteReader(string)"/> with list of properties to be read and list of proName-propValue filter pairs.
        /// </summary>
        IEnumerable<ITikEntityRow> ExecuteReader(string entityPath, IEnumerable<string> propertyList, TikConnectorQueryFilterDictionary filter);
        
        /// <summary>
        /// Executes creation command for entity with given values.
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z API notation).</param>
        /// <param name="values">The values (propertyName-propertyValue).</param>
        /// <returns>Id of created entity.</returns>
        string ExecuteCreate(string entityPath, Dictionary<string, string> values);

        /// <summary>
        /// Executes update command for entity with given values (set values).
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z API notation).</param>
        /// <param name="id">The id of entity to be updated.</param>
        /// <param name="values">The values (propertyName-propertyValue) - null value means unset of value.</param>
        void ExecuteSet(string entityPath, string id, Dictionary<string, string> values);

        /// <summary>
        /// Executes update command for entity with given values (unset values).
        /// </summary>
        /// <param name="entityPath">The entity path.</param>
        /// <param name="id">The id.</param>
        /// <param name="properties">The properties to be unset.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        void ExecuteUnset(string entityPath, string id, List<string> properties);

        /// <summary>
        /// Executes delete command for entity with given values.
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z API notation).</param>
        /// <param name="id">The id of entity to be deleted.</param>
        void ExecuteDelete(string entityPath, string id);

        /// <summary>
        /// Executes move command for entitity with given <paramref name="idToMove"/>.
        /// Moves given <paramref name="idToMove"/> BEFORE entity with <paramref name="idToMoveBefore"/>.
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z API notation).</param>
        /// <param name="idToMove">The id of entity to move.</param>
        /// <param name="idToMoveBefore">
        /// The id of entity BEFORE which is <paramref name="idToMove"/> entity moved. 
        /// </param>
        /// <remarks>Make sense only for <see cref="TikListMode.Ordered"/> lists of entities.</remarks>
        /// <seealso cref="ExecuteMoveToEnd"/>
        void ExecuteMove(string entityPath, string idToMove, string idToMoveBefore);

        /// <summary>
        /// The same as <see cref="ExecuteMove"/> but moves given <paramref name="idToMove"/> to the end of list.
        /// </summary>
        /// <seealso cref="ExecuteMove"/>
        void ExecuteMoveToEnd(string entityPath, string idToMove);
    }

    //ROUTEROS.CLASS
    //DONE function getall($cmd, $proplist = FALSE, $args = array(), $assoc = FALSE, $callback = FALSE)
    //DONE function set($cmd, $args = array(), $callback = FALSE)
    //DONE function add($cmd, $args = array(), $callback = FALSE)
    //DONE function remove($cmd, $id, $callback = FALSE)
    //function move($cmd, $id, $before, $callback = FALSE)

    //function reboot()
    //function cancel($tag = FALSE, $callback = FALSE)
    //function fetchurl($url, $callback = FALSE)
    //function unsett($cmd, $id, $value, $callback = FALSE)
    //function scan($id, $duration="00:10:00", $callback = FALSE)
    //function freqmon($id, $duration="00:02:00", $callback = FALSE)
    //function listen($cmd, $args = FALSE, $callback)
    //function btest($address, $speed = "1M", $protocol = "tcp", $callback = FALSE)
    //function dispatch(&$continue)

}
