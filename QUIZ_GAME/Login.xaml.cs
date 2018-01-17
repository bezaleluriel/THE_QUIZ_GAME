using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if (emailBox.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                emailBox.Focus();
            }
            else if (!Regex.IsMatch(emailBox.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                emailBox.Select(0, emailBox.Text.Length);
                emailBox.Focus();
            }
            else
            {
                string email = emailBox.Text;
                string password = PasswordBox.Password;

                DB_Connect db_connect = new DB_Connect();
                List<string>[] answer = db_connect.Select(email, password);
                if (answer != null)
                {
                    errormessage.Text = "Login successfully";
                    string user_name = answer[2][0];
                    // here we will want to save the user email and nickname in app.config/ settings .
                    Properties.Settings.Default["user_name"] = user_name;
                    Properties.Settings.Default["user_email"] = email;
                    Properties.Settings.Default.Save();
                    Reset();
                    this.Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing email/password.";
                }
                
            }

        }
    }
}
