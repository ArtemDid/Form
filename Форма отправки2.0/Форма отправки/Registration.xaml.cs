using System.Windows;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

namespace Форма_отправки
{
    public partial class Registration : Window
    {
        static TcpClient client;
        private const string host = "192.168.1.3";
        private const int port = 50000;
        static NetworkStream stream;
        public Registration(MainWindow mainWindow)
        {
            InitializeComponent();
            passwordBox.MaxLength = 16;
        }

        //public Registration(Registration registration)
        //{
        //}

        private void button_Click(object sender, RoutedEventArgs e)
        {                   
            

            if (textBoxLogin.Text == "" || textBoxName.Text == "" || passwordBox == null)
            {
                MessageBox.Show("Invalid info", "Please fill all fields");
            }
            else 
            {
                client = new TcpClient(); //чи да.
                try
                {
                    client.Connect(host, port);
                    stream = client.GetStream();

                    string message = "Регистрация" + "\n" + textBoxName.Text + "\n" + textBoxLogin.Text + "\n" + passwordBox.Password;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Disconnect();
                    Hide();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }          
            
        }
        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            /*Environment.Exit(0);*/ //завершение процесса
        }
    }
}