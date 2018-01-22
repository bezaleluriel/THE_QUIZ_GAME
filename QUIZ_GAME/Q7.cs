using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    /// <summary>
    /// Class of question type 7, Implements the interface Iquestion.
    /// </summary>
    /// <seealso cref="QUIZ_GAME.Iquestion" />
    class Q7 : Iquestion
    {
        private int Level;
        private string questionBeforeSetting;
        private string question;
        private string User_email;
        private string trueAnswer;
        private string[] wrongAnswers;
        private string clue;
        private DB_Connect db;
        private string firstArtistName;
        private string firstArtistLocation;
        private string answer_artist_id;

        //the location of artist x which the question is about.
        public string FirstArtistLocation
        {
            get { return firstArtistLocation; }
            set
            {
                firstArtistLocation = value;
                this.firstArtistLocation = this.firstArtistLocation.Replace("'", "''");
            }
        }

        //the name of artist x which the question is about.
        public string FirstArtistName
        {
            get { return firstArtistName; }
            set
            {
                firstArtistName = value;
                this.firstArtistName = this.firstArtistName.Replace("'", "''");
            }
        }

        // the artist_id of the artist from the answer.
        public string Answer_artist_id
        {
            get { return answer_artist_id; }
            set
            {
                answer_artist_id = value;
                this.answer_artist_id = this.answer_artist_id.Replace("'", "''");
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
                if(clue!=null)
                {
                    this.clue = this.clue.Replace("'", "''");
                }
            }
        }

        // the answer to this question.
        public string TrueAnswer
        {
            get { return trueAnswer; }
            set
            {
                trueAnswer = value;
                if(trueAnswer != null)
                {
                    this.trueAnswer = this.trueAnswer.Replace("'", "''");
                }
            }
        }

        // the other 3 options that are not correct.
        public string[] WrongAnswers
        {
            get { return wrongAnswers; }
            set
            {
                wrongAnswers = value;
                if(wrongAnswers[0]!=null && wrongAnswers[1] != null && wrongAnswers[2] != null)
                {
                    this.wrongAnswers[0] = this.wrongAnswers[0].Replace("'", "''");
                    this.wrongAnswers[1] = this.wrongAnswers[1].Replace("'", "''");
                    this.wrongAnswers[2] = this.wrongAnswers[2].Replace("'", "''");
                }
            }
        }

        /**
        CONSTRUCTOR  
        **/
        public Q7(int level, string user_email)
        {
            db = new DB_Connect();
            //setting level
            this.Level = level;
            //setting question before we find x.
            this.questionBeforeSetting = "Which artist was born in the same place as the artist X";
            //setting user email.
            this.User_email = user_email;

            //a goto jump point.
            AA:
            // setting the name and location of artist x which the question is about.
            getFirstArtistNameAndLocation();
            // building the question.
            this.Question = buildQuestion();
            // building the wrong answers.
            this.WrongAnswers = buildAnswers();
            // building the true answer.
            this.TrueAnswer = buildTrueAnswer();
            if ((TrueAnswer == null) || (TrueAnswer == FirstArtistName))
            {
                goto AA;
            }            
            // building the clue.
            this.Clue = buildClue();
            if(Clue == null)
            {
                goto AA;
            }
        }

        /**
        buildAnswers - this function builds 3 optional answers that are not true.
        **/
        public string[] buildAnswers()
        {   
            List<string>[] answer1 = null;
            //List<string>[] answer2 = null;
            //List<string>[] answer3 = null;
            // randomley choosing 3 artists for 3 wrong answers.
            answer1 = db.selectQ7("select artist_name from artists order by rand() limit 3;", "3_artist_names");
            //answer2 = db.selectQ7("select artist_name from artists order by rand() limit 1;", "artist_name");
            //answer3 = db.selectQ7("select artist_name from artists order by rand() limit 1;", "artist_name");

            // if one of the querys returns null we return null.
            if (answer1 == null)
            {
                return null;
            }
            else
            {
                // if qeurys are not null we will return an array of size 3 with 3 wrong answers.
                string[] answers = new string[3];
                answers[0] = answer1[0][0];
                answers[1] = answer1[0][1];
                answers[2] = answer1[0][2];
                return answers;
            }
        }

        /**
        buildClue - this function builds a clue, first tries to find one with skills and
        if it can't find one then it creates one without skills.
        **/
        public string buildClue()
        {
            //goto point.
            CC:
            // first we will try to build a clue using the skills.
            // here we try to find an artist from the artist_skills table who was born in the same place and who's rate is > 0
            // and give him as a clue.
            List<string>[] answer = null;
            answer = db.selectQ7("SELECT artist_name FROM artists WHERE artist_id = (SELECT artist_id FROM (SELECT * FROM user_artists_skills NATURAL JOIN artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND user_artists_skills.rate > 0) as A WHERE artist_location = '" + FirstArtistLocation + "' AND artist_name != '" + FirstArtistName + "' AND artist_name != '" + this.TrueAnswer + "' limit 1);", "artist_name");

            if (answer != null)
            {
                if((answer[0][0].Replace("'", "''") == TrueAnswer) || (answer[0][0].Replace("'", "''") == FirstArtistName))
                {
                    goto CC;
                }
                string clue = "The artist " + answer[0][0] + " was born in the same place";
                return clue;
            }
            // if we couldn't build the clue using the skills we build one without using the skills.
            // we choose an artist that was born in the same place and has highesst artist familiarity and give him as a clue.
            else
            {
                answer = db.selectQ7("select artist_name from (select artist_name, max(artist_familiarity) from (select * from artists_locations natural join artists where artists_locations.artist_id = artists.artist_id AND artists_locations.artist_location = '" + FirstArtistLocation + "') as A) as B", "artist_name");
                if(answer != null)
                {
                    if ((answer[0][0].Replace("'", "''") == TrueAnswer) || (answer[0][0].Replace("'", "''") == firstArtistName))
                    {
                        goto CC;
                    }
                    string clue = "The artist " + answer[0][0] + " was born in the same place";
                    return clue;
                }
                return null;
            }
        }

        /**
        buildQuestion - this function builds the question.
        **/
        public string buildQuestion()
        {            
            string question = "Which artist was born in the same place as the artist " + FirstArtistName + "?";
            return question;
        }

        /**
        buildTrueAnswer - this function builds the true answer.
        **/
        public string buildTrueAnswer()
        {
            List<string>[] answer = null;
            // this query finds the number of lines in artist skills table.
            answer = db.selectQ7("SELECT COUNT(rate) from user_artists_skills where user_email = '" + User_email + "'","count");
            // this number is the number of rows in a third of the table.
            int thirdOfTable = Int32.Parse(answer[0][0]) / 3;
            // this number returns the number of rows in two thirds of the table.
            int twoThirdsOfTable = thirdOfTable * 2; 
            answer = null;

            // now we build the answer according to the level.
            switch (Level)
            {
                case 1:
                    // we will try to use the query with the skills - 
                    // we choose an artist name from top third of artist skills table of this user (highest third of rates) 
                    // that was born in the same place as the artist that the question is about.
                    answer = db.selectQ7("select artist_name,artist_id from artists natural join(select * from user_artists_skills natural join artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND user_email = '"+ User_email +"' order by rate desc limit " + thirdOfTable + ") AS B where artist_location = '" + FirstArtistLocation + "' order by rand() limit 1","artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    // if 1st query was not successful we try choosing artist from top 5 (highest rate) of *all users* skill tables
                    // who was born in the same place.
                    answer = db.selectQ7("SELECT artist_name,artist_id from (select * from (select * from user_artists_skills natural join artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND artist_location = '" + FirstArtistLocation + "' ) AS A natural join artists where A.artist_id = artists.artist_id group by artist_name order by rate desc limit 5) AS B ORDER BY RAND() limit 1;", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    // if the queries using skills failed we create one that doesn't user skills.
                    // choose an artist who's artist_familiarity is greater than 0.7
                    answer = db.selectQ7("select artist_name,artist_id from (select * from artists_locations natural join artists) as A where artist_familiarity > 0.7 AND artist_location = '" + FirstArtistLocation + "'  ORDER BY RAND() limit 1;", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    break;
                case 2:
                    // we will try to use the query with the skills - 
                    // we choose an artist name from top third of artist skills table of this user (middle third of rates) 
                    // that was born in the same place as the artist that the question is about.
                    answer = db.selectQ7("select artist_name,artist_id from artists natural join (select * from (select * from user_artists_skills natural join artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND user_email = '" + User_email + "' order by rate desc limit " + twoThirdsOfTable +" )AS B order by rate ASC limit " + thirdOfTable + ") AS C where artist_location = '" + FirstArtistLocation + "' order by rand() limit 1", "artist_name,artist_id");
                    // if we could find an artist using the skills we return the artist name
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    // if 1st query was not successful we try choosing artist from top 10 (highest rate) of *all users* skill tables
                    // who was born in the same place.
                    answer = db.selectQ7("SELECT artist_name,artist_id from (select * from (select * from user_artists_skills natural join artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND artist_location = '" + FirstArtistLocation + "' ) AS A natural join artists where A.artist_id = artists.artist_id group by artist_name order by rate desc limit 10) AS B ORDER BY RAND() limit 1", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    // if the queries using skills failed we create one that doesn't user skills.
                    // choose an artist who's artist_familiarity isbetween 0.4 and 0.7
                    answer = db.selectQ7("select artist_name,artist_id from (select * from artists_locations natural join artists) as A where(artist_familiarity < 0.7 and artist_familiarity > 0.4 and artist_location = '" + FirstArtistLocation + "' ) ORDER BY RAND() limit 1;", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    break;
                case 3:
                    // we will try to use the query with the skills - 
                    // we choose an artist name from top third of artist skills table of this user (bottom third of rates) 
                    // that was born in the same place as the artist that the question is about.
                    answer = db.selectQ7("select artist_name,artist_id from artists natural join (select * from user_artists_skills natural join artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND user_email = '" + User_email + "' order by rate asc limit " + thirdOfTable +") AS B where artist_location = '" + FirstArtistLocation + "' order by rand() limit 1", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    // if 1st query was not successful we try choosing artist from top 20 (highest rate) of *all users* skill tables
                    // who was born in the same place.
                    answer = db.selectQ7("SELECT artist_name,artist_id from (select * from (select * from user_artists_skills natural join artists_locations where user_artists_skills.artist_id = artists_locations.artist_id AND artist_location = '" + FirstArtistLocation + "' ) AS A natural join artists where A.artist_id = artists.artist_id group by artist_name order by rate desc limit 20) AS B ORDER BY RAND() limit 1", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
                        return answer[0][0];
                    }
                    // if the queries using skills failed we create one that doesn't user skills.
                    // choose an artist who's artist_familiarity is < 0.4.
                    answer = db.selectQ7("select artist_name,artist_id from (select * from artists_locations natural join artists) as A where artist_familiarity < 0.4 AND artist_location = '" + firstArtistLocation + "' ORDER BY RAND() limit 1", "artist_name,artist_id");
                    if (answer != null)
                    {
                        answer_artist_id = answer[0][1];
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
        getFirstArtistNameAndLocation - we need to choose the singer X and know his name and location.
        **/
        public void getFirstArtistNameAndLocation()
        {
            List<string>[] answer = null;
            // randomley choose an artist who's familiarity is > 0.7.
            answer = db.selectQ7("SELECT artist_name, artist_location FROM artists natural join artists_locations where artist_familiarity > 0.7 ORDER BY RAND()  limit 1;", "artist_name,artist_location");
            if (answer == null)
            {
                // this query cannot be null.. there are millions of options
            }
            else
            {
                // answer is a 2d array containing the singers name in cell [0][0]
                // and answer[0][1] containing his location.
                FirstArtistName = answer[0][0];
                FirstArtistLocation = answer[0][1];              
            }

        }

        /**
        updates relevant skills in skills tables according to the answer and if user answered correctly.
        **/ 
        public void updateRelevantSkills(bool correctAnswer)
        {
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasLocationsSkill = false, hasArtistsSkill = false;
            string artistID = answer_artist_id;
            string artist_location = FirstArtistLocation;
            hasArtistsSkill = db.CheckSpecificSkill("artists_skills", artistID, this.User_email);
            hasLocationsSkill = db.CheckSpecificSkill("locations_skills", artist_location, this.User_email);

            if (hasLocationsSkill)
                db.UpdateRate("user_locations_skills", this.User_email, "artist_location", artist_location, toAdd);
            else
                db.InsertNewSkill("user_locations_skills", this.User_email, artist_location, toAdd);
            if (hasArtistsSkill)
                db.UpdateRate("user_artists_skills", this.User_email, "artist_id", artistID, toAdd);
            else
                db.InsertNewSkill("user_artists_skills", this.User_email, artistID, toAdd);
        }
    }
}
