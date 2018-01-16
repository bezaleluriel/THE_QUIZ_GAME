using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Answer
    {
        private string answerString;
        private int position;
        private bool isCorrect;

        public Answer(string answerStr,int pos, bool correct)
        {
            this.answerString = answerStr;
            this.position = pos;
            this.isCorrect = correct;
        }
        public string AnswerString
        {
            get { return answerString; }
            set { answerString = value; }
        }
        public int Position
        {
            get { return position; }
            set { position = value; }
        }
        public bool IsCorrect
        {
            get { return isCorrect; }
            set { isCorrect = value; }
        }
    }
}
