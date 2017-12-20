using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QUIZ_GAME
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// Uriel is my king
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            emailBox.Text = "";
            PasswordBox.Password = "";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = emailBox.Text;
            string password = PasswordBox.Password;

            DB_Connect db_connect = new DB_Connect();          
            List<string>[] answer = db_connect.Select(email, password);
            Console.WriteLine("email = " + answer[0] + "password = " + answer[1]);

            Reset();
        }
    }
}
