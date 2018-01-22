using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    /// <summary>
    /// Class of question type 4, Implements the interface Iquestion.
    /// </summary>
    /// <seealso cref="QUIZ_GAME.Iquestion" />
    class Q4 : Iquestion
    {
        private int Level;
        private string question;
        private string User_email;
       // private string TrueAnswer;
        private string clue;
        private Song songForQuiestion;
        private DB_ConnectQ3Q4 db;
        private string artist_id;

       

        public string Clue
        {
            get { return clue; }
            set { clue = value; }
        }
        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public string[] WrongAnswers { get; set; }
        public string TrueAnswer { get; set; }


        public Q4(int level, string user_email)
        {
            db = new DB_ConnectQ3Q4();
            this.Level = level;
            this.Question = "What is the name of the artist that sang the song <song_name> from the <album_name> album?";
            this.User_email = user_email;
            this.Question = buildQuestion();
            this.Clue = buildClue();
            WrongAnswers = buildAnswers();
        }

        //Returning a skill property with regard to the current level.
        public string getSkill(string skillName)
        {
            int numOfRows = 0;
            string table_name;
            List<string>[] answer = null;
            switch (skillName)
            {
                //string query;
                case "artist":
                    table_name = "user_artists_skills";
                    answer = db.SelectUserSkills(this.User_email, table_name, false);
                    break;
                case "song":
                    table_name = "user_songs_skills";
                    answer = db.SelectUserSkills(this.User_email, table_name, false);
                    break;
                case "year":
                    table_name = "user_years_skills";
                    answer = db.SelectUserSkills(this.User_email, table_name, false);
                    break;
                case "location":
                    table_name = "user_locations_skills";
                    answer = db.SelectUserSkills(this.User_email, table_name, false);
                    break;

            }
            if (answer == null)
                return null;
            numOfRows = answer[0].Count;
            //TODO : Add check if enough rows for skill, if not enough - return null
            if (numOfRows == 0)
                return null;

            Random rnd = new Random();
            int choose = 0;
            switch (this.Level)
            {
                case 1:
                    choose = rnd.Next(0, (numOfRows / 3) + 1);
                    break;
                case 2:
                    choose = rnd.Next((numOfRows / 3), ((numOfRows / 3) * 2) + 1);
                    break;
                case 3:
                    choose = rnd.Next((numOfRows / 3) * 2, numOfRows);
                    break;
            }
            return answer[0].ElementAt(choose);
        }

        public string buildQuestion()
        {
            switch (this.Level)
            {
                case 1:
                    return buildQuestionToFirstLevel();
                case 2:
                    return buildQuestionToSecondLevel();
                case 3:
                    return buildQuestionToThirdLevel();
            }
            throw new NotImplementedException();

        }



        public string buildQuestionToFirstLevel()
        {
            bool checker;
            Random rnd2 = new Random();
            //For mixing the choices between with skills and without.
            int chooseWithSkills = rnd2.Next(100) % 10;

            if (chooseWithSkills < 3)
            {



                string skillParameter = "";
                if ((skillParameter = getSkill("song")) != null)
                {
                    this.songForQuiestion = db.GetSongByID(skillParameter);
                }
                else if (((skillParameter = getSkill("artist")) != null) && (songForQuiestion == null))
                {
                    this.songForQuiestion = db.GetRandomSongByArtistID(skillParameter);
                }
            }
            //If there is no skill - choose question by other random parameter
            if (songForQuiestion == null)
            {

                do
                {
                    //The case of no skills
                    Random rnd = new Random();
                    int chooseQuestionMethod = rnd.Next(1, 4); // creates a number between 1 and 3
                    switch (1)
                    {
                        case 1:
                            this.songForQuiestion = this.db.GetSongOfArtistByPopularity(0.8, 1);
                            break;
                        case 2:
                            this.songForQuiestion = this.db.GetRandomTopSongSkillsSong();
                            break;
                        //todo- something with album
                    }
                } while (this.songForQuiestion == null);
            }

            this.Question = this.Question.Replace("<song_name>", this.songForQuiestion.Song_name);
            this.Question = this.Question.Replace("<album_name>", this.songForQuiestion.Album_name);
            TrueAnswer = this.songForQuiestion.Artist_name;
            return this.Question;
        }

        public string buildQuestionToSecondLevel()
        {
            bool checker;
            Random rnd2 = new Random();
            //For mixing the choices between with skills and without.
            int chooseWithSkills = rnd2.Next(100) % 10;

            if (chooseWithSkills < 3)
            {
                string skillParameter = "";
                if ((skillParameter = getSkill("song")) != null)
                {
                    this.songForQuiestion = db.GetSongByID(skillParameter);
                }
                else if (((skillParameter = getSkill("artist")) != null) &&
                         (songForQuiestion == null))
                {
                    this.songForQuiestion =
                        db.GetRandomSongByArtistID(skillParameter);
                }
            }
            //If there is no skill - choose question by other random parameter
            if (songForQuiestion == null)
            {
                do
                {
                    //The case of no skills
                    Random rnd = new Random();
                    int chooseQuestionMethod =
                        rnd.Next(1, 4); // creates a number between 1 and 3
                    switch (1)
                    {
                        case 1:
                            this.songForQuiestion =
                                this.db.GetSongOfArtistByPopularity(0.5, 0.8);
                            break;
                        case 2:
                            this.songForQuiestion = this.db.GetOthersSongSkillsSong("MIDDLE");

                            break;
                    }
                } while (this.songForQuiestion == null);
            }
            this.Question = this.Question.Replace("<song_name>", this.songForQuiestion.Song_name);
            this.Question = this.Question.Replace("<album_name>", this.songForQuiestion.Album_name);
            TrueAnswer = this.songForQuiestion.Artist_name;
            return this.Question;

        }

        public string buildQuestionToThirdLevel()
        {
            bool checker;
            Random rnd2 = new Random();
            //For mixing the choices between with skills and without.
            int chooseWithSkills = rnd2.Next(100) % 10;

            if (chooseWithSkills < 3)
            {
                string skillParameter = "";
                if ((skillParameter = getSkill("song")) != null)
                {
                    this.songForQuiestion = db.GetSongByID(skillParameter);
                }
                else if (((skillParameter = getSkill("artist")) != null) &&
                         (songForQuiestion == null))
                {
                    this.songForQuiestion =
                        db.GetRandomSongByArtistID(skillParameter);
                }
            }
            //If there is no skill - choose question by other random parameter
            if (songForQuiestion == null)
            {
                do
                {
                    //The case of no skills
                    Random rnd = new Random();
                    int chooseQuestionMethod =
                        rnd.Next(1, 4); // creates a number between 1 and 3
                    switch (1)
                    {
                        case 1:
                            this.songForQuiestion =
                                this.db.GetSongOfArtistByPopularity(0.1, 0.5);
                            break;
                        case 2:
                            this.songForQuiestion =
                                this.db.GetOthersSongSkillsSong("BOTTOM");
                            break;
                    }
                } while (this.songForQuiestion == null);
            }
            this.Question = this.Question.Replace("<song_name>", this.songForQuiestion.Song_name);
            this.Question = this.Question.Replace("<album_name>", this.songForQuiestion.Album_name);
            TrueAnswer = this.songForQuiestion.Artist_name;
            return this.Question;
        }

        public string buildClue()
        {

            Song songForClue = null;
            //The same singer sang the song 
            this.Clue = "The same singer sang the song <song_name> ";
            //Build Clue by Artist skill
            //List<string>[] answer = null;
            //answer = db.SelectUserSkills(this.User_email, "user_artists_skills", true);


            //the firsttt clue

            //(SELECT * from user_songs_skills WHERE user_email = "abc1@gmail.com" AND  rate > 0) as blabla to keep all the posirive songs of him
            //select * from (Select * from songs where artist_id = "AR002UA1187B9A637D") as bella where song_id = "TRAFPWS12903CDF737"; when i check one by one all the songs id 
            //this.Clue = this.Clue.Replace("<song_name>", songForClue.Song_name);

            //with skills
            songForClue = db.GetSongbyjoin(songForQuiestion.Artist_id, User_email);
            if (songForClue != null)
            {
                this.Clue = this.Clue.Replace("<song_name>", songForClue.Song_name);
                return this.Clue;
            }

            //without skills
            songForClue = db.GetRandomSongByArtistID(this.songForQuiestion.Artist_id);

            if (songForClue != null)
            {
                this.Clue = this.Clue.Replace("<song_name>", songForClue.Song_name);
                return this.Clue;
            }

            return this.Clue;
        }

        public string[] buildAnswers()
        {
            //select artist_name from artists  order by rand() limit 3;


            string query = "select artist_name from artists where artist_name != '" + TrueAnswer + "'";// order by rand() limit 3;";
            // List<string>[] args = db.getYear(query);
            //string query = "select year from songs where year != '" + TrueAnswer + "'";
            List<string>[] args = db.getArtistNameByJoin4(query);
            Random rnd = new Random();
            List<string> artistsList = args[0];
            int yearsSize = (artistsList).Count - 1;
            int index1 = rnd.Next(0, yearsSize / 3);
            int index2 = rnd.Next((yearsSize / 3) + 1, (yearsSize / 3) * 2);
            int index3 = rnd.Next(((yearsSize / 3) * 2) + 1, yearsSize);
            string[] answers = { artistsList.ElementAt(index1), artistsList.ElementAt(index2), artistsList.ElementAt(index3) };
            return answers;

        }

        public string buildTrueAnswer()
        {
            return this.TrueAnswer;
        }


        public void updateRelevantSkills(bool correctAnswer)
        {
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasLocationsSkill = false, hasSongsSkill = false, hasArtistsSkill = false, hasYearsSkill = false;
            string songID = songForQuiestion.Song_id;
            string artistID = songForQuiestion.Artist_id;
            int songYear = songForQuiestion.Year;
            string artist_location = db.GetArtistLocation(artistID);
            hasSongsSkill = db.CheckSpecificSkill("songs_skills", songID, this.User_email);
            hasArtistsSkill = db.CheckSpecificSkill("artists_skills", artistID, this.User_email);
            hasYearsSkill = db.CheckSpecificSkill("years_skills", songYear.ToString(), this.User_email);
            if (artist_location != null)
            {
                hasLocationsSkill = db.CheckSpecificSkill("locations_skills", artist_location, this.User_email);
                if (hasLocationsSkill)
                    db.UpdateRate("user_locations_skills", this.User_email, "artist_location", artist_location, toAdd);
                else
                    db.InsertNewSkill("user_locations_skills", this.User_email, artist_location, toAdd);
            }
            if (hasSongsSkill)
                db.UpdateRate("user_songs_skills", this.User_email, "song_id", songID, toAdd);
            else
                db.InsertNewSkill("user_songs_skills", this.User_email, songID, toAdd);
            if (hasArtistsSkill)
                db.UpdateRate("user_artists_skills", this.User_email, "artist_id", artistID, toAdd);
            else
                db.InsertNewSkill("user_artists_skills", this.User_email, artistID, toAdd);
            if (hasYearsSkill)
                db.UpdateRate("user_years_skills", this.User_email, "year", songYear.ToString(), toAdd);
            else
                db.InsertNewSkill("user_years_skills", this.User_email, songYear.ToString(), toAdd);
        }

    }
}

