using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Globalization;

namespace Tik4Net.Session.Api
{
    /// <summary>
    /// <see cref="ITikSession"/> implementation that uses Mikrotik API to access router.
    /// </summary>
    public sealed class ApiSession: ITikSession
    {
        private const int API_DEFAULT_PORT = 8728;
        private TcpClient connection;
        private NetworkStream connectionStream;
        private bool logged = false;

        #region ITikSession Members

        /// <summary>
        /// See <see cref="ITikSession.Open(string)"/> for details.
        /// </summary>
        public void Open(string host)
        {
            Open(host, API_DEFAULT_PORT);
        }

        /// <summary>
        /// See <see cref="ITikSession.Open(string, int)"/> for details.
        /// </summary>
        public void Open(string host, int port)
        {
            connection = new TcpClient();
            connection.Connect(host, port); 
            connectionStream = connection.GetStream();                        
        }

        /// <summary>
        /// See <see cref="ITikSession.LogOn"/> for details.
        /// </summary>
        public void LogOn(string user, string password)
        {            
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
                throw new TikSessionException("Exception during connection: {0}", string.Join("\n", result.ToArray()));
            }
        }

        /// <summary>
        /// See <see cref="ITikSession.LogOff"/> for details.
        /// </summary>
        public void LogOff()
        {
            if (logged)
            {
                WriteCommand("/quit"); //Does not work in 3.x (see links)
                List<string> response = ReadResponse(); //!fatalsession terminated on request
                //http://forum.mikrotik.com/viewtopic.php?f=9&t=37075
                //http://forum.mikrotik.com/viewtopic.php?f=9&t=44264
            }
        }

        /// <summary>
        /// See <see cref="ITikSession.QueryDataRows"/> for details.
        /// </summary>
        public IEnumerable<ITikEntityRow> QueryDataRows(string entityPath)
        {
            List<ApiEntityRow> result = QueryDataRowsInternal(entityPath);

            return result.Cast<ITikEntityRow>().ToList();
        }

        /// <summary>
        /// See <see cref="ITikSession.Query"/> for details.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public IEnumerable<TEntity> Query<TEntity>()
            where TEntity: ITikReadableEntity, new()
        {
            string entityPath = new TEntity().LoadPath;

            List<ApiEntityRow> response = QueryDataRowsInternal(entityPath);

            List<TEntity> result = new List<TEntity>(response.Count);
            foreach (ApiEntityRow entityRow in response)
            {
                TEntity entityInstance = new TEntity();
                entityInstance.LoadFromEntityRow(entityRow);
                result.Add(entityInstance);
            }
            return result;
        }

        //public List<string> Query(string entity, Dictionary<string, string> filter)
        //{

        //}

        #endregion

        #region IDisposable Members

        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for details.
        /// Closes TCPIP connection to router if has been established by <see cref="Open(string)"/> call.
        /// </summary>
        public void Dispose()
        {
            if (connectionStream != null)
                connectionStream.Close();
            if (connection != null)
                connection.Close();
        }

        #endregion

        private List<ApiEntityRow> QueryDataRowsInternal(string entityPath)
        {
            //return Query(entity, new Dictionary<string, string>());
            string command = string.Format(CultureInfo.InvariantCulture, "{0}/print\n=detail=", entityPath);
            //if (filter.Count > 0)
            //    command += "\n" + string.Join("\n", filter.Select(p => string.Format(".{0}={1}", p.Key, p.Value)).ToArray()); //TODO escape?

            WriteMultilineCommand(command);
            List<string> response = ReadResponse();

            if (response[0].StartsWith("!trap=", StringComparison.OrdinalIgnoreCase))
                throw new TikSessionException("API error: {0}", string.Join("\n", response.ToArray()));

            if (response[response.Count - 1] != "!done")
                throw new TikSessionException("Unknown response format.\n{0}", string.Join("\n", response.ToArray()));

            List<ApiEntityRow> result = new List<ApiEntityRow>(response.Count - 2);
            for (int i = 1; i < response.Count - 1; i++)
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
                    output.Add(o);
                    if (o.Substring(0, 5) == "!done")
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
            }
            return output;
        }

        //http://ayufan.eu/projects/rosapi/repository/entry/trunk/routeros.class.php
    }
}
