using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Форма_отправки
{   

    public partial class MainWindow : Window
    {

        static TcpClient client;
        private const string host = "192.168.1.3";
        private const int port = 50000;
        static NetworkStream stream;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration frm = new Registration(this);
            frm.ShowDialog();

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (textBoxLogin.Text == "" || PasswordBox.Password == "")
                MessageBox.Show("Заполните все поля", "Error");
            else
            {
                Window2 frm = new Window2(this, textBoxLogin.Text, PasswordBox.Password);
                frm.Show();
                Close();
            }
            
        }

       
    }
}