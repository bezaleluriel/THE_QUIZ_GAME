using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Q8 : Iquestion
    {
        private int Level;
        private string questionBeforeSetting;
        private string question;
        private string User_email;
        private string trueAnswer;
        private string[] wrongAnswers;
        private string clue;
        private DB_Connect db;
        string year;
        string answer1, answer2;
        string answer1_song_id = null;
        string answer2_song_id = null;


        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                this.year = this.year.Replace("'", "''");
            }
        }

        public string Answer2
        {
            get { return answer2; }
            set
            {
                answer2 = value;
                this.answer2 = this.answer2.Replace("'", "''");
            }
        }

        public string Answer1
        {
            get { return answer1; }
            set
            {
                answer1 = value;
                this.answer1 = this.answer1.Replace("'", "''");
            }
        }

        // the song_id of the first answer.
        public string Answer1_song_id
        {
            get { return answer1_song_id; }
            set
            {
                answer1_song_id = value;
                this.answer1_song_id = this.answer1_song_id.Replace("'", "''");
            }
        }

        // the song_id of the second answer.
        public string Answer2_song_id
        {
            get { return answer2_song_id; }
            set
            {
                answer2_song_id = value;
                this.answer2_song_id = this.answer2_song_id.Replace("'", "''");
            }
        }

        // the question itself.
        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        // the clue to the question.
        public string Clue
        {
            get { return clue; }
            set
            {
                clue = value;
                this.clue = this.clue.Replace("'", "''");
            }
        }

        // the answer to this question.
        public string TrueAnswer
        {
            get { return trueAnswer; }
            set
            {
                trueAnswer = value;
                this.trueAnswer = this.trueAnswer.Replace("'", "''");
            }
        }

        // the other 3 options that are not correct.
        public string[] WrongAnswers
        {
            get { return wrongAnswers; }
            set
            {
                wrongAnswers = value;
                this.wrongAnswers[0] = this.wrongAnswers[0].Replace("'", "''");
                this.wrongAnswers[1] = this.wrongAnswers[1].Replace("'", "''");
                this.wrongAnswers[2] = this.wrongAnswers[2].Replace("'", "''");
            }
        }

        /**
        CONSTRUCTOR  
        **/
        public Q8(int level, string user_email)
        {
            db = new DB_Connect();
            //setting level
            this.Level = level;
            //setting question before we find x.
            this.questionBeforeSetting = "Which two songs were released in the year X";
            //setting user email.
            this.User_email = user_email;
            BB:
            // selecting a random year on which this question will be based.
            randomly_Select_Year();
            // building the question.
            this.Question = buildQuestion();
            // building the wrong answers.
            this.WrongAnswers = buildAnswers();
            // building the true answer.
            this.TrueAnswer = buildTrueAnswer();
            if (TrueAnswer == null)
            {
                goto BB;
            }
            // building the clue.
            this.clue = buildClue();
            if (clue == null)
            {
                goto BB;
            }
        }

        /**
        buildAnswers - this function builds 3 optional answers that are not true.
        **/
        public string[] buildAnswers()
        {
            List<string>[] answer1 = null;

            // randomley choosing 3 artists for 3 wrong answers.
            answer1 = db.selectQ8("select song_name from songs order by rand() limit 6","song_name");

            // if one of the querys returns null we return null.
            if (answer1 == null)
            {
                return null;
            }
            else
            {
                // if qeurys are not null we will return an array of size 3 with 3 wrong answers.
                string[] answers = new string[3];
                answers[0] = answer1[0][0] + "," + answer1[0][1];
                answers[1] = answer1[0][2] + "," + answer1[0][3]; ;
                answers[2] = answer1[0][4] + "," + answer1[0][5]; ;
                return answers;
            }
        }

        /**
        buildClue - this function builds a clue, first tries to find one with skills and
        if it can't find one then it creates one without skills.
        **/
        public string buildClue()
        {
            // there is no skill that can be used to give a clue..
            // give a clue without skills - give the name of the singer of one of the songs.
            List<string>[] answer = null;
            answer = db.selectQ8("select artist_name from songs natural join artists where song_name = '" + this.answer1 + "'", "artist_name");
            if (answer == null) { return null; }
            string clue = "The artist of one of the two songs is " + answer[0][0];
            return clue;
        }

        /**
        buildQuestion - this function builds the question.
        **/
        public string buildQuestion()
        {
            string question = "Which two songs were released in the year " + this.year;
            return question;
        }

        /**
        buildTrueAnswer - this function builds the true answer.
        **/
        public string buildTrueAnswer()
        {
            //setting the first answer
            this.answer1 = helper();
            //setting the second answer
            this.answer2 = helper();
            //return a final answer made of these 2 answers.
            string result = answer1 + "," + answer2;
            return result;
        }

        /**
        helper - creates one answer
        **/
        public string helper()
        {
            List<string>[] answer = null;
            // this query finds the number of lines in artist skills table.
            answer = db.selectQ7("SELECT COUNT(rate) from user_songs_skills where user_email = '" + User_email + "'", "count");
            // this number is the number of rows in a third of the table.
            int thirdOfTable = Int32.Parse(answer[0][0]) / 3;
            // this number returns the number of rows in two thirds of the table.
            int twoThirdsOfTable = thirdOfTable * 2;
            answer = null;

            // now we build the answer according to the level.
            switch (Level)
            {
                case 1:
                    // we will try to use the query with the skills
                    // we try to choose a song from the top third (highest rated) part of the user_song_skills table 
                    // that was released in the same year as the song in the question.
                    answer = db.selectQ8("select song_name,song_id from (select * from user_songs_skills natural join songs where user_songs_skills.song_id = songs.song_id AND user_email = '" + User_email + "' order by rate desc limit " + thirdOfTable + ") AS B order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    // or if there is no skill for this user we check if there is a song in 5 top rated songs(of other users) in
                    // songs_skill_table where the release year matches the question.                    
                    answer = db.selectQ8("select song_name,song_id from (select * from user_songs_skills natural join songs where user_songs_skills.song_id = songs.song_id and user_email != '" + User_email + "' group by user_email order by rate desc limit 5) as A limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    // choose a song that was released in the same year and has familiarity > 0.6.
                    answer = db.selectQ8("select song_name,song_id from songs natural join artists where songs.artist_id = artists.artist_id AND year = '" + this.year + "' AND artist_familiarity > 0.6 order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    break;
                case 2:
                    // we will try to use the query with the skills
                    // we try to choose a song from the middle third (middle rated) part of the user_song_skills table 
                    // that was released in the same year as the song in the question.
                    answer = db.selectQ8("select song_name,song_id from (select * from (select * from user_songs_skills natural join songs where user_songs_skills.song_id = songs.song_id AND user_email = '" + User_email + "' order by rate desc limit " + twoThirdsOfTable + ")AS B order by rate ASC limit " + thirdOfTable + ") AS C order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    // or if there is no skill for this user we check if there is a song in 10 top rated songs(of other users) in
                    // songs_skill_table where the release year matches the question. 
                    answer = db.selectQ8("select song_name,song_id from (select * from user_songs_skills natural join songs where user_songs_skills.song_id = songs.song_id and user_email != '" + User_email + "' group by user_email order by rate desc limit 10) as A order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    // choose a song that was released in the same year and has familiarity between 0.4 and 0.6.
                    answer = db.selectQ8("select song_name,song_id from songs natural join artists where songs.artist_id = artists.artist_id AND year = '" + this.year + "' AND artist_familiarity < 0.6 AND artist_familiarity > 0.4 order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    break;
                case 3:
                    // we will try to use the query with the skills
                    // we try to choose a song from the bottom third (low rated) part of the user_song_skills table 
                    // that was released in the same year as the song in the question.
                    answer = db.selectQ8("select song_name,song_id from (select * from user_songs_skills natural join songs where user_songs_skills.song_id = songs.song_id AND user_email = '" + User_email + "' order by rate asc limit " + thirdOfTable + ") AS B order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    // or if there is no skill for this user we check if there is a song in 15 top rated songs(of other users) in
                    // songs_skill_table where the release year matches the question. 
                    answer = db.selectQ8("select song_name,song_id from (select * from user_songs_skills natural join songs where user_songs_skills.song_id = songs.song_id and user_email != '" + User_email + "' group by user_email order by rate desc limit 15) as A order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    // choose a song that was released in the same year and has familiarity < 0.4.
                    answer = db.selectQ8("select song_name,song_id from songs natural join artists where songs.artist_id = artists.artist_id AND year = '" + this.year + "' AND artist_familiarity < 0.4 order by rand() limit 1", "song_name,song_id");
                    if (answer != null)
                    {
                        if (answer1_song_id == null)
                        {
                            answer1_song_id = answer[0][1];
                        }
                        else
                        {
                            answer2_song_id = answer[0][1];
                        }
                        return answer[0][0];
                    }
                    break;
            }
            if (answer == null)
            {
                return null;
            }
            else
            {
                return answer[0][0];
            }
        }

        /**
        randomly_Select_Year - randomley selects a year that we will choose two songs from.
        **/
        public void randomly_Select_Year()
        {
            Random rnd = new Random();
            int rand = rnd.Next(1980, 2016);
            this.year = rand.ToString();
        }

        public void updateRelevantSkills(bool correctAnswer)
        {
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasFirstSongsSkill = false, hasSecondSongsSkill = false,hasYearsSkill = false;
            string first_song_ID = this.Answer1_song_id;
            string second_song_ID = this.Answer2_song_id;
            string songYear = this.year;
            hasFirstSongsSkill = db.CheckSpecificSkill("songs_skills", first_song_ID, this.User_email);
            hasFirstSongsSkill = db.CheckSpecificSkill("songs_skills", second_song_ID, this.User_email);
            hasYearsSkill = db.CheckSpecificSkill("years_skills", songYear, this.User_email);
            if (hasFirstSongsSkill)
                db.UpdateRate("user_songs_skills", this.User_email, "song_id", first_song_ID, toAdd);
            else
                db.InsertNewSkill("user_songs_skills", this.User_email, first_song_ID, toAdd);

            if (hasSecondSongsSkill)
                db.UpdateRate("user_songs_skills", this.User_email, "song_id", second_song_ID, toAdd);
            else
                db.InsertNewSkill("user_songs_skills", this.User_email, second_song_ID, toAdd);
            if (hasYearsSkill)
                db.UpdateRate("user_years_skills", this.User_email, "year", songYear, toAdd);
            else
                db.InsertNewSkill("user_years_skills", this.User_email, songYear.ToString(), toAdd);
        }
    }
}
