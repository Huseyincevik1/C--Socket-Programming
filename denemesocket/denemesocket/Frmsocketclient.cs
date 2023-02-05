using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using SimpleTCP;

namespace denemesocket
{
    public partial class Frmsocketclient : Form
    {
        Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        byte[] byteData;
        public Frmsocketclient()
        {
            InitializeComponent();

        }

        private void ConnectToServer()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            byteData = new byte[1024];
            if (textBox1.Text == "127.0.0.1" && textBox2.Text == "800")
            {
                Server.BeginConnect(new IPEndPoint(ipAddress, 800), new AsyncCallback(Connect_Callback), null);


            }
            else
            {
                MessageBox.Show("bağlantı başarısız");

            }


        }
        void StopServer()
        {

            Server.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), Server);

            Server.Close();
        }
        private static void DisconnectCallback(IAsyncResult ar)
        {

            Socket Socket = (Socket)ar.AsyncState;
            Socket.EndDisconnect(ar);

        }



        private void Connect_Callback(IAsyncResult ar)
        {
            try
            {

                Server.EndConnect(ar);

                byte[] buffer = Encoding.ASCII.GetBytes("im new client");
                Server.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Send_Callback), Server);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "client error");
            }
        }

        void Receive_Callback(IAsyncResult AR)
        {
            int receivedSize = Server.EndReceive(AR);
            byte[] recBuffer;
            string text;
            try
            {

                recBuffer = new byte[receivedSize];
                Array.Copy(byteData, recBuffer, receivedSize);
                text = Encoding.ASCII.GetString(recBuffer);
                this.BeginInvoke(new MethodInvoker(() => { textBox3.Text += "\n" + text + "\n"; }));
                Server.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, Receive_Callback, Server);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "client error");

            }

        }

        public void Send_Callback(IAsyncResult ar)
        {
            try
            {
                Server = (Socket)ar.AsyncState;
                Server.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client Error: ", ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(textBox4.Text);
            Server.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Send_Callback), Server);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectToServer();
            Server.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(Receive_Callback), Server);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopServer();
        }
    }
}
