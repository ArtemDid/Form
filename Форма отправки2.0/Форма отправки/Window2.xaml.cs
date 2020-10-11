using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Форма_отправки
{
    public partial class Window2 : Window
    {
        //public static string userName;
        private const string host = "10.3.201.14"; // Вынести в Appconfig
        private const int port = 50000;//тоже ^
        static TcpClient client;
        static NetworkStream stream;

        public Window2(MainWindow mainWindow,string name,string password)
        {
            InitializeComponent();
            Connect(name, password);
        }
        private void Connect(string name, string password)
        {
           
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = name;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                listBox.Items.Add($"Добро пожаловать");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

                MainWindow MW = new MainWindow();

                MW.Show();

                this.Close();
            }
        }
        private void SendMessage()
        {
            byte[] data = Encoding.Unicode.GetBytes(textBoxMessage.Text);
            stream.Write(data, 0, data.Length);
            listBox.Items.Add(textBoxMessage.Text);
            textBoxMessage.Text = "";
        }
        void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    Dispatcher.BeginInvoke(new ThreadStart(delegate { listBox.Items.Add(message); }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString()); //соединение было прервано
                    Disconnect();
                }
            }
        }
        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }

        

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();

            MainWindow win = new MainWindow();
            win.Show();

            this.Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
    }
}