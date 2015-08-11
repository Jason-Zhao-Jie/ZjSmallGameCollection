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
    internal class GameNetwork
    {
        public delegate void SocketCall(IPAddress client, int port, byte[] datas);
        public delegate void SocketConnCall(IPAddress client, int port);
        internal event VoidFunc OnServerStart = null;
        internal event SocketConnCall OnConnect = null;
        internal event SocketCall OnReceive = null;
        internal event SocketConnCall OnDisconnect = null;
        internal event VoidFunc OnServerStop = null;

        private Socket serverS = null;
        private int serverPort = 0;
        private IPAddress[] ips = null;
        private int serverUser = 0;
        private List<Socket> ServerCs = new List<Socket>(); //服务器的被连接客户端列表

        private List<Socket> clientS = new List<Socket>();  //本机创建的客户端列表
        
        internal GameNetwork(int serverPort)
        {
            this.serverPort = serverPort;
            ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }

        internal bool OpenServer()
        {
            if(serverS == null)
            {
                serverS = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverS.Bind(new IPEndPoint(ips[0], serverPort));
                serverS.Listen(16);
                serverS.BeginAccept(AcceptCall);
            }

            serverUser++;
            return true;
        }

        internal bool CloseServer()
        {
            serverUser--;
            if(serverUser <= 0)
            {
                serverUser = 0;
                serverS.EndAccept();
                serverS.Shutdown(SocketShutdown.Both);
                serverS.Close();
            }
            return true;
        }

        private void AcceptCall(IAsyncResult res)
        {

        }
    }
}
