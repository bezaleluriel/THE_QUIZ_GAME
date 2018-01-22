using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QUIZ_GAME
{
    /// <summary>
    /// Class represents the flow of the game. Initializing 3 threads that calculating the questions, 
    /// creating the sequence of the questions and more.
    /// </summary>
    public class GameFlow : INotifyPropertyChanged
    {
        private DB_Connect db;
        private Iquestion[] questionsList;
        private int currentQuestionNumber = 0;
        private int currentMoney;
        private string user_email;
        private string currentQuestion;
        private string currentFirstAns;
        private string currentSecondAns;
        private string currentThirdAns;
        private string currentFourthAns;
        private string currentClue;
        private int currentCorrectAnsNumber;
        private bool gameFinished;

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Gets or sets the current question.
        /// </summary>
        /// <value>
        /// The current question.
        /// </value>
        public string CurrentQuestion
        {
            get { return currentQuestion; }
            set { currentQuestion = value; NotifyPropertyChanged("CurrentQuestion"); }
        }
        /// <summary>
        /// Gets or sets the current money.
        /// </summary>
        /// <value>
        /// The current money.
        /// </value>
        public int CurrentMoney
        {
            get { return currentMoney; }
            set { currentMoney = value; NotifyPropertyChanged("CurrentMoney"); }
        }
        /// <summary>
        /// Gets or sets the current question number.
        /// </summary>
        /// <value>
        /// The current question number.
        /// </value>
        public int CurrentQuestionNumber
        {
            get { return currentQuestionNumber; }
            set { currentQuestionNumber = value; NotifyPropertyChanged("CurrentQuestionNumber"); }
        }
        /// <summary>
        /// Gets or sets the current clue.
        /// </summary>
        /// <value>
        /// The current clue.
        /// </value>
        public string CurrentClue
        {
            get { return currentClue; }
            set { currentClue = value; NotifyPropertyChanged("CurrentClue"); }
        }
        /// <summary>
        /// Gets or sets the current first ans.
        /// </summary>
        /// <value>
        /// The current first ans.
        /// </value>
        public string CurrentFirstAns
        {
            get { return currentFirstAns; }
            set { currentFirstAns = value; NotifyPropertyChanged("CurrentFirstAns"); }
        }
        public string CurrentSecondAns
        {
            get { return currentSecondAns; }
            set { currentSecondAns = value; NotifyPropertyChanged("CurrentSecondAns"); }
        }
        /// <summary>
        /// Gets or sets the current third ans.
        /// </summary>
        /// <value>
        /// The current third ans.
        /// </value>
        public string CurrentThirdAns
        {
            get { return currentThirdAns; }
            set { currentThirdAns = value; NotifyPropertyChanged("CurrentThirdAns"); }
        }
        /// <summary>
        /// Gets or sets the current fourth ans.
        /// </summary>
        /// <value>
        /// The current fourth ans.
        /// </value>
        public string CurrentFourthAns
        {
            get { return currentFourthAns; ; }
            set { currentFourthAns = value; NotifyPropertyChanged("CurrentFourthAns"); }
        }
        /// <summary>
        /// Gets or sets the current correct ans number.
        /// </summary>
        /// <value>
        /// The current correct ans number.
        /// </value>
        public int CurrentCorrectAnsNumber
        {
            get { return currentCorrectAnsNumber; }
            set { currentCorrectAnsNumber = value; NotifyPropertyChanged("CurrentCorrectAnsNumber"); }
        }
        /// <summary>
        /// Initializes a new instance of the GameFlow class, receiving the email of the user that playing on it.
        /// </summary>
        /// <param name="mail">The mail of the user that playing.</param>
        public GameFlow(string mail)
        {
            //Sign for threads that the game finished.
            gameFinished = false;
            db = new DB_Connect();
            CurrentClue = "";
            CurrentMoney = 0;
            questionsList = new Iquestion[15];
            this.user_email = mail;
            //Building two Questions.
            buildFirstTwoQuestions();
            //Activate The first Question.
            ActivateQuestion((Iquestion)questionsList[0]);
            //Thread for first level questions.
            new Thread(() =>
            {
                buildFirstLevel();
            }).Start();
            //Thread for second level questions.
            new Thread(() =>
            {
                //Building Second Level 
                buildSecondLevel();
            }).Start();
            //Thread for third level questions.
            new Thread(() =>
            {
                buildThirdLevel();
            }).Start();

        }
        /// <summary>
        /// Receiving a question and activating it's properties to be the current question.
        /// </summary>
        /// <param name="question">The question to activate.</param>
        private void ActivateQuestion(Iquestion question)
        {
            CurrentQuestion = question.Question;
            int correctAnswerLocation;
            Random rnd = new Random();
            correctAnswerLocation = rnd.Next(0, 4);
            this.CurrentCorrectAnsNumber = correctAnswerLocation;
            ActivateAnswer(correctAnswerLocation, question.TrueAnswer);
            int wrongAnsIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i != correctAnswerLocation)
                    ActivateAnswer(i, question.WrongAnswers[wrongAnsIndex++]);
            }
        }

        /// <summary>
        ///Activating the property of the clue, will show it on GUI when activated.
        /// </summary>
        public void getClue()
        {
            CurrentClue = ((Iquestion)questionsList[currentQuestionNumber]).Clue;
        }
        /// <summary>
        /// Activates the answer propert, on specific position.
        /// </summary>
        /// <param name="position">The position to activate the answer in.</param>
        /// <param name="answer">The answer to activate at the proper property.</param>
        private void ActivateAnswer(int position, string answer)
        {
            switch (position)
            {
                case 0:
                    CurrentFirstAns = answer;
                    break;
                case 1:
                    CurrentSecondAns = answer;
                    break;
                case 2:
                    CurrentThirdAns = answer;
                    break;
                case 3:
                    CurrentFourthAns = answer;
                    break;

            }
        }

        /// <summary>
        /// Builds the first two questions of the game.
        /// It occuring on the main thread.
        /// </summary>
        private void buildFirstTwoQuestions()
        {
            ////////////////////////////
            Random rnd = new Random();
            for (int i = 0; i < 2; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                int quesitonType = rnd.Next(1, 7); // Random question type between 1-7
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 1, user_email);
            }
        }

        /// <summary>
        /// Builds the first level questions (Questions number 3-5).
        /// </summary>
        private void buildFirstLevel()
        {
            ////////////////////////////
            Random rnd = new Random();
            for (int i = 2; i < 5; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                int quesitonType = rnd.Next(1, 7); // Random question type between 1-8
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 1, user_email);
            }
        }
        /// <summary>
        /// Builds the second level questions(Questions number 5-10)
        /// </summary>
        private void buildSecondLevel()
        {
            ////////////////////////////
            Random rnd = new Random();
            for (int i = 5; i < 10; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                int quesitonType = rnd.Next(1, 8); // Random question type between 1-9
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 2, user_email);
            }
        }

        /// <summary>
        /// Builds the third level questions(Questions number 10-15)
        /// </summary>
        private void buildThirdLevel()
        {
            ////////////////////////////
            Random rnd = new Random();
            for (int i = 10; i < 15; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                //TODO: Change it to 1,9 when we have all the questions.
                int quesitonType = rnd.Next(1, 9); // Random question type between 1-9
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 3, user_email);
            }
        }

        /// <summary>
        /// Builds the question by type and level.
        /// </summary>
        /// <param name="type">The type of the question (one of 1-8).</param>
        /// <param name="level">The level to build the question for.</param>
        /// <param name="user_email">The user email.</param>
        /// <returns>The Question built</returns>
        private Iquestion buildQuestionByTypeAndLevel(int type, int level, string user_email)
        {
            switch (type)
            {
                case 1:
                    return new Q1(level, user_email);
                case 2:
                    return new Q2(level, user_email);
                case 3:
                    return new Q3(level, user_email);
                case 4:
                    return new Q4(level, user_email);
                case 5:
                    return new Q5(level, user_email);
                case 6:
                    return new Q6(level, user_email);
                case 7:
                    return new Q7(level, user_email);
                default:
                    return new Q8(level, user_email);
            }

        }
        /// <summary>
        /// Moves to next question, and activates it's properties.
        /// </summary>
        /// <returns>False if the game finished, otherwise - true</returns>
        public bool MoveToNextQuestion()
        {
            //Returning false when the game is finished
            if (currentQuestionNumber == 14)
            {
                CurrentMoney = getMoneyByQuestionNumber(currentQuestionNumber);
                finishGame();
                return false;
            }
            Stopwatch stop1 = new Stopwatch();
            stop1.Start();
            //Waiting to tje next question calculation
            while (questionsList[currentQuestionNumber + 1] == null)
            {
                //If the timeout of question calculation passed - finish the game
                if (stop1.ElapsedMilliseconds >= 20000)
                {
                    MessageBox.Show("Problem Occured While Calculating New Questions(TIMEOUT)\n, Please Restart The Game");
                    return false;
                }
            }
            if (currentQuestionNumber >= questionsList.Length)
            {
                string message = "Please wait, the  next question calculation is still in progress.";
                MessageBoxResult result = MessageBox.Show(message, "Who Wants To Be A Millionaire?", MessageBoxButton.OK);
                return false;
            }

            CurrentMoney = getMoneyByQuestionNumber(currentQuestionNumber);
            CurrentClue = "";
            currentQuestionNumber++;
            ActivateQuestion((Iquestion)questionsList[currentQuestionNumber]);
            return true;
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Updates the skills of the user according to the last question.
        /// </summary>
        /// <param name="rightAnswer">if set to <c>true</c> [right answer].</param>
        public void UpdateSkills(bool rightAnswer)
        {
            ((Iquestion)questionsList[currentQuestionNumber]).updateRelevantSkills(rightAnswer);
        }

        /// <summary>
        /// Finishes the game, insert the high score to the DB.
        /// </summary>
        public void finishGame()
        {
            this.gameFinished = true;
            db.insertHighScore(this.user_email, CurrentMoney);
        }

        /// <summary>
        /// Gets the money by question number.
        /// </summary>
        /// <param name="questionNumber">The question number.</param>
        /// <returns></returns>
        public int getMoneyByQuestionNumber(int questionNumber)
        {
            switch (questionNumber)
            {
                case 0:
                    return 100;
                case 1:
                    return 200;
                case 2:
                    return 300;
                case 3:
                    return 500;
                case 4:
                    return 1000;
                case 5:
                    return 2000;
                case 6:
                    return 4000;
                case 7:
                    return 8000;
                case 8:
                    return 16000;
                case 9:
                    return 32000;
                case 10:
                    return 64000;
                case 11:
                    return 125000;
                case 12:
                    return 250000;
                case 13:
                    return 500000;
                default:
                    return 1000000;
            }
        }
    }
}
