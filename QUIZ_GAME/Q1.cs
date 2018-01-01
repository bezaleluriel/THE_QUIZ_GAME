using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Q1 : Iquestion
    {
        private int Level;
        private string Question;
        private string User_email;
        private string TrueAnswer;
        private DB_Connect db;

        public Q1(int level1, string user_email1)
        {
            Level = level1;
            User_email = user_email1;
            db = new DB_Connect();
        }
        public string[] buildAnswers()
        {
            string query;
            switch (Level)
            {
                case 1:
                    //if there is no skill
                    query = "Select artist_id, song_name from artists where artist_familiarity >= 0.8 order by rand() limit 1";
                    List<string>[] args = db.selectQ1(query); 
                    query = "Select * from songs where artist_id =" + (args[0]).ElementAt(0) + "order by rand() limit 1";
                    break;

            }
            return null;
        }

        public string buildClue()
        {
            throw new NotImplementedException();
        }

        public string buildQuestion()
        {
            throw new NotImplementedException();
        }

        public string buildTrueAnswer()
        {
            throw new NotImplementedException();
        }
    }
}
