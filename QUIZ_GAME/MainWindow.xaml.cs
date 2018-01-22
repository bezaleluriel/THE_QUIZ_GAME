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
using System.Windows.Navigation;
using System.Windows.Shapes;
using QUIZ_GAME;
using System.Threading;

namespace QUIZ_GAME
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            if (Properties.Settings.Default.user_name.Length == 0)
                loginRegBtnText.Text = "Login";
            else
            {
                loginRegBtnText.Text = "Log Out";
                txtHello.Text = Properties.Settings.Default.user_name;
                stackHello.Visibility = Visibility.Visible;
            }
                

            //   txtHello.Text = "Hello " + Properties.Settings.Default.user_name;
        }




        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.user_name.Length == 0)
            {
                MessageBox.Show("You need to Login/Register to start a game!");
                return;
            }

            WaitingWindow waitingWin = new WaitingWindow();
            waitingWin.Show();
            this.Close();
            
            new Thread(() =>
            {
                GameFlow gameFlow = new GameFlow(Properties.Settings.Default.user_email);
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    Game game = new Game(gameFlow);
                    game.Show();
                    waitingWin.Close();
                }));
            }).Start();
            Thread.Sleep(2000);
            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.user_name.Length == 0) {
                Login login = new Login();
                login.ShowDialog();
            }
            //Else - Log Out
            else
            {
                btnRegister.Visibility = Visibility.Visible;
                loginRegBtnText.Text = "Login";
                stackHello.Visibility = Visibility.Hidden;
                Properties.Settings.Default["user_name"] = "";
                Properties.Settings.Default["user_email"] = "";
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.user_name.Length != 0)
            {
                loginRegBtnText.Text = "Log Out";
                btnRegister.Visibility = Visibility.Hidden;
                txtHello.Text = Properties.Settings.Default.user_name;
                stackHello.Visibility = Visibility.Visible;

            }


                //txtHello.Text = "Hello " + Properties.Settings.Default.user_name;
                //this.Close();
            }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
            if (Properties.Settings.Default.user_name.Length != 0)
            {
                loginRegBtnText.Text = "Log Out";
                btnRegister.Visibility = Visibility.Hidden;
                txtHello.Text = Properties.Settings.Default.user_name;
                stackHello.Visibility = Visibility.Visible;
            }

        }

        private void btnHighScores_Click(object sender, RoutedEventArgs e)
        {
            HighScoresWindow hsWin = new HighScoresWindow(false, "");
            hsWin.Show();
            this.Close();
        }
    }
}
