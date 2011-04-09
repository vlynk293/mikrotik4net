using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tik4Net.Connector.Api
{
    /// <summary>
    /// <see cref="ITikConnector"/> implementation that uses Mikrotik API to access router.
    /// </summary>
    internal sealed class ApiConnector : ITikConnector, IApiConnector, IDisposable
    {
        //Inspiration:
        //  http://ayufan.eu/projects/rosapi/repository/entry/trunk/routeros.class.php
        private const int API_DEFAULT_PORT = 8728;
        private static Regex trapRegex = new Regex(@"^!trap\s*(?<ERROR>.*)$");
        private static Regex fatalRegex = new Regex(@"^!fatal\s*(?<ERROR>.*)$");
        private static Regex logonResponseRegex = new Regex(@"^!done\n=ret=(?<HASH>[0-9a-f]+)$", RegexOptions.Singleline);
        private static Regex doneRegex = new Regex(@"^!done$");
        private static Regex dataRowRegex = new Regex(@"^!re");
        private static Regex createResponseRegex = new Regex(@"^!done\s*=ret=(?<ID>.+)$");
        private static Regex closeResponseRegex = new Regex(@"^!fatal\s*session terminated on request");
        private TcpClient connection;
        private NetworkStream connectionStream;
        private bool loggedOn = false;

        #region ITikConnector Members

        public bool LoggedOn
        {
            get { return loggedOn; }
        }

        public void Open(string host, string user, string password)
        {
            Open(host, API_DEFAULT_PORT, user, password);
        }

        public void Open(string host, int port, string user, string password)
        {
            if (loggedOn)
                throw new TikConnectorException("Already logged in");
            //open connection
            connection = new TcpClient();
            connection.Connect(host, port); 
            connectionStream = connection.GetStream();                        

            //logon
            string response = ExecuteAndReadResponse("/login", null, logonResponseRegex);
            string hashedPass = ApiConnectorHelper.EncodePassword(password, logonResponseRegex.Match(response).Groups["HASH"].Value); //performace - logonResponseRegex is validated twice
            ExecuteNonQuery("/login", 
                new Dictionary<string, string> 
                {
                    {"=name", user},
                    {"=response", "00" + hashedPass}
                }); 

            loggedOn = true;
        }

        public void Close()
        {
            //logoff
            if (loggedOn)
            {
                string command = "/quit"; //Does not work in 3.x (see links)
                WriteCommand(command); 
                List<string> response = ReadResponse();
                if ((response.Count != 1) || (!closeResponseRegex.IsMatch(response[0])))
                    ValidateResponseRows(command, response, true, closeResponseRegex, null, null);
                
                //!fatalsession terminated on request
                //http://forum.mikrotik.com/viewtopic.php?f=9&t=37075
                //http://forum.mikrotik.com/viewtopic.php?f=9&t=44264
            }

            //close TCPIP
            if (connectionStream != null)
                connectionStream.Close();
            if (connection != null)
                connection.Close();

            loggedOn = false;
        }

        public ITikEntityRow CreateEntityRow(string entityRowData)
        {
            return new ApiEntityRow(entityRowData);
        }

        private List<string> ExecuteAndReadResponse(string command, Dictionary<string, string> parameters,
            bool exactlyOneRow, Regex firstRowRegex, Regex lastRowRegex, Regex otherRowRegex)
        {
            Guard.ArgumentNotNullOrEmptyString(command, "command");

            if ((parameters != null) && (parameters.Count > 0))
                command += "\n" + string.Join("\n",  parameters.Select(p => p.Key + "=" + p.Value).ToArray());

            WriteCommand(command);
            List<string> result = ReadResponse();

            ValidateResponseRows(command, result, exactlyOneRow, firstRowRegex, lastRowRegex, otherRowRegex);
            return result;
        }

        private string ExecuteAndReadResponse(string command, Dictionary<string, string> parameters, Regex exactlyOneRowRegex)
        {
            return ExecuteAndReadResponse(command, parameters, true, exactlyOneRowRegex, null, null)[0];
        }

        //For future use
        //private List<string> ExecuteAndReadResponse(string command, Dictionary<string, string> parameters, bool exactlyOneRow)
        //{
        //    return ExecuteAndReadResponse(command, parameters, exactlyOneRow, null, null, null);
        //}

        //For future use
        //private List<string> ExecuteAndReadResponse(string command, Dictionary<string, string> parameters)
        //{
        //    return ExecuteAndReadResponse(command, parameters, false, null, null, null);
        //}

        private void ExecuteNonQuery(string command, Dictionary<string, string> parameters)
        {
            ExecuteAndReadResponse(command, parameters, true, doneRegex, null, null);
        }

        private static void ValidateResponseRows(string command, List<string> response, bool exactlyOneRow,
            Regex firstRowRegex, Regex lastRowRegex, Regex otherRowRegex)
        {
            //!trap -> exception  
            List<string> errors = new List<string>();
            foreach (string row in response)
            {
                Match trapMatch = trapRegex.Match(row);
                Match fatalMatch = fatalRegex.Match(row);
                if (fatalMatch.Success)
                    errors.Insert(0, string.Format(CultureInfo.CurrentCulture, "FATAL: {0}", fatalMatch.Groups["ERROR"].Value));
                else if (trapMatch.Success)
                    errors.Add(trapMatch.Groups["ERROR"].Value);                
            }
            if (errors.Count > 0)
                throw new TikConnectorException("Target returns error.", command, errors, response); 

            //verify response row count
            if (exactlyOneRow && (response.Count != 1))
                throw new TikConnectorException("Target doesn't return exactly 1 row.", command, response);

            //verify response rows
            if (firstRowRegex != null)
            {
                if (response.Count == 0)
                    throw new TikConnectorException("Target returns empty result.", command);
                else if (!firstRowRegex.IsMatch(response[0]))
                    throw new TikConnectorException("First row in result doesn't match given format.", command, firstRowRegex, response);
            }

            if (lastRowRegex != null)
            {
                if (response.Count == 0)
                    throw new TikConnectorException("Target returns empty result.", command);
                else if (((response.Count > 1) || (firstRowRegex == null)) 
                    && (!lastRowRegex.IsMatch(response[response.Count - 1]))) //do not validate if exactly 1 row is returned and was verified by firstRowRegex
                    throw new TikConnectorException("Last row in result doesn't match given format.", command, lastRowRegex, response);
            }

            if (otherRowRegex != null)
            {
                int startIdx = firstRowRegex == null ? 0 : 1;
                int endIdx = lastRowRegex == null ? response.Count - 1 : response.Count - 2;
                for (int i = startIdx; i <= endIdx; i++)
                {
                    if (!otherRowRegex.IsMatch(response[i]))
                        throw new TikConnectorException(string.Format(CultureInfo.CurrentCulture, "{0}. response row in result doesn't match given format.", i)
                            , command, otherRowRegex, response);
                }
            }               
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteReader(string)"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> ExecuteReader(string entityPath)
        {
            return ExecuteReader(entityPath, null);
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteReader(string,IEnumerable{string})"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> ExecuteReader(string entityPath, IEnumerable<string> propertyList)
        {
            //Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            //EnsureLoggedOn();

            //List<ITikEntityRow> result = ExecuteReaderInternal(string.Format(CultureInfo.InvariantCulture, "{0}/print", entityPath), 
            //    propertyList, null, new Dictionary<string, string>{ { "=detail", "" } });

            //return result;
            return ExecuteReader(entityPath, propertyList, null);
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteReader(string,IEnumerable{string},TikConnectorQueryFilterDictionary)"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> ExecuteReader(string entityPath, IEnumerable<string> propertyList, TikConnectorQueryFilterDictionary filter)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            //Guard.ArgumentNotNull(filter, "filter");
            EnsureLoggedOn();

            List<ITikEntityRow> result = ExecuteReaderInternal(string.Format(CultureInfo.InvariantCulture, "{0}/print", entityPath), 
                propertyList, filter, new Dictionary<string, string>{ { "=detail", "" } });

            return result;
        }

        private List<ITikEntityRow> ExecuteReaderInternal(string command, IEnumerable<string> propertyList, 
            TikConnectorQueryFilterDictionary filter, Dictionary<string, string> parameters)
        {
            // ip/address/print
            // =detail=
            // =.proplist=..,..,..,..
            // ?address=10.10.10.10

            Dictionary<string, string> allParameters;
            //parameters
            if (parameters != null)
                allParameters = new Dictionary<string,string>(parameters);
            else
                allParameters = new Dictionary<string, string>(); 

            //propList
            if ((propertyList != null) && propertyList.Any()) //.proplist  (if specified)
                allParameters.Add("=.proplist", string.Join(",", propertyList.ToArray()));

            //filter
            if (filter != null) //filter (if specified)
            {
                foreach(KeyValuePair<string, string> fltPair in filter)
                {
                    allParameters.Add("?" + fltPair.Key, fltPair.Value); //TODO - convert from internal expression!!!
                }
            }

            List<string> response = ExecuteAndReadResponse(command, allParameters, false, null, doneRegex, dataRowRegex);

            List<ApiEntityRow> result = new List<ApiEntityRow>(response.Count - 1);
            for (int i = 0; i < response.Count - 1; i++)
            {
                result.Add(new ApiEntityRow(response[i]));
            }

            return result.Cast<ITikEntityRow>().ToList();
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteCreate"/> for details.
        /// </summary>
        public string ExecuteCreate(string entityPath, Dictionary<string, string> values)
        {
            //ip/address/add
            //=address=192.168.88.1/24
            //>!done
            //>=ret=...ID...
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNull(values, "values");

            if (values.Count <= 0)
                throw new ArgumentException("No specified values in ExecuteCreate.", "values");

            Dictionary<string, string> parameters = new Dictionary<string, string>(values.Count);
            foreach (KeyValuePair<string, string> valPair in values) //values
                parameters.Add("=" + valPair.Key, valPair.Value);

            string command = entityPath + "/add";
            string result = ExecuteAndReadResponse(command, parameters, createResponseRegex);

            return createResponseRegex.Match(result).Groups["ID"].Value; //performace - createResponseRegex is validated twice
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteSet"/> for details.
        /// </summary>
        public void ExecuteSet(string entityPath, string id, Dictionary<string, string> values)
        {
            //ip/address/set
            //=.id=...ID...
            //=address=10.10.10.10
            //>!done
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(id, "id");
            Guard.ArgumentNotNull(values, "values");

            if (values.Count <= 0)
                throw new ArgumentException("No specified values in ExecuteSet.", "values");

            string command = entityPath + "/set";
            Dictionary<string, string> parameters = new Dictionary<string,string>(values.Count);
            parameters.Add("=.id", id); //id            
            foreach (KeyValuePair<string, string> valPair in values) //values
                parameters.Add("=" + valPair.Key, valPair.Value);

            ExecuteNonQuery(command, parameters);
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteUnset"/> for details.
        /// </summary>
        public void ExecuteUnset(string entityPath, string id, List<string> properties)
        {
            //ip/address/unset
            //=.id=...ID...
            //=address=
            //>!done
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(id, "id");
            Guard.ArgumentNotNull(properties, "properties");

            if (properties.Count <= 0)
                throw new ArgumentException("No specified properties in ExecuteUnset.", "properties");

            string command = entityPath + "/unset";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("=.id", id); //id            
            foreach (string propName in properties) //values
                parameters.Add("=" + propName, "");

            ExecuteNonQuery(command, parameters);
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteDelete"/> for details.
        /// </summary>
        public void ExecuteDelete(string entityPath, string id)
        {
            //ip/address/remove
            //=.id=...ID...
            //>!done
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(id, "id");

            string command = entityPath + "/remove";
            Dictionary<string, string> parameters = new Dictionary<string, string> 
                { 
                    { "=.id", id } 
                };

            ExecuteNonQuery(command, parameters);
        }

        /// <summary>
        /// See <see cref="ITikConnector.ExecuteMove"/> for details.
        /// </summary>
        public void ExecuteMove(string entityPath, string idToMove, string idToMoveBefore)
        {
            //this will move queue witdh ID before queue with ID2 - http://forum.mikrotik.com/viewtopic.php?f=9&t=29390
            // /queue/simple/move
            // =numbers=...ID...
            // =destination=...ID2...
            // >!done
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(idToMove, "idToMove");
            Guard.ArgumentNotNullOrEmptyString(idToMoveBefore, "idToMoveBefore");

            string command = entityPath + "/move"; 
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "=numbers", idToMove },
                    { "=destination", idToMoveBefore }
                };

            ExecuteNonQuery(command, parameters);
        }

        public void ExecuteMoveToEnd(string entityPath, string idToMove)
        {

            //this will move queue witdh ID to the end - http://forum.mikrotik.com/viewtopic.php?f=9&t=29390
            // /queue/simple/move
            // =.id=...ID...
            // >!done
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(idToMove, "idToMove");

            string command = entityPath + "/move";
            Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { ".id", idToMove },
                };

            ExecuteNonQuery(command, parameters);
        }

        #endregion

        #region IApiConnector Members

        public void ApiExecuteNonQuery(string command)
        {
            ExecuteNonQuery(command, null);
        }

        public void ApiExecuteNonQuery(string command, Dictionary<string, string> parameters)
        {
            Guard.ArgumentNotNull(parameters, "parameters");

            var tmp = parameters.Select(p=> new KeyValuePair<string, string>("=" + p.Key, p.Value));
            ExecuteNonQuery(command, tmp.ToDictionary(t=>t.Key, t=>t.Value));
        }

        public List<string> ApiExecute(string command)
        {
            WriteCommand(command);
            List<string> result = ReadResponse();
            return result;
            //return ExecuteAndReadResponse(command, null, false, null, null, null); - we don't need converting !trap to exception
        }

        public List<string> ApiExecute(string command, Dictionary<string, string> parameters)
        {
            string cmd = command;
            foreach (KeyValuePair<string, string> param in parameters)
            {
                cmd += "\n" + "=" + param.Key + "=" + param.Value;
            }

            return ApiExecute(cmd);
        }

        public List<ITikEntityRow> ApiExecuteReader(string command)
        {
            Guard.ArgumentNotNullOrEmptyString(command, "command");

            return ExecuteReaderInternal(command, null, null, null);
        }

        public List<ITikEntityRow> ApiExecuteReader(string command, Dictionary<string, string> parameters)
        {
            Guard.ArgumentNotNullOrEmptyString(command, "command");
            Guard.ArgumentNotNull(parameters, "parameters");

            var tmp = parameters.Select(p => new KeyValuePair<string, string>("=" + p.Key, p.Value));
            return ExecuteReaderInternal(command, null, null, tmp.ToDictionary(t => t.Key, t => t.Value));
        }
        #endregion

        #region ILogConnector Members

        public void Log(string message, Tik4Net.Objects.LogLevel level)
        {
            Dictionary<string, string> parameters = new Dictionary<string,string>()
                {
                    { "=message", message }
                };
            string command = "/log/" + ApiConnectorHelper.LogLevelToCommandSufix(level);
            ExecuteNonQuery(command, parameters);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Calls <see cref="Close"/>.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion

        private void WriteCommand(string command)
        {
            string[] lines = command.Split('\n');
            foreach (string line in lines)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(line.ToCharArray());
                byte[] length = ApiConnectorHelper.EncodeLength(bytes.Length);

                connectionStream.Write(length, 0, length.Length);
                connectionStream.Write(bytes, 0, bytes.Length);
            }

            connectionStream.WriteByte(0); //final byte
        }

        private List<string> ReadResponse()
        {
            List<string> output = new List<string>();
            string o = "";
            byte[] tmp = new byte[4];
            long count;
            while (true)
            {
                tmp[3] = (byte)connectionStream.ReadByte();
                //if(tmp[3] == 220) tmp[3] = (byte)connection.ReadByte(); it sometimes happend to me that 
                //mikrotik send 220 as some kind of "bonus" between words, this fixed things, not sure about it though
                if (tmp[3] == 0)
                {
                    output.Add(o.TrimEnd('\n'));                
                    if (o.StartsWith("!done", StringComparison.Ordinal))//(o.Substring(0, 5) == "!done")
                    {
                        break;
                    }
                    else
                    {
                        o = "";
                        continue;
                    }
                }
                else
                {
                    if (tmp[3] < 0x80)
                    {
                        count = tmp[3];
                    }
                    else
                    {
                        if (tmp[3] < 0xC0)
                        {
                            int tmpi = BitConverter.ToInt32(new byte[] { (byte)connectionStream.ReadByte(), tmp[3], 0, 0 }, 0);
                            count = tmpi ^ 0x8000;
                        }
                        else
                        {
                            if (tmp[3] < 0xE0)
                            {
                                tmp[2] = (byte)connectionStream.ReadByte();
                                int tmpi = BitConverter.ToInt32(new byte[] { (byte)connectionStream.ReadByte(), tmp[2], tmp[3], 0 }, 0);
                                count = tmpi ^ 0xC00000;
                            }
                            else
                            {
                                if (tmp[3] < 0xF0)
                                {
                                    tmp[2] = (byte)connectionStream.ReadByte();
                                    tmp[1] = (byte)connectionStream.ReadByte();
                                    int tmpi = BitConverter.ToInt32(new byte[] { (byte)connectionStream.ReadByte(), tmp[1], tmp[2], tmp[3] }, 0);
                                    count = tmpi ^ 0xE0000000;
                                }
                                else
                                {
                                    if (tmp[3] == 0xF0)
                                    {
                                        tmp[3] = (byte)connectionStream.ReadByte();
                                        tmp[2] = (byte)connectionStream.ReadByte();
                                        tmp[1] = (byte)connectionStream.ReadByte();
                                        tmp[0] = (byte)connectionStream.ReadByte();
                                        count = BitConverter.ToInt32(tmp, 0);
                                    }
                                    else
                                    {
                                        //Error in packet reception, unknown length
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    o += (Char)connectionStream.ReadByte();
                }
                if (count > 0)
                    o += '\n';
            }
            if (connectionStream.DataAvailable)
                throw new TikConnectorException("Not all data read - propably you send more than one command in one request.");

            return output;
        }

        private void EnsureLoggedOn()
        {
            if (!loggedOn || (connection == null) || (connectionStream == null))
                throw new TikConnectorException("Connection has not been opened.");
        }
    }
}
