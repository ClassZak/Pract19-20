using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace OneThreadServer
{
    class Program
{
        static void Main(string[]args)
{
            Console.OutputEncoding = Encoding.GetEncoding(866);
            Console.WriteLine("Однопоточныйсерверзапущен");
            //Подготавливаемконечнуюточкудлясокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8888);
            //Создаемпотоковыйсокет,протоколTCP/IP
            Socket sock = new Socket(ipAddr.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //связываемсокетсконечнойточкой
                sock.Bind(ipEndPoint);
                //начинаемпрослушкусокета
                sock.Listen(10);
                //Начинаемслушатьсоединения
                while (true)
                {
                    Console.WriteLine("Слушаем,порт{0}", ipEndPoint);
                    //Программаприостанавливается,
                    //ожидаявходящеесоединение
                    //сокетдляобменаданнымисклиентом
                    Socket s = sock.Accept();
                    //сюдабудемзаписыватьполученныеотклиентаданные
                    string data = null;
                    //Клиентесть,начинаемчитатьотнегозапрос
                    //массивполученныхданных
                    byte[] bytes = new byte[1024];
                    //длинаполученныхданных
                    int bytesCount = s.Receive(bytes);
                    //Декодируемстроку
                    data += Encoding.UTF8.GetString(bytes, 0, bytesCount);
                    //Показываемданныенаконсоли
                    Console.Write("Данныеотклиента:" + data + "\n\n");
                    //Отправляемответклиенту
                    string reply = "Querysize:" + data.Length.ToString()
                    + "chars";
                    //кодируемответсервера
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    //отправляемответсервера
                    s.Send(msg);
                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Соединениезавершено.");
                        break;
                    }
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
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
    }
}