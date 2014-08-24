using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tik4Net.Session
{
    /// <summary>
    /// Mikrotik session. Main object to access mikrotik router.
    /// Implementation of interface depends on technology that 
    /// is used to access mikrotik (API, SSH, TELNET, ...).
    /// <example>
    /// using(ITikSession session = new XYZSession())
    /// {
    ///     session.Open("192.168.1.1");
    ///     session.LogOn("user", "pass");
    ///     // ... do work ... //Query ...
    ///     session.LogOff();
    /// }
    /// </example>
    /// </summary>
    public interface ITikSession: IDisposable
    {
        /// <summary>
        /// Opens connection to the specified mikrotik host on default port (depends on technology).
        /// </summary>
        /// <param name="host">The host.</param>
        void Open(string host);

        /// <summary>
        /// Opens connection to the specified mikrotik host on specified port.
        /// </summary>
        /// <param name="host">The host (name or ip).</param>
        /// <param name="port">TCPIP port.</param>
        void Open(string host, int port);

        /// <summary>
        /// Perform the logon operation (call <see cref="Open(string, int)"/> before).
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        void LogOn(string user, string password);


        /// <summary>
        /// Performs the logoff operation.
        /// </summary>
        void LogOff();

        /// <summary>
        /// Queries the data rows (rows from which data entities can be constructed).
        /// </summary>
        /// <param name="entityPath">The entity (in x/y/z notation).</param>
        /// <returns>List of parsed data rows.</returns>
        IEnumerable<ITikEntityRow> QueryDataRows(string entityPath);

        /// <summary>
        /// Similar to <see cref="QueryDataRows"/> but constructs list of <typeparamref name="TEntity"/> 
        /// instances from data rows obtained by <see cref="QueryDataRows"/> call.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (must be <see cref="ITikReadableEntity"/> and have to contains empty constructor).</typeparam>
        /// <returns>List of <typeparamref name="TEntity"/> instances from obtained data rows.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        IEnumerable<TEntity> Query<TEntity>() 
            where TEntity: ITikReadableEntity, new();
        
        //List<IEntityRow> Query(string entity, Dictionary<string, string> filter)
        //    where TEntity: IReadableEntity, new(); //TODO
    }
}
