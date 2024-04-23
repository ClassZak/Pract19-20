using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Communicate("localhost", 8888);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }

        }

        static void Communicate(string hostname, int port)
        {
            byte[] bytes = new byte[1024];
            string message = null;
            byte[] data = Encoding.UTF8.GetBytes(message);

            IPHostEntry ipHost = Dns.GetHostEntry(hostname);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.
            Stream, ProtocolType.Tcp);

            //Подключаемся к серверу
            sock.Connect(ipEndPoint);

            int bytesSent = sock.Send(data);
            int bytesRec = sock.Receive(bytes);

            //TextBox + label

            //answer of server

            if (message.IndexOf("<TheEnd>") == -1)
                Communicate(hostname, port);

            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}
