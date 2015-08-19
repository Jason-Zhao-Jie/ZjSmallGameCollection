using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace ZjSmallGameCollection
{
    using ServerConnList = Dictionary<Socket, AGameNetworkHead.ServerClientStatue>;// List<KeyValuePair<Socket, AGameNetworkHead.NetServerStatue>>;
    using ClientList = List<KeyValuePair<Socket, byte[]>>;
    internal class GameNetwork
    {
        public delegate void SocketCall(Socket clientS, byte[] datas);
        public delegate void SocketConnCall(Socket clientS);
        internal event VoidFunc OnServerStart = null;
        internal event SocketConnCall OnServerConnect = null;
        internal event SocketCall OnServerReceive = null;
        internal event SocketConnCall OnServerDisconnect = null;
        internal event VoidFunc OnServerStop = null;
        internal event SocketConnCall OnClientConnect = null;
        internal event SocketCall OnClientReceive = null;
        internal event SocketConnCall OnClientDisconnect = null;

        private Socket serverS = null;
        private int serverPort = 0;
        private IPAddress[] ips = null;
        private int serverUser = 0;
        private ClientList serverCs = new ClientList(); //服务器的被连接客户端资源列表

        private ClientList clientS = new ClientList();  //本机创建的客户端列表
        string appName = null;
        string appNetTag = null;

        /// <summary>
        /// 构造函数，创造网络通信管理类对象
        /// </summary>
        /// <param name="serverPort">设定的服务器端口</param>
        internal GameNetwork(int serverPort, string appName, string appNetTag)
        {
            this.serverPort = serverPort;
            this.appName = appName;
            this.appNetTag = appNetTag;
            ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }
        /// <summary>
        /// 建立并开启服务器，如果本机服务器已经开启，则仅增加服务器的引用计数器
        /// </summary>
        /// <returns>是否成功</returns>
        internal bool OpenServer()
        {
            if(serverS == null)
            {
                serverS = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverS.Bind(new IPEndPoint(ips[0], serverPort));
                serverS.Listen(16);
                serverS.BeginAccept(AcceptCall, serverS);
                if(OnServerStart != null)
                    OnServerStart();
            }

            serverUser++;
            return true;
        }
        /// <summary>
        /// 关闭服务器，该操作只会减少服务器的引用计数器，如果引用计数器为0，则关闭服务器的监听
        /// </summary>
        /// <param name="isToCloseAll">是否关闭所有服务器引用</param>
        /// <returns>是否成功</returns>
        internal bool CloseServer(bool isToCloseAll = false)
        {
            serverUser--;
            if(isToCloseAll || serverUser <= 0)
            {
                serverUser = 0;
                serverS.Shutdown(SocketShutdown.Both);
                serverS.Close();
                if(OnServerStop != null)
                    OnServerStop();
            }
            return true;
        }
        /// <summary>
        /// 创建并开启客户端，返回客户端的索引号
        /// </summary>
        /// <param name="addr">要连接的服务器地址</param>
        /// <param name="port">要连接的服务器端口</param>
        /// <returns>是否成功</returns>
        internal int CreateClient(IPAddress addr, int port)
        {
            Socket cl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            KeyValuePair<Socket, byte[]> dest = new KeyValuePair<Socket, byte[]>(cl, new byte[2048]);
            int index = -1;
            try
            {
                cl.Connect(addr, port);
                for(int i = 0; i < clientS.Count; i++)
                {
                    if(clientS[i].Key == null)
                    {
                        index = i;
                        break;
                    }
                }
                if(index < 0)
                {
                    clientS.Add(dest);
                    index = clientS.Count;
                }
                else
                {
                    clientS.RemoveAt(index);
                    clientS.Insert(index, dest);
                }
                if(OnClientConnect != null)
                    OnClientConnect(cl);

                AsyncCallback recv = (IAsyncResult e) =>
                {
                    var mess = cl.EndReceive(e);
                    if(OnClientReceive != null)
                        OnClientReceive(cl, dest.Value);
                };
                BeginToReceive(dest.Value, 0, recv, cl);
            }
            catch(ObjectDisposedException)
            {
                index = -1;
                if(dest.Key != null)
                {
                    dest.Key.Close();
                    var idx = clientS.FindIndex((KeyValuePair<Socket, byte[]> obj) =>
                    {
                        if(obj.Key == dest.Key && obj.Value == dest.Value)
                            return true;
                        return false;
                    });
                    clientS.RemoveAt(idx);
                    clientS.Insert(idx, new KeyValuePair<Socket, byte[]>(null, null));
                }
                if(OnClientDisconnect != null)
                    OnClientDisconnect(dest.Key);
            }
            return index;
        }
        /// <summary>
        /// 关闭客户端，重用索引
        /// </summary>
        /// <param name="index">要关闭的客户端索引</param>
        /// <returns>关闭是否成功</returns>
        internal bool CloseClient(int index)
        {
            if(index >= 0 && clientS[index].Key != null)
            {
                clientS[index].Key.Close();
                if(OnClientDisconnect != null)
                    OnClientDisconnect(clientS[index].Key);
                clientS.RemoveAt(index);
                clientS.Insert(index, new KeyValuePair<Socket, byte[]>(null, null));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 关闭所有网络工作
        /// </summary>
        /// <returns>是否成功</returns>
        internal bool CloseAll()
        {
            var ret = CloseServer(true);
            for(int i = 0; i < clientS.Count; i++)
            {
                if(clientS[i].Key != null)
                    ret = ret && CloseClient(i);
            }
            serverCs.Clear();
            clientS.Clear();

            return ret;
        }
        /// <summary>
        /// 作为服务器对指定的客户端进行发送
        /// </summary>
        /// <param name="s">远程客户端套接字</param>
        /// <param name="mess">要发送的内容</param>
        internal void ServerSend(Socket s, byte[] mess)
        {
            s.Send(BeginToSend(mess));
        }
        /// <summary>
        /// 指定客户端对其服务器进行发送
        /// </summary>
        /// <param name="index">客户端索引</param>
        /// <param name="mess">要发送的内容</param>
        internal void ClientSend(int index, byte[] mess)
        {
            if(index >= 0 && clientS.Count > index && clientS[index].Key != null)
            {
                clientS[index].Key.Send(BeginToSend(mess));
            }
        }

        private void AcceptCall(IAsyncResult res)
        {
            KeyValuePair<Socket, byte[]> dest = new KeyValuePair<Socket, byte[]>(null, null);
            try
            {
                var src = serverS.EndAccept(res);
                dest = new KeyValuePair<Socket, byte[]>(src, new byte[2048]);
                serverCs.Add(dest);
                if(OnServerConnect != null)
                    OnServerConnect(src);
                AsyncCallback recv = (IAsyncResult e) =>
                {
                    byte[] tmpBuffer = new byte[2048];
                    var count = CopyBytesWithoutZero(tmpBuffer, 0, dest.Value);
                    var mess = src.EndReceive(e);
                    Array.Clear(dest.Value, 0, 2048);
                    var head = new GameProtocol();
                    var count2 = CopyBytesWithoutZero(tmpBuffer, count, dest.Value);
                    try
                    {
                        head.Bytes = tmpBuffer;
                        CopyBytesWithoutZero(dest.Value, head.packlen, tmpBuffer, true);

                        if(OnServerReceive != null)
                            OnServerReceive(src, head.Bytes);
                    }
                    catch
                    {

                    }
                };
                BeginToReceive(dest.Value, 0, recv, src);
            }
            catch(ObjectDisposedException)
            {
                if(dest.Key != null)
                {
                    dest.Key.Close();
                }
                serverCs.Remove(dest);
                if(OnServerDisconnect != null)
                    OnServerDisconnect(dest.Key);
            }
        }

        virtual protected void BeginToReceive(byte[] buffers, int startIndex, AsyncCallback cb, Socket src)
        {
            AsyncCallback recv = (IAsyncResult e) =>
            {
                try
                {
                    cb(e);
                    BeginToReceive(buffers, startIndex, cb, src);
                }
                catch(ObjectDisposedException)
                {
                    if(src != null)
                    {
                        src.Close();
                    }
                    for(int i = 0; i < serverCs.Count; i++)
                    {
                        if(serverCs[i].Key == src && src != null)
                        {
                            serverCs.Remove(serverCs[i]);
                            if(OnServerDisconnect != null)
                                OnServerDisconnect(src);
                            break;
                        }
                    }
                    for(int i = 0; i < clientS.Count; i++)
                    {
                        if(clientS[i].Key == src && src != null)
                        {
                            clientS.Remove(clientS[i]);
                            var idx = clientS.FindIndex((KeyValuePair<Socket, byte[]> obj) =>
                            {
                                if(obj.Key == src)
                                    return true;
                                return false;
                            });
                            clientS.RemoveAt(idx);
                            clientS.Insert(idx, new KeyValuePair<Socket, byte[]>(null, null));

                            if(OnClientDisconnect != null)
                                OnClientDisconnect(src);
                            break;
                        }
                    }
                }
            };
            src.BeginReceive(buffers, startIndex, 2048 - startIndex, SocketFlags.None, recv, src);
        }

        virtual protected byte[] BeginToSend(byte[] mess)
        {
            var head = new GameProtocol();
            head.appname = appName;
            head.appNetTag = appNetTag;
            head.dataPack = mess;
            return head.Bytes;
        }
        static protected int CopyBytesWithoutZero(byte[] buffer, int index, byte[] src, bool isCut = false)
        {
            int length = src.Length;
            bool start = false;
            int ret = -1;
            if(index >= buffer.Length || index < 0)
                return 0;
            for(int i = Math.Min(index + length, buffer.Length - 1); i >= index; i--)
            {
                if(src[i - index] != 0 || start)
                {
                    start = true;
                    buffer[i] = src[i - index];
                    if(isCut)
                        src[i - index] = 0;
                }
                if(!start)
                    ret = i - index;
            }
            return ret;
        }
    }

    internal class GameProtocol
    {
        const ushort head = 0x5A4A;                 //ZJ
        internal short packlen = 0;                 //包总长度
        internal long version = 1;                  //0.0.0.1
        internal short language = 0x0401;           //C#, windows

        internal short appNameLen = 0;
        internal string appname = "";       //应用名
        internal short appNetTagLen = 0;
        internal string appNetTag = "";     //应用指定的标签
        internal byte checksum = 0;     //校验和

        internal short dataPackLen = 0;
        internal byte[] dataPack = null;    //数据包
        DateTime nowTime;               //发送时间
        const ushort tail = 0xA4A5;

        internal int ByteLen
        {
            get
            {
                appNameLen = Convert.ToInt16(Encoding.UTF8.GetByteCount(appname));
                appNetTagLen = Convert.ToInt16(Encoding.UTF8.GetByteCount(appNetTag));
                dataPackLen = Convert.ToInt16(dataPack == null ? 0 : dataPack.Length);
                packlen = Convert.ToInt16(31 + appNameLen + appNetTagLen + dataPackLen);
                return packlen;
            }
        }

        internal byte[] Bytes
        {
            get
            {
                byte[] ret = new byte[ByteLen];
                int index = 0;

                ReplaceByte(ret, BitConverter.GetBytes(head), ref index);
                ReplaceByte(ret, BitConverter.GetBytes(packlen), ref index);
                ReplaceByte(ret, BitConverter.GetBytes(version), ref index);
                ReplaceByte(ret, BitConverter.GetBytes(language), ref index);

                ReplaceByte(ret, BitConverter.GetBytes(appNameLen), ref index);
                ReplaceByte(ret, Encoding.UTF8.GetBytes(appname), ref index);
                ReplaceByte(ret, BitConverter.GetBytes(appNetTagLen), ref index);
                ReplaceByte(ret, Encoding.UTF8.GetBytes(appNetTag), ref index);
                checksum = Convert.ToByte(ret[3] + ret[11] + ret[13] + ret[15] + ret[15 + appNameLen] + ret[17 + appNameLen] + ret[17 + appNameLen + appNetTagLen]);
                ReplaceByte(ret, new byte[] { checksum }, ref index);

                ReplaceByte(ret, BitConverter.GetBytes(dataPackLen), ref index);
                ReplaceByte(ret, dataPack, ref index);
                ReplaceByte(ret, BitConverter.GetBytes(nowTime.ToBinary()), ref index);
                ReplaceByte(ret, BitConverter.GetBytes(tail), ref index);

                return ret;
            }
            set
            {
                int index = 0;
                if(BitConverter.ToUInt16(value, index) != head)
                    throw new Exception("the data head is wrong");
                index += 2;
                packlen = BitConverter.ToInt16(value, index);
                index += 2;
                if(packlen <= 30 || packlen > value.Length)
                    throw new Exception("the data length is wrong");
                version = BitConverter.ToInt64(value, index);
                index += 8;
                language = BitConverter.ToInt16(value, index);
                index += 2;
                appNameLen = BitConverter.ToInt16(value, index);
                index += 2;
                appname = BitConverter.ToString(value, index, appNameLen);
                index += appNameLen;
                appNetTagLen = BitConverter.ToInt16(value, index);
                index += 2;
                appNetTag = BitConverter.ToString(value, index, appNetTagLen);
                index += 2;
                checksum = value[index++];
                if(checksum != Convert.ToByte(value[3] + value[11] + value[13] + value[15] + value[15 + appNameLen] + value[17 + appNameLen] + value[17 + appNameLen + appNetTagLen]))
                    throw new Exception("the data checksum is wrong");
                dataPackLen = BitConverter.ToInt16(value, index);
                index += 2;
                dataPack = new byte[dataPackLen];
                Array.Copy(value, index, dataPack, 0, dataPackLen);
                index += dataPackLen;
                nowTime = DateTime.FromBinary(BitConverter.ToInt64(value, index));
                index += 8;
                if(BitConverter.ToUInt16(value, index) != tail)
                    throw new Exception("the data tail is wrong");
            }
        }

        private static void ReplaceByte(byte[] array, byte[] replaced, ref int index)
        {
            for(int i = index; i < index + replaced.Length; i++)
            {
                array[i] = replaced[i - index];
            }
            index += replaced.Length;
        }
    }
    
    internal abstract class AGameNetworkHead
    {
        public enum ServerClientStatue : byte
        {
            Waiting,
            Connecting,
            Connected,
            Disconnect
        };
        static private GameNetwork super = null;
        static ServerConnList serverState = new ServerConnList();
        static AGameNetworkHead()
        {
            if(super != null)
            {
                super.OnServerStart += () =>
                {
                    MessageBox.Show("服务器已开启!");
                };
                super.OnServerStop += () =>
                {
                    MessageBox.Show("服务器已关闭!");
                };
                super.OnServerConnect += (Socket clientS) =>
                {
                    serverState.Add(clientS, ServerClientStatue.Connecting);
                    super.ServerSend(clientS, ConnMess);
                };
                super.OnServerDisconnect += (Socket clientS) =>
                {
                    serverState.Remove(clientS);
                };
                super.OnServerReceive += (Socket clientS, byte[] mess) =>
                {

                };
            }
        }
        protected static byte[] ConnMess
        {
            get;
        }
        protected static byte[] ConnBack
        {
            get;
        }
        static internal void StartServer()
        {
            super.OpenServer();
        }
        static internal void StopServer()
        {
            super.CloseServer();
        }
        internal int ConnectToServer(IPAddress serverAddr,int serverPort)
        {
            var ret = super.CreateClient(serverAddr, serverPort);
            return ret;
        }
        internal void DisconnectToServer(int index)
        {
            super.CloseClient(index);
        }
    }
}