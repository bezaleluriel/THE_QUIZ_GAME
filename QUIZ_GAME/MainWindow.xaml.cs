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
            Game game = new Game();
            game.Show();
            this.Close();
        }
    }
}
