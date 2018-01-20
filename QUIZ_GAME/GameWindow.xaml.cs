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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private GameFlow gameFlow;
        public Game(GameFlow gf)
        {
            //gameFlow = new GameFlow("halfonamir1@gmail.com");
            this.gameFlow = gf;
            InitializeComponent();
            this.DataContext = gameFlow;
            
        }

        private void btnA1_Click(object sender, RoutedEventArgs e)
        {
            if(this.gameFlow.CurrentCorrectAnsNumber == 0)
            {
                MessageBeforeNextWin(true);
                gameFlow.UpdateSkills(true);
                gameFlow.MoveToNextQuestion();
                moneyImg.Source = new BitmapImage(new Uri(@"/images/money/" + gameFlow.CurrentQuestionNumber.ToString() + ".jpg", UriKind.Relative));
            }
            else
            {
                gameFlow.finishGame();
                MessageBeforeNextWin(false);
                gameFlow.UpdateSkills(false);
                HighScoresWindow highScoresWindow = new HighScoresWindow(true,gameFlow.CurrentMoney.ToString());
                highScoresWindow.Show();
                this.Close();
            }
        }

        private void btnA2_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameFlow.CurrentCorrectAnsNumber == 1)
            {
                MessageBeforeNextWin(true);
                gameFlow.UpdateSkills(true);
                gameFlow.MoveToNextQuestion();
                moneyImg.Source = new BitmapImage(new Uri(@"/images/money/" + gameFlow.CurrentQuestionNumber.ToString() + ".jpg", UriKind.Relative));
            }
            else
            {
                gameFlow.finishGame();
                MessageBeforeNextWin(false);
                gameFlow.UpdateSkills(false);
                HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                highScoresWindow.Show();
                this.Close();
            }
        }

        private void btnA3_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameFlow.CurrentCorrectAnsNumber == 2)
            {
                MessageBeforeNextWin(true);
                gameFlow.UpdateSkills(true);
                gameFlow.MoveToNextQuestion();
                moneyImg.Source = new BitmapImage(new Uri(@"/images/money/" + gameFlow.CurrentQuestionNumber.ToString() + ".jpg", UriKind.Relative));
            }
            else
            {
                gameFlow.finishGame();
                MessageBeforeNextWin(false);
                gameFlow.UpdateSkills(false);
                HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                highScoresWindow.Show();
                this.Close();
            }
        }

        private void btnA4_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameFlow.CurrentCorrectAnsNumber == 3)
            {
                MessageBeforeNextWin(true);
                gameFlow.UpdateSkills(true);
                gameFlow.MoveToNextQuestion();
                moneyImg.Source = new BitmapImage(new Uri(@"/images/money/" + gameFlow.CurrentQuestionNumber.ToString() + ".jpg", UriKind.Relative));
            }
            else
            {
                gameFlow.finishGame();
                MessageBeforeNextWin(false);
                gameFlow.UpdateSkills(false);
                HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                highScoresWindow.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gameFlow.getClue();
        }

        public void MessageBeforeNextWin(bool trueAnswer)
        {
            
            string message;
            if (trueAnswer)
                message = "You Right!\nMove to next question?";
            else
                message = "You Were Wrong!\nGAME OVER!";
            MessageBoxResult result = MessageBox.Show(message, "Who Wants To Be A Millionaire?", MessageBoxButton.OK);
        }
    }
}
