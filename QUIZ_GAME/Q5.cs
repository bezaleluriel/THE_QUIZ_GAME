using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Q5 : Iquestion
    {
        private int Level;
        private string question;
        private string User_email;
        private string trueAnswer;
        private string clue;
        private string[] wrongAnswers;
        private Song songForQuiestion;
        private DB_Connect db;

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
        public string[] WrongAnswers
        {
            get { return wrongAnswers; }
            set { wrongAnswers = value; }
        }
        public string TrueAnswer
        {
            get { return trueAnswer; }
            set { trueAnswer = value; }
        }


        public Q5(int level, string user_email)
        {
            db = new DB_Connect();
            this.Level = level;
            this.Question = "What is the name of the song released by <artist_name> in <year> ?";
            this.User_email = user_email;
            this.Question = buildQuestion();
            this.Clue = buildClue();
            this.buildAnswers();
            buildTrueAnswer();

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
                    answer = db.SelectUserSkills(this.User_email, table_name,false);
                    break;
                case "song":
                    table_name = "user_songs_skills";
                    answer = db.SelectUserSkills(this.User_email, table_name,false);
                    break;
                case "year":
                    table_name = "user_years_skills";
                    answer = db.SelectUserSkills(this.User_email, table_name,false);
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
                    choose = rnd.Next(0, (numOfRows / 3)+1);
                    break;
                case 2:
                    choose = rnd.Next((numOfRows / 3), ((numOfRows / 3)*2)+1);
                    break;
                case 3:
                    choose = rnd.Next((numOfRows / 3) * 2, numOfRows + 1);
                    break;
            }
            if (numOfRows == 1)
                choose = 0;
            return answer[0].ElementAt(choose);
        }


        public string buildClue()
        {
            string songYear = this.songForQuiestion.Year.ToString();
            Song songForClue = null;
             this.Clue = "In this year <artist_name> also released released the song <song_name> ";
            //Build Clue by Artist skill
            List<string>[] answer = null;
            answer = db.SelectUserSkills(this.User_email, "user_artists_skills",true);
            foreach(string artist in answer[0])
            {
                //string artist = (string)o_artist;
                songForClue=db.GetRandomSongByArtistIDAndYear(artist, songYear);
                if (songForClue != null)
                {
                    this.Clue = this.Clue.Replace("<artist_name>", songForClue.Artist_name);
                    this.Clue = this.Clue.Replace("<song_name>", songForClue.Song_name);
                    return this.Clue;
                }
            }
            //If there is no artist with positive rate that released song at this year, build another default clue.
            clue = "The song is from the album <album_name>";
            this.Clue = clue.Replace("<album_name>", this.songForQuiestion.Album_name);
            return this.Clue;
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
            string skillParameter = "";
            if ((skillParameter = getSkill("song")) != null)
            {
                this.songForQuiestion = db.GetSongByID(skillParameter);
            }
            else if (((skillParameter = getSkill("artist")) != null) && (songForQuiestion == null))
            {
                this.songForQuiestion = db.GetRandomSongByArtistID(skillParameter);
            }
            else if (((skillParameter = getSkill("year")) != null) && (songForQuiestion == null))
            {
                this.songForQuiestion = db.GetRandomSongByYear(skillParameter,false);
            }
            //If there is no skill - choose question by other random parameter
            else
            {

                do
                {
                    //The case of no skills
                    Random rnd = new Random();
                    int chooseQuestionMethod = rnd.Next(1, 4); // creates a number between 1 and 3
                    switch (chooseQuestionMethod)
                    {
                        case 1:
                            this.songForQuiestion = this.db.GetSongOfArtistByPopularity(0.8, 1);
                            break;
                        case 2:
                            this.songForQuiestion = this.db.GetRandomTopSongSkillsSong();
                            break;
                        case 3:
                            this.songForQuiestion = this.db.GetRandomTopYearSkillsSong();
                            break;
                    }
                } while (this.songForQuiestion == null);
            }

            this.Question = this.Question.Replace("<artist_name>", this.songForQuiestion.Artist_name);
            this.Question = this.Question.Replace("<year>", this.songForQuiestion.Year.ToString());
            return this.Question;
        }

        public string buildQuestionToSecondLevel()
        {
            string skillParameter = "";
            if ((skillParameter = getSkill("song")) != null)
            {
                this.songForQuiestion = db.GetSongByID(skillParameter);
            }
            else if ( ((skillParameter = getSkill("artist")) != null)&&(songForQuiestion == null))
            {
                this.songForQuiestion = db.GetRandomSongByArtistID(skillParameter);
            }
            else if (((skillParameter = getSkill("year")) != null) && (songForQuiestion == null))
            {
                this.songForQuiestion = db.GetRandomSongByYear(skillParameter,false);
            }
            //If there is no skill - choose question by other random parameter
            else 
            {
                do
                {
                    //The case of no skills
                    Random rnd = new Random();
                    int chooseQuestionMethod = rnd.Next(1, 4); // creates a number between 1 and 3
                    switch (chooseQuestionMethod)
                    {
                        case 1:
                            this.songForQuiestion = this.db.GetSongOfArtistByPopularity(0.5, 0.8);
                            break;
                        case 2:
                            this.songForQuiestion = this.db.GetOthersSongSkillsSong("MIDDLE");
                            break;
                        case 3:
                            this.songForQuiestion = this.db.GetOthersYearSkillsSong("MIDDLE");
                            break;
                    }
                } while (this.songForQuiestion == null);
            }
            this.Question = this.Question.Replace("<artist_name>", this.songForQuiestion.Artist_name);
            this.Question = this.Question.Replace("<year>", this.songForQuiestion.Year.ToString());
            return this.Question;

        }

        public string buildQuestionToThirdLevel()
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
            else if (((skillParameter = getSkill("year")) != null) && (songForQuiestion == null))
            {
                this.songForQuiestion = db.GetRandomSongByYear(skillParameter,false);
            }
            //If there is no skill - choose question by other random parameter
            else
            {
                do
                {
                    //The case of no skills
                    Random rnd = new Random();
                    int chooseQuestionMethod = rnd.Next(1, 4); // creates a number between 1 and 3
                    switch (chooseQuestionMethod)
                    {
                        case 1:
                            this.songForQuiestion = this.db.GetSongOfArtistByPopularity(0.1, 0.5);
                            break;
                        case 2:
                            this.songForQuiestion = this.db.GetOthersSongSkillsSong("BOTTOM");
                            break;
                        case 3:
                            this.songForQuiestion = this.db.GetOthersYearSkillsSong("BOTTOM");
                            break;
                    }
                } while (this.songForQuiestion == null);
            }
            this.Question = this.Question.Replace("<artist_name>", this.songForQuiestion.Artist_name);
            this.Question = this.Question.Replace("<year>", this.songForQuiestion.Year.ToString());
            return this.Question;
        }


        public string buildTrueAnswer()
        {
            TrueAnswer = songForQuiestion.Song_name;
            return TrueAnswer;
        }

        public string[] buildAnswers()
        {
            string[] wrongAns = null;
            do
            {
                wrongAns = db.get3RandomSongs();
            } while (wrongAns == null);
            
            WrongAnswers = wrongAns;
            return wrongAns;
        }
        public void updateRelevantSkills(bool correctAnswer)
        {
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasLocationsSkill=false, hasSongsSkill = false, hasArtistsSkill = false, hasYearsSkill = false;
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
            if(hasSongsSkill)
                db.UpdateRate("user_songs_skills", this.User_email, "song_id", songID, toAdd);
            else
                db.InsertNewSkill("user_songs_skills", this.User_email, songID, toAdd);
            if(hasArtistsSkill)
                db.UpdateRate("user_artists_skills", this.User_email, "artist_id", artistID, toAdd);
            else
                db.InsertNewSkill("user_artists_skills", this.User_email, artistID, toAdd);
            if(hasYearsSkill)
                db.UpdateRate("user_years_skills", this.User_email, "year", songYear.ToString(), toAdd);
            else
                db.InsertNewSkill("user_years_skills", this.User_email, songYear.ToString(), toAdd);
        }

    }
}
