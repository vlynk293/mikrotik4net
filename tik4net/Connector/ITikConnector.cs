using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        List<string> ExecuteAndReadResponse(string command);

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

        ITikEntityRow CreateEntityRow(string entityRowData);
        /// <summary>
        /// Queries the data rows (rows from which data entities can be constructed).
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z notation).</param>
        /// <returns>List of parsed data rows.</returns>
        IEnumerable<ITikEntityRow> QueryDataRows(string entityPath);

        IEnumerable<ITikEntityRow> QueryDataRows(string entityPath, IEnumerable<string> propertyList);

        IEnumerable<ITikEntityRow> QueryDataRows(string entityPath, IEnumerable<string> propertyList, IEnumerable<KeyValuePair<string, string>> filter);

        ///// <summary>
        ///// Similar to <see cref="QueryDataRows"/> but constructs list of <typeparamref name="TEntity"/> 
        ///// instances from data rows obtained by <see cref="QueryDataRows"/> call.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity (must be <see cref="ITikReadableEntity"/> and have to contains empty constructor).</typeparam>
        ///// <returns>List of <typeparamref name="TEntity"/> instances from obtained data rows.</returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        //IEnumerable<TEntity> Query<TEntity>() 
        //    where TEntity: ITikReadableEntity, new();
        
        //List<IEntityRow> Query(string entity, Dictionary<string, string> filter)
        //    where TEntity: IReadableEntity, new(); //TODO

        string ExecuteCreate(string entityPath, Dictionary<string, string> values);

        void ExecuteDelete(string entityPath, string id);

        void ExecuteUpdate(string entityPath, string id, Dictionary<string, string> values);

        void ExecuteMove(string entityPath, string idToMove, string idToMoveBefore);
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
