using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZjSmallGameCollection
{
    using ClientList = List<KeyValuePair<Socket, List<ArraySegment<byte>>>>;
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
        
        /// <summary>
        /// 构造函数，创造网络通信管理类对象
        /// </summary>
        /// <param name="serverPort">设定的服务器端口</param>
        internal GameNetwork(int serverPort)
        {
            this.serverPort = serverPort;
            ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }
        /// <summary>
        /// 建立并开启服务器，如果本机服务器已经开启，则仅增加服务器的引用计数器
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
        internal bool CloseServer()
        {
            serverUser--;
            if(serverUser <= 0)
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
        /// <param name="addr"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        internal int CreateClient(IPAddress addr,int port)
        {
            Socket cl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            KeyValuePair<Socket, List<ArraySegment<byte>>> dest = new KeyValuePair<Socket, List<ArraySegment<byte>>>(cl, new List<ArraySegment<byte>>());
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
                        OnClientReceive(cl, dest.Value[0].Array);
                };
                BeginToReceive(dest.Value, recv, cl);
            }
            catch(ObjectDisposedException)
            {
                index = -1;
                if(dest.Key != null)
                {
                    dest.Key.Close();
                    var idx = clientS.FindIndex((KeyValuePair<Socket, List<ArraySegment<byte>>> obj) =>
                    {
                        if(obj.Key == dest.Key && obj.Value == dest.Value)
                            return true;
                        return false;
                    });
                    clientS.RemoveAt(idx);
                    clientS.Insert(idx, new KeyValuePair<Socket, List<ArraySegment<byte>>>(null, new List<ArraySegment<byte>>()));
                }
                if(OnClientDisconnect != null)
                    OnClientDisconnect(dest.Key);
            }
            return index;
        }
        /// <summary>
        /// 关闭客户端，重用索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool CloseClient(int index)
        {
            if(index >= 0 && clientS[index].Key != null)
            {
                clientS[index].Key.Close();
                if(OnClientDisconnect != null)
                    OnClientDisconnect(clientS[index].Key);
                clientS.RemoveAt(index);
                clientS.Insert(index, new KeyValuePair<Socket, List<ArraySegment<byte>>>(null, new List<ArraySegment<byte>>()));
                return true;
            }
            return false;
        }

        private void AcceptCall(IAsyncResult res)
        {
            KeyValuePair<Socket, List<ArraySegment<byte>>> dest = new KeyValuePair<Socket, List<ArraySegment<byte>>>(null, new List<ArraySegment<byte>>());
            try
            {
                var src = serverS.EndAccept(res);
                dest = new KeyValuePair<Socket, List<ArraySegment<byte>>>(src, new List<ArraySegment<byte>>());
                serverCs.Add(dest);
                if(OnServerConnect != null)
                    OnServerConnect(src);
                AsyncCallback recv = (IAsyncResult e) =>
                {
                    var mess = src.EndReceive(e);
                    if(OnServerReceive != null)
                        OnServerReceive(src, dest.Value[0].Array);
                };
                BeginToReceive(dest.Value, recv, src);
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

        private void BeginToReceive(IList<ArraySegment<byte>> buffers, AsyncCallback cb, Socket src)
        {
            AsyncCallback recv = (IAsyncResult e) =>
            {
                try
                {
                    cb(e);
                    src.BeginReceive(buffers, SocketFlags.None, cb, src);
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
                            var idx = clientS.FindIndex((KeyValuePair<Socket, List<ArraySegment<byte>>> obj) =>
                            {
                                if(obj.Key == src)
                                    return true;
                                return false;
                            });
                            clientS.RemoveAt(idx);
                            clientS.Insert(idx, new KeyValuePair<Socket, List<ArraySegment<byte>>>(null, new List<ArraySegment<byte>>()));

                            if(OnClientDisconnect != null)
                                OnClientDisconnect(src);
                            break;
                        }
                    }
                }
            };
            src.BeginReceive(buffers, SocketFlags.None, recv, src);
        }
    }
}
