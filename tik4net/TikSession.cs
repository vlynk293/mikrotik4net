using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;
using System.Globalization;

namespace Tik4Net
{
    /// <summary>
    /// Represents main object to connect mikrotik router.
    /// Uses its <see cref="Connector"/> to do this job. You could use <see cref="Connector"/>
    /// property do perform low-level access to mikrotik router.
    /// <para>
    /// You can either use predefined connector type (<see cref="TikConnectorType"/>)
    /// or provide your custom instance of connector that implements <see cref="ITikConnector"/>
    /// interface.
    /// </para>
    /// <para>
    /// <see cref="ActiveSession"/> thread static field could be used to 
    /// access most inner active (instanced) session. This property is used
    /// by tikObject constructors if you don't provide session instance.
    /// </para>
    /// <para>Open connection via <see cref="Open(string,string,string)"/> method call
    /// before its use.</para>
    /// <para>Use <see cref="TikSession"/> in <see cref="IDisposable"/> pattern
    /// to ensure proper logoff.</para>
    /// <example>
    /// using(TikSession session = new TikSession(TikConnectorType.Api))
    /// {
    ///   session.Open("192.168.88.1", "admin", "");
    ///   
    ///   QueueTreeList qtl = new QueueTreeList();
    ///   gtl.LoadAll();
    ///   foreach(QueueTree qt in qtl)
    ///   {
    ///     Console.WriteLine(qt.Name);
    ///   }
    /// }
    /// </example>
    /// </summary>
    public sealed class TikSession: IDisposable
    {
        [ThreadStatic]
        private static Stack<TikSession> activeSessions = new Stack<TikSession>();

        private readonly ITikConnector connector;
        private readonly TikConnectorType connectorType;

        /// <summary>
        /// Gets the active session (lastly created instance of <see cref="TikSession"/>) in current thread.
        /// </summary>
        /// <value>The active session or null (if no session is active).</value>
        public static TikSession ActiveSession
        {
            get
            {
                if (activeSessions.Count > 0)
                    return activeSessions.Peek();
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="ITikConnector"/> used to access mikrotik router.
        /// Use <see cref="ITikConnector">Connector</see> to perform low-level 
        /// operations on mikrotik router.
        /// </summary>
        /// <value>The connector to mikrotik router used by <see cref="TikSession"/> instance.</value>
        public ITikConnector Connector
        {
            get { return connector; }
        }

        /// <summary>
        /// Gets the type of the <see cref="Connector"/> - predefined type or  <see cref="TikConnectorType.Custom"/>.
        /// </summary>
        /// <value>The type of the connector.</value>
        public TikConnectorType ConnectorType
        {
            get { return connectorType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSession"/> class with one of
        /// predefined <see cref="TikConnectorType"/> <see cref="Connector"/> types.
        /// </summary>
        /// <param name="connectorType">Type of the connector.</param>
        public TikSession(TikConnectorType connectorType)
            : this(null, connectorType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TikSession"/> class with <see cref="TikConnectorType.Custom"/>
        /// <see cref="ConnectorType"/> and custom instance of <see cref="Connector"/>.
        /// </summary>
        public TikSession(ITikConnector connector)
            :this(connector, TikConnectorType.Custom)
        {            
        }

        private TikSession(ITikConnector connector, TikConnectorType connectorType)
        {
            if ((connectorType == TikConnectorType.Custom))
            {
                if (connector == null)
                    throw new ArgumentException("Use constructor with connector instance for custom connectors.", "connectorType");
                else
                    this.connector = connector;
            }
            else
            {
                switch (connectorType)
                {
                    case TikConnectorType.Api:
                        this.connector = new Tik4Net.Connector.Api.ApiConnector();
                        break;
                    case TikConnectorType.Ssh:
                        throw new NotImplementedException();
                    case TikConnectorType.Telnet:
                        throw new NotImplementedException();
                    default:
                        throw new NotImplementedException(string.Format(CultureInfo.CurrentCulture, "Not supported TikConnectorType '{0}'.", connectorType));
                }
            }

            this.connectorType = connectorType;

            activeSessions.Push(this);
        }
        /// <summary>
        /// See <see cref="Open(string, int, string, string)"/>. 
        /// Uses default port for <see cref="Connector"/> type.
        /// </summary>
        public void Open(string host, string user, string password)
        {
            connector.Open(host, user, password);
        }

        /// <summary>
        /// Opens session to the the specified host.
        /// </summary>
        /// <param name="host">The host (IP).</param>
        /// <param name="port">The port.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public void Open(string host, int port, string user, string password)
        {
            connector.Open(host, port, user, password);
        }

        //#region -- SAVE --
        //public void Save<TList>(TList list)
        //    where TList : ITikList, new()     
        //{
        //    //TODO VERIFY - http://www.ispforum.cz/viewtopic.php?f=3&t=4514
        //    SaveInternal<TList>(list);
        //}        

        //private void SaveInternal<TList>(TList list)
        //      where TList : ITikList, new()
        //{
        //    //TODO verify the same filter!!!
        //    TList refList = FetchAll<TList>();

        //    ListMergeHelper<ITikEntity> mergeHelper = new ListMergeHelper<ITikEntity>(
        //        SaveInsertItem, SaveUpdateItem, SaveDeleteItem);
        //    mergeHelper.Merge(list.GetEnumerator(), refList.GetEnumerator());           
        //}

        //private static void SaveInsertItem(object sender, ITikEntity sourceItem)
        //{
        //    Console.WriteLine("Add {0}", sourceItem);
        //}

        //private static void SaveDeleteItem(object sender, ITikEntity destinationItem)
        //{
        //    Console.WriteLine("Delete {0}", destinationItem);
        //}

        //private static void SaveUpdateItem(object sender, ITikEntity sourceItem, ITikEntity destinationItem)
        //{
        //    Console.WriteLine("Update {0} -> {1}", destinationItem, sourceItem);
        //}

        //#endregion

        #region IDisposable Members

        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for details.
        /// Closes TCPIP connection to router if has been established by <see cref="Open(string, int, string, string)"/> call.
        /// </summary>
        public void Dispose()
        {
            activeSessions.Pop();
            connector.Close();
        }

        #endregion
    }
}
