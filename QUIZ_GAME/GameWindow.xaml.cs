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
        MediaPlayer mplayer;
        public Game(GameFlow gf)
        {
            mplayer = new MediaPlayer();
            this.gameFlow = gf;
            InitializeComponent();
            this.DataContext = gameFlow;
            this.Loaded += GameWindow_Loaded;



        }

        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mplayer.Open(new Uri(@"../../images/sound1.mp3", UriKind.Relative));
            mplayer.Play();
        }

        private void btnA1_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameFlow.CurrentCorrectAnsNumber == 0)
            {
                MessageBeforeNextWin(true);
                gameFlow.UpdateSkills(true);
                if (gameFlow.MoveToNextQuestion() == false)
                {
                    HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                    highScoresWindow.Show();
                    this.Close();
                }
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

        private void btnA2_Click(object sender, RoutedEventArgs e)
        {
            if (this.gameFlow.CurrentCorrectAnsNumber == 1)
            {
                MessageBeforeNextWin(true);
                gameFlow.UpdateSkills(true);
                if (gameFlow.MoveToNextQuestion() == false)
                {
                    HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                    highScoresWindow.Show();
                    this.Close();

                }
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
                if (gameFlow.MoveToNextQuestion() == false)
                {
                    HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                    highScoresWindow.Show();
                    this.Close();
                }
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
                if (gameFlow.MoveToNextQuestion() == false)
                {
                    HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                    highScoresWindow.Show();
                    this.Close();
                }
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
            mplayer.Play();
            string message;
            if (trueAnswer)
            {
                mplayer.Open(new Uri(@"../../images/correct.mp3", UriKind.Relative));
                mplayer.Play();
                message = "You Right!\nMove to next question?";
            }

            else
            {
                mplayer.Open(new Uri(@"../../images/wrong.mp3", UriKind.Relative));
                mplayer.Play();
                message = "You Were Wrong!\nGAME OVER!";
            }

            MessageBoxResult result = MessageBox.Show(message, "Who Wants To Be A Millionaire?", MessageBoxButton.OK);
        }

        private void btnRetire_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to retire?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                gameFlow.finishGame();
                HighScoresWindow highScoresWindow = new HighScoresWindow(true, gameFlow.CurrentMoney.ToString());
                highScoresWindow.Show();
                this.Close();
            }


        }
    }
}
