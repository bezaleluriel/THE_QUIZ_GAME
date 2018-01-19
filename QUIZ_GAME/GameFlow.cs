﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QUIZ_GAME
{
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

        public string CurrentQuestion
        {
            get { return currentQuestion; }
            set { currentQuestion = value; NotifyPropertyChanged("CurrentQuestion"); }
        }
        public int CurrentMoney
        {
            get { return currentMoney; }
            set { currentMoney = value; NotifyPropertyChanged("CurrentMoney"); }
        }
        public string CurrentClue
        {
            get { return currentClue; }
            set { currentClue = value; NotifyPropertyChanged("CurrentClue"); }
        }
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
        public string CurrentThirdAns
        {
            get { return currentThirdAns; }
            set { currentThirdAns = value; NotifyPropertyChanged("CurrentThirdAns"); }
        }
        public string CurrentFourthAns
        {
            get { return currentFourthAns; ; }
            set { currentFourthAns = value; NotifyPropertyChanged("CurrentFourthAns"); }
        }
        public int CurrentCorrectAnsNumber
        {
            get { return currentCorrectAnsNumber; }
            set { currentCorrectAnsNumber = value; NotifyPropertyChanged("CurrentCorrectAnsNumber"); }
        }

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
            new Thread(() =>
            {
                buildFirstLevel();
            }).Start();
            new Thread(() =>
            {
                //Building Second Level 
                buildSecondLevel();
            }).Start();
            new Thread(() =>
            {
                buildThirdLevel();
            }).Start();

        }

        private void ActivateQuestion(Iquestion question)
        {
            CurrentQuestion = question.Question;
            int correctAnswerLocation;
            Random rnd = new Random();
            correctAnswerLocation = rnd.Next(0, 4);
            this.CurrentCorrectAnsNumber = correctAnswerLocation;
            ActivateAnswer(correctAnswerLocation, question.TrueAnswer);
            int wrongAnsIndex = 0;
            for(int i = 0; i < 4; i++)
            {
                if(i != correctAnswerLocation)
                {
                    //i%3 Because we have 3 wrong answers
                    ActivateAnswer(i, question.WrongAnswers[wrongAnsIndex++]);
                }
            }
        }

        public void getClue()
        {
            CurrentClue = ((Iquestion)questionsList[currentQuestionNumber]).Clue;
        }

        private void ActivateAnswer(int position,string answer)
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

        private void buildFirstTwoQuestions()
        {
            ////////////////////////////
            Random rnd = new Random();
            for (int i = 0; i < 2; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                //TODO: Change it to 1,9 when we have all the questions.
                int quesitonType = rnd.Next(1, 7); // Random question type between 1-6
                this.questionsList[i]= buildQuestionByTypeAndLevel(quesitonType, 1, user_email);
                //this.questionsList.Add(buildQuestionByTypeAndLevel(quesitonType, 1,user_email));
            }
        }

        private void buildFirstLevel()
        {
            ////////////////////////////
            Random rnd = new Random();
            for(int i = 2; i < 5; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                //TODO: Change it to 1,9 when we have all the questions.
                int quesitonType = rnd.Next(1, 7); // Random question type between 1-6
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 1, user_email);
                //this.questionsList.Add(buildQuestionByTypeAndLevel(quesitonType, 1,user_email));
            }
        }
        private void buildSecondLevel()
        {
            ////////////////////////////
            Random rnd = new Random();
            for (int i = 5; i < 10; i++)
            {
                //If the game finished - kill the thread.
                if (gameFinished == true)
                    break;
                //TODO: Change it to 1,9 when we have all the questions.
                int quesitonType = rnd.Next(1, 7); // Random question type between 1-6
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 2, user_email);
            }
        }
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
                int quesitonType = rnd.Next(1, 7); // Random question type between 1-6
                this.questionsList[i] = buildQuestionByTypeAndLevel(quesitonType, 3, user_email);
                // this.questionsList.Add(buildQuestionByTypeAndLevel(quesitonType, 2, user_email));
            }
        }

        private Iquestion buildQuestionByTypeAndLevel(int type,int level,string user_email)
        {
            switch (type)
            {
                case 1:
                    //TODO: Change it to Q1 after Shani will fix the bag.
                    return new Q8(level, user_email);
                case 2:
                    return new Q8(level, user_email);
                case 3:
                    return new Q8(level, user_email);
                case 4:
                    return new Q8(level, user_email);
                case 5:
                    return new Q8(level, user_email);
                case 6:
                    return new Q8(level, user_email);
                case 7:

                    break;
                default:

                    break;
            }
            //TODO: Remove the return null
            return null;
        }
        public void MoveToNextQuestion()
        {
            //Waiting to tje next question calculation
            while (questionsList[currentQuestionNumber + 1] == null) { }
            //TODO :Maybe Problem
            if (currentQuestionNumber >= questionsList.Length)
            {
                string message = "Please wait, the  next question calculation is still in progress.";
                MessageBoxResult result = MessageBox.Show(message, "Who Wants To Be A Millionaire?", MessageBoxButton.OK);
                return;
            }

            CurrentMoney = getMoneyByQuestionNumber(currentQuestionNumber);
            CurrentClue = "";
            currentQuestionNumber++;
            ActivateQuestion((Iquestion)questionsList[currentQuestionNumber]);
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void UpdateSkills(bool rightAnswer)
        {
            ((Iquestion)questionsList[currentQuestionNumber]).updateRelevantSkills(rightAnswer);
        }

        public void finishGame()
        {
            this.gameFinished = true;
            db.insertHighScore(this.user_email, CurrentMoney);
        }

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
                    return 400;
                case 4:
                    return 500;
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
                default :
                    return 1000000;
            }
        }


    }
}
