using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tik4Net.Connector;

namespace Tik4Net
{
    public sealed class TikSession: IDisposable
    {
        [ThreadStatic]
        private static Stack<TikSession> activeSessions = new Stack<TikSession>();

        private readonly ITikConnector connector;
        private readonly TikConnectorType connectorType;

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

        public ITikConnector Connector
        {
            get { return connector; }
        }

        public TikConnectorType ConnectorType
        {
            get { return connectorType; }
        }

        public TikSession(TikConnectorType connectorType)
            : this(null, connectorType)
        {
        }

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
                        throw new NotImplementedException(string.Format("Not supported TikConnectorType '{0}'.", connectorType));
                }
            }

            this.connectorType = connectorType;

            activeSessions.Push(this);
        }

        public void Open(string host, string user, string password)
        {
            connector.Open(host, user, password);
        }

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
