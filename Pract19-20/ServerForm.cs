using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pract19_20
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Service();
        }


        private async void Service()
        {
            await Task.Run(() =>
            {
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8888);

                Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sock.Bind(ipEndPoint);
                    sock.Listen(10);
                    
                    byte[] bytes = new byte[1024];

                    while (true)
                    {
                        Socket s = sock.Accept();
                        int bytesCount = 0;

                        try
                        {
                            bytesCount = s.Receive(bytes);
                        }
                        catch(SocketException)
                        {
                            MessageBox.Show("Клиент отключился","Отключение",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            s.Shutdown(SocketShutdown.Both);
                            s.Close();
                            sock.Close();
                            Service();
                            break;
                        }
                        
                        string data = Encoding.UTF8.GetString(bytes, 0, bytesCount);

                        if (textBox1.InvokeRequired)
                            textBox1.Invoke(new Action(() =>
                            {
                                textBox1.Text = data;
                            }));
                        else
                            continue;
                        
                        string reply = "Query size:" + data.Length.ToString()
                            + "chairs";
                        byte[] msg = Encoding.UTF8.GetBytes(reply);
                        s.Send(msg);
                        if (data.IndexOf("<TheEnd>") > -1)
                        {
                            MessageBox.Show("Connection received", "Status of connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        s.Shutdown(SocketShutdown.Both);
                        s.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        
    }
}
