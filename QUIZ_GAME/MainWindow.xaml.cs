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
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            //this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            WaitingWindow waitingWin = new WaitingWindow();
            waitingWin.Show();
            this.Close();
            
            new Thread(() =>
            {
                GameFlow gameFlow = new GameFlow("halfonamir1@gmail.com");
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    Game game = new Game(gameFlow);
                    game.Show();
                    waitingWin.Close();
                }));
            }).Start();
            
            
        }
    }
}
