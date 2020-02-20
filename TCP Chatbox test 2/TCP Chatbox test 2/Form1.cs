using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// my code start here
using System.Net;
using System.Net.Sockets;
using System.IO;


//stop import


namespace TCP_Chatbox_test_2
{
    public partial class Form1 : Form
    {

        /*declaring variables*/
        private TcpClient client;
        public StreamReader Sreader;
        public StreamWriter Swriter;
        public string recieve;
        public string texttosend;


            //end declaring

        public Form1()
        {
            InitializeComponent();

            //starting my implementation
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox1.Text = address.ToString();
                }
            }
            //end of my imp
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("alan is gay");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(textBox2.Text));
                listener.Start();
                client = listener.AcceptTcpClient();
                Sreader = new StreamReader(client.GetStream());
                Swriter = new StreamWriter(client.GetStream());
                Swriter.AutoFlush = true;

                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            } catch
            {
                MessageBox.Show("WHAAAT");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(textBox3.Text), int.Parse(textBox4.Text));

            try
            {
                client.Connect(IpEnd);

                if (client.Connected)
                {
                    textBox5.AppendText("Connected to server" + "\n");
                    Swriter = new StreamWriter(client.GetStream());
                    Sreader = new StreamReader(client.GetStream());
                    Swriter.AutoFlush = true;
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    recieve = Sreader.ReadLine();
                    this.textBox5.Invoke(new MethodInvoker(delegate ()
                    {
                        textBox5.AppendText("You:" + recieve + "\n");
                    }));
                    recieve = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            if (client.Connected)
            {
                Swriter.WriteLine(texttosend);
                this.textBox5.Invoke(new MethodInvoker(delegate ()
                {
                    textBox5.AppendText("Me:" + texttosend + "\n");
                }));
            }
            else
            {
                MessageBox.Show("Sending failed");
            }
            backgroundWorker2.CancelAsync();
        }


       
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                texttosend = textBox6.Text;
                backgroundWorker2.RunWorkerAsync();
            }
            textBox6.Text = "";
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)13)
            {
                if (textBox6.Text != "")
                {
                    texttosend = textBox6.Text;
                    backgroundWorker2.RunWorkerAsync();
                }
                textBox6.Text = "";
                // Enter key pressed
            }
        
        }

      
    }
}
