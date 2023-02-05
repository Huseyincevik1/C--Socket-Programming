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
    struct Client_Info
    {
        public Socket clientSocket;
        public int CLientID;
    }
    public partial class Frmsocketserver : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Client_Info> clientSockets_List = new List<Client_Info>();
        int BUFFER_SIZE = 2048;
        int Client_ID = 0;
        int tempID;

        byte[] buffer;
        private bool a;

        public Frmsocketserver()
        {
            InitializeComponent();


            buffer = new byte[BUFFER_SIZE];
            
        }

        void startServer()
        {

            if (textBox1.Text == "127.0.0.1" && textBox2.Text == "800")
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                serverSocket.Bind(new IPEndPoint(ipAddress, 800));
                serverSocket.Listen(50);
                serverSocket.BeginAccept(Accept_Callback, null);
            }
            else
            {
                MessageBox.Show("bağlantı başarısız");

            }

        }
        void StopServer()
        {
            serverSocket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), serverSocket);
            serverSocket.Close();
        }


        void Accept_Callback(IAsyncResult AR)
        {


            Socket ClientSocket;

            try
            {
                ClientSocket = serverSocket.EndAccept(AR);

                Client_ID++;
                Client_Info cl = new Client_Info();
                cl.clientSocket = ClientSocket;
                cl.CLientID = Client_ID;
                clientSockets_List.Add(cl);



                ClientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, Receive_Callback, ClientSocket);

                serverSocket.BeginAccept(Accept_Callback, null);
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Server Error");
            }


        }

        void Receive_Callback(IAsyncResult AR)
        {

            Socket currentClient = (Socket)AR.AsyncState;
            int receivedSize;
            byte[] recBuffer;
            string text;
            try
            {
                receivedSize = currentClient.EndReceive(AR);
                recBuffer = new byte[receivedSize];
                Array.Copy(buffer, recBuffer, receivedSize);
                text = Encoding.ASCII.GetString(recBuffer);
                foreach (var item in clientSockets_List)
                {
                    if (item.clientSocket == currentClient)
                    {
                        this.BeginInvoke(new MethodInvoker(() => {/*textbox3*/ textBox3.Text += "Client " + item.CLientID + ": " + text + "\n"; }));
                    }
                }

                currentClient.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, Receive_Callback, currentClient);
            }
            catch (SocketException ex)
            {

                currentClient.Close();
                foreach (Client_Info Item in clientSockets_List)
                {
                    if (currentClient == Item.clientSocket)
                    {
                        clientSockets_List.Remove(Item);
                    }
                }

                MessageBox.Show("Server", ex.Message);

            }

        }


        public void Send_Callback(IAsyncResult ar)
        {


            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private static void DisconnectCallback(IAsyncResult ar)
        {

            Socket serverSocket = (Socket)ar.AsyncState;
            serverSocket.EndDisconnect(ar);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(textBox4.Text);

                foreach (Client_Info Item in clientSockets_List)
                {
                    Item.clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(Send_Callback), Item.clientSocket);
                }
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server", ex.Message);
            }

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            startServer();
            foreach (Client_Info Item in clientSockets_List)
            {
                if (tempID == Item.CLientID)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(textBox4.Text);
                    Item.clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(Send_Callback), Item.clientSocket);
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopServer();
        }

    }
}
