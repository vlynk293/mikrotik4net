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
    /// <see cref="ITikSession"/> implementation that uses Mikrotik API to access router.
    /// </summary>
    internal sealed class ApiConnector: ITikConnector
    {
        private const int API_DEFAULT_PORT = 8728;
        private TcpClient connection;
        private NetworkStream connectionStream;
        private bool logged = false;

        #region ITikConnector Members

        /// <summary>
        /// See <see cref="ITikSession.Open(string, string, string)"/> for details.
        /// </summary>
        public void Open(string host, string user, string password)
        {
            Open(host, API_DEFAULT_PORT, user, password);
        }

        /// <summary>
        /// See <see cref="ITikSession.Open(string, int, string, string)"/> for details.
        /// </summary>
        public void Open(string host, int port, string user, string password)
        {
            //open connection
            connection = new TcpClient();
            connection.Connect(host, port); 
            connectionStream = connection.GetStream();                        

            //logon
            WriteCommand("/login");
            string hash = ReadResponse()[0].Split(new string[] { "ret=" }, StringSplitOptions.None)[1];
            string hashedPass = EncodePassword(password, hash);
            WriteMultilineCommand(string.Format(CultureInfo.InvariantCulture, "/login\n=name={0}\n=response=00{1}", user, hashedPass));
            List<string> result = ReadResponse();
            //TODO result.Length
            if (result[0] == "!done")
            {
                logged = true;
            }
            else
            {
                logged = false;
                throw new TikConnectorException("Exception during connection: {0}", string.Join("\n", result.ToArray()));
            }
        }

        public void Close()
        {
            //logoff
            if (logged)
            {
                WriteCommand("/quit"); //Does not work in 3.x (see links)
                List<string> response = ReadResponse(); //!fatalsession terminated on request
                //http://forum.mikrotik.com/viewtopic.php?f=9&t=37075
                //http://forum.mikrotik.com/viewtopic.php?f=9&t=44264
            }

            //close TCPIP
            if (connectionStream != null)
                connectionStream.Close();
            if (connection != null)
                connection.Close();

            logged = false;
        }

        public ITikEntityRow CreateEntityRow(string entityRowData)
        {
            return new ApiEntityRow(entityRowData);
        }

        public List<string> ExecuteAndReadResponse(string command)
        {
            Guard.ArgumentNotNullOrEmptyString(command, "command");
            WriteMultilineCommand(command);
            List<string> result = ReadResponse();

            return result;
        }

        /// <summary>
        /// See <see cref="ITikSession.QueryDataRows"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> QueryDataRows(string entityPath)
        {
            return QueryDataRows(entityPath, null);
        }

        /// <summary>
        /// See <see cref="ITikSession.QueryDataRows"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> QueryDataRows(string entityPath, IEnumerable<string> propertyList)
        {
            EnsureLoggedOn();

            List<ApiEntityRow> result = QueryDataRowsInternal(entityPath, propertyList, null);

            return result.Cast<ITikEntityRow>().ToList();
        }

        /// <summary>
        /// See <see cref="ITikSession.QueryDataRows"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> QueryDataRows(string entityPath, IEnumerable<string> propertyList, IEnumerable<KeyValuePair<string, string>> filter)
        {
            Guard.ArgumentNotNull(filter, "filter");
            EnsureLoggedOn();

            string filterStr = string.Join("\n", filter.Select(p => string.Format("?{0}={1}", p.Key, p.Value)).ToArray());
            List<ApiEntityRow> result = QueryDataRowsInternal(entityPath, propertyList, filterStr);

            return result.Cast<ITikEntityRow>().ToList();
        }

        public string ExecuteCreate(string entityPath, Dictionary<string, string> values)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            if (values.Count <= 0)
                throw new ArgumentException("No specified values in ExecuteAdd.", "values");

            string valuesStr = string.Join("\n", values.Select(p => string.Format("={0}={1}", p.Key, p.Value)).ToArray()); //=address=192.168.88.1/24
            string executePath = entityPath + "/add"; //ip/address/add
            string command = executePath + "\n" + valuesStr; 

            //TODO - refactor to WriteAndReadResponse
            WriteMultilineCommand(command);
            List<string> response = ReadResponse();

            if (response[0].StartsWith("!trap", StringComparison.OrdinalIgnoreCase))
                throw new TikConnectorException("API error: {0}", string.Join("\n", response.ToArray()));
            //refactor end (or may be expected lines count and regex?)

            if (response.Count != 1)
                throw new TikConnectorException("Unknown response format in ExecuteAdd.\n{0}", string.Join("\n", response.ToArray()));

            Regex responseRegex = new Regex(@"^!done\s*=ret=(?<ID>.+)$"); //!done=ret=*AEB18
            Match match = responseRegex.Match(response[0]);
            if (!match.Success) 
                throw new TikConnectorException("Unknown response format in ExecuteAdd.\n{0}", string.Join("\n", response.ToArray()));
            else
                return match.Groups["ID"].Value;
        }

        public void ExecuteUpdate(string entityPath, string id, Dictionary<string, string> values)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(id, "id");
            if (values.Count <= 0)
                throw new ArgumentException("No specified values in ExecuteUpdate.", "values");

            string valuesStr = string.Join("\n", values.Select(p => string.Format("={0}={1}", p.Key, p.Value)).ToArray()); //=address=192.168.88.1/24
            string executePath = entityPath + "/set"; //ip/address/set
            string command = executePath + "\n" + string.Format("=.id={0}", id) + "\n" + valuesStr;

            WriteMultilineCommand(command);
            List<string> response = ReadResponse();

            if (response[0].StartsWith("!trap", StringComparison.OrdinalIgnoreCase))
                throw new TikConnectorException("API error: {0}", string.Join("\n", response.ToArray()));

            if (response.Count != 1)
                throw new TikConnectorException("Unknown response format in ExecuteUpdate.\n{0}", string.Join("\n", response.ToArray()));

            if (response[0] != "!done")
                throw new TikConnectorException("Unknown response format in ExecuteUpdate.\n{0}", string.Join("\n", response.ToArray()));
        }

        public void ExecuteDelete(string entityPath, string id)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(id, "id");

            string executePath = entityPath + "/remove"; //ip/address/remove
            string command = executePath + "\n" + string.Format("=.id={0}", id);

            WriteMultilineCommand(command);
            List<string> response = ReadResponse();

            if (response[0].StartsWith("!trap", StringComparison.OrdinalIgnoreCase))
                throw new TikConnectorException("API error: {0}", string.Join("\n", response.ToArray()));

            if (response.Count != 1)
                throw new TikConnectorException("Unknown response format in ExecuteDelete.\n{0}", string.Join("\n", response.ToArray()));

            if (response[0] != "!done")
                throw new TikConnectorException("Unknown response format in ExecuteDelete.\n{0}", string.Join("\n", response.ToArray()));
        }

        public void ExecuteMove(string entityPath, string idToMove, string idToMoveBefore)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");
            Guard.ArgumentNotNullOrEmptyString(idToMove, "idToMove");
            Guard.ArgumentNotNullOrEmptyString(idToMoveBefore, "idToMoveBefore");

            //this will move queue witdh ID before queue with ID2
            //Code:
            ///queue/simple/move
            //=numbers=<queue id>
            //=destination=<queue id2>
            //http://forum.mikrotik.com/viewtopic.php?f=9&t=29390
            string executePath = entityPath + "/move"; //ip/address/move
            string command = executePath + "\n" + string.Format("=numbers={0}\n=destination={1}", idToMove, idToMoveBefore);

            WriteMultilineCommand(command);
            List<string> response = ReadResponse();

            if (response[0].StartsWith("!trap", StringComparison.OrdinalIgnoreCase))
                throw new TikConnectorException("API error: {0}", string.Join("\n", response.ToArray()));

            if (response.Count != 1)
                throw new TikConnectorException("Unknown response format in ExecuteMove.\n{0}", string.Join("\n", response.ToArray()));

            if (response[0] != "!done")
                throw new TikConnectorException("Unknown response format in ExecuteMove.\n{0}", string.Join("\n", response.ToArray()));
        }


        #endregion

        private List<ApiEntityRow> QueryDataRowsInternal(string entityPath, IEnumerable<string> propertyList, string filterStr)
        {
            string propertiesStr;
            if((propertyList == null) || !propertyList.Any())
                propertiesStr =  null;
            else
                propertiesStr = string.Format(CultureInfo.InvariantCulture, "=.proplist=" + string.Join(",", propertyList.ToArray()));

            //return Query(entity, new Dictionary<string, string>());
            string command = string.Format(CultureInfo.InvariantCulture, "{0}/print\n=detail=", entityPath);
            if (!string.IsNullOrEmpty(propertiesStr))
                command += "\n" + propertiesStr;
            if (!string.IsNullOrEmpty(filterStr))
                command += "\n" + filterStr;

            //if (filter.Count > 0)
            //    command += "\n" + string.Join("\n", filter.Select(p => string.Format(".{0}={1}", p.Key, p.Value)).ToArray()); //TODO escape?

            WriteMultilineCommand(command);
            List<string> response = ReadResponse();


            if (response[0].StartsWith("!trap", StringComparison.OrdinalIgnoreCase))
                throw new TikConnectorException("API error: {0}", string.Join("\n", response.ToArray()));

            if (response[response.Count - 1] != "!done")
                throw new TikConnectorException("Unknown response format in Query.\n{0}", string.Join("\n", response.ToArray()));

            List<ApiEntityRow> result = new List<ApiEntityRow>(response.Count - 1);
            for (int i = 0; i < response.Count - 1; i++)
            {
                result.Add(new ApiEntityRow(response[i]));
            }

            return result;
        }

        private void WriteMultilineCommand(string command)
        {
            string[] lines = command.Split('\n');
            foreach (string line in lines)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(line.ToCharArray());
                byte[] length = EncodeLength(bytes.Length);

                connectionStream.Write(length, 0, length.Length);
                connectionStream.Write(bytes, 0, bytes.Length);
            }

            connectionStream.WriteByte(0); //final byte
        }

        private void WriteCommand(string command)
        {
            //TODO verify not contains \n
            WriteMultilineCommand(command);
        }

        private static byte[] EncodeLength(int length)
        {
            if (length < 0x80)
            {
                byte[] tmp = BitConverter.GetBytes(length);
                return new byte[1] { tmp[0] };
            }
            if (length < 0x4000)
            {
                byte[] tmp = BitConverter.GetBytes(length | 0x8000);
                return new byte[2] { tmp[1], tmp[0] };
            }
            if (length < 0x200000)
            {
                byte[] tmp = BitConverter.GetBytes(length | 0xC00000);
                return new byte[3] { tmp[2], tmp[1], tmp[0] };
            }
            if (length < 0x10000000)
            {
                byte[] tmp = BitConverter.GetBytes(length | 0xE0000000);
                return new byte[4] { tmp[3], tmp[2], tmp[1], tmp[0] };
            }
            else
            {
                byte[] tmp = BitConverter.GetBytes(length);
                return new byte[5] { 0xF0, tmp[3], tmp[2], tmp[1], tmp[0] };
            }
        }

        private static string EncodePassword(string password, string hash)
        {
            byte[] hash_byte = new byte[hash.Length / 2];
            for (int i = 0; i <= hash.Length - 2; i += 2)
            {
                hash_byte[i / 2] = Byte.Parse(hash.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            byte[] heslo = new byte[1 + password.Length + hash_byte.Length];
            heslo[0] = 0;
            Encoding.ASCII.GetBytes(password.ToCharArray()).CopyTo(heslo, 1);
            hash_byte.CopyTo(heslo, 1 + password.Length);

            Byte[] hotovo;
            System.Security.Cryptography.MD5 md5;

            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            hotovo = md5.ComputeHash(heslo);

            //Convert encoded bytes back to a 'readable' string
            string result = "";
            foreach (byte h in hotovo)
            {
                result += h.ToString("x2", CultureInfo.InvariantCulture);
            }
            return result;
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
                    if (o.StartsWith("!done"))//(o.Substring(0, 5) == "!done")
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
                throw new Exception("Not all data read - propably you send more than one command in one request.");
            return output;
        }

        private void EnsureLoggedOn()
        {
            if ((connection == null) || (connectionStream == null))
                throw new TikConnectorException("Connection has not been opened.");
            //TODO verify logon
        }

        //http://ayufan.eu/projects/rosapi/repository/entry/trunk/routeros.class.php
    }
}
