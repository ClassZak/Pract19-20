using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class ClientForm : Form
    {
        
        public ClientForm()
        {
            InitializeComponent();
        }

        async void Communicate(string hostname, int port)
        {
            await Task.Run(() =>
            {
                try
                {
                    byte[] bytes = new byte[1024];
                    string message="";
                    if (textBox1.InvokeRequired)
                        textBox1.Invoke(new Action(() =>
                        {
                            message = textBox1.Text;
                        }));


                    byte[] data = Encoding.UTF8.GetBytes(message);

                    IPHostEntry ipHost = Dns.GetHostEntry(hostname);
                    IPAddress ipAddr = ipHost.AddressList[0];
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                    Socket sock = new Socket(ipAddr.AddressFamily, SocketType.
                    Stream, ProtocolType.Tcp);

                    //Connecting to server
                    sock.Connect(ipEndPoint);

                    int bytesSent = sock.Send(data);
                    int bytesRec = sock.Receive(bytes);

                    //answer of server
                    if (textBox2.InvokeRequired)
                        textBox2.Invoke(new Action(() =>
                        {
                            textBox2.Text = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        }));
                    

                    if (message.IndexOf("<TheEnd>") == -1)
                        Communicate(hostname, port);

                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
                catch (System.Net.Sockets.SocketException)
                {
                    MessageBox.Show
                    (
                        "Ошибка подключения!\nПопробуйте перезапустить серверную часть приложения",
                        "Ошибка подключения",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
