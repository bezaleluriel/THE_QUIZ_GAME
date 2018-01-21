using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Q6 : Iquestion
    {
        private int Level;
        private string question;
        private string User_email;
        private string trueAnswer;
        private string[] wrongAnswers;
        private string clue;
        private Album albumForQuestion;
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

        public Q6(int level, string user_email)
        {
            db = new DB_Connect();
            this.Level = level;
            this.Question = "When <artist_name> released the album <album_name> ?";
            this.Clue = "At the same year that the song <song_name> of <artist_name> was released.";
            this.User_email = user_email;
            this.Question = buildQuestion();
            this.Clue = buildClue();
            this.WrongAnswers = buildAnswers();
            this.TrueAnswer = buildTrueAnswer();
            
        }


        public string[] buildAnswers()
        {
            Random rnd = new Random();
            Queue<string> yearsQueue = new Queue<string>();
            do
            {
                int year = rnd.Next(1979, 2018); // creates a number between 1980 to 2017
                //Create 3 answers of unique years that different from the right answer.
                if ((!yearsQueue.Contains(year.ToString())) && (albumForQuestion.Year.ToString() != year.ToString()))
                    yearsQueue.Enqueue(year.ToString());
            }while(yearsQueue.Count < 3);
            WrongAnswers = yearsQueue.ToArray();
            return WrongAnswers;
        }

        public string buildClue()
        {
            if(buildClueBySongsSkills() == true)
            {
                return this.Clue;
            }
            //If there is no compatible clue,taking a song of popular artist that released at the same year and give it as clue.
            else
            {
                Song songForClue = null;
                songForClue = db.GetRandomSongByYear(this.albumForQuestion.Year.ToString(), true);
                if (songForClue != null)
                {
                    this.Clue = this.Clue.Replace("<song_name>", songForClue.Song_name);
                    this.Clue = this.Clue.Replace("<artist_name>", songForClue.Artist_name);
                }
                //TODO : Fix this problem!
                else
                    this.Clue = "PROBLEM WITH CLUE ABOUT SONG FROM" + albumForQuestion.Year.ToString();
                return this.Clue;
            }
            //throw new NotImplementedException();
        }

        //Returning true if the clue built, and false otherwise.
        private bool buildClueBySongsSkills()
        {
            Song songForClue = db.SongFromSongsSkillsByYear(this.User_email, this.albumForQuestion.Year);
            this.Clue = "At the same year that the song <song_name> of <artist_name> was released.";
            if (songForClue != null)
            {
                this.Clue = this.Clue.Replace("<song_name>", songForClue.Song_name);
                this.Clue = this.Clue.Replace("<artist_name>", songForClue.Artist_name);
                return true;
            }
            return false;
            
        }

        public string buildQuestion()
        {
            switch (this.Level)
            {
                case 1:
                    return buildQuestionToLevel(0.8,1,"TOP");
                case 2:
                    return buildQuestionToLevel(0.5, 0.8, "MIDDLE");
                default:
                    return buildQuestionToLevel(0.2, 0.5, "BOTTOM");
            }
        }

        public string buildTrueAnswer()
        {
            this.TrueAnswer = albumForQuestion.Year.ToString();
            return TrueAnswer;
        }

        public string buildQuestionToLevel(double popularFrom, double popularTo,string locationInTable)
        {
            albumIsNull:
            Album album;
            Song songForQuestion;
            if ((album = BuildQuestionByArtistSkills()) != null)
                this.albumForQuestion = album;
            else if ((album = BuildQuestionByYearsSkills()) != null)
                    this.albumForQuestion = album;
            else
            {
                Random rnd = new Random();
                int chooseMethod = rnd.Next(0, 2);
                if(chooseMethod == 1)
                    songForQuestion = db.GetSongOfArtistByPopularity(popularFrom, popularTo);
                else
                     songForQuestion = db.GetOthersYearSkillsSong(locationInTable);
                if (songForQuestion == null)
                    goto albumIsNull;
                album = db.GetSpecificAlbumOfArtist(songForQuestion.Artist_id, songForQuestion.Album_name);
            }
            if (album == null)
                goto albumIsNull;
            this.albumForQuestion = album;
            this.Question = this.Question.Replace("<artist_name>", album.Artist_name);
            this.Question = this.Question.Replace("<album_name>", album.Album_name);
            return this.Question;
        }

        private Album BuildQuestionByArtistSkills()
        {
            Album album = null;
            List<string>[] answer = null;
            answer = db.SelectUserSkills(this.User_email, "user_artists_skills", false);
            List<string> subList = getSubListSkillsByLevel(answer[0]);
            foreach (string artist in subList)
            {
                //string artist = (string)o_artist;
                this.albumForQuestion = db.GetRandAlbumOfArtist(artist);
                if (album != null)
                {
                    return album;
                }
            }
            return null;
        }

        private List<string> getSubListSkillsByLevel(List<string> list)
        {
            int numOfElements = list.Count;

            switch (this.Level){
                case 1:
                    return list.GetRange(0, numOfElements / 3);
                    
                case 2:
                    return list.GetRange(numOfElements / 3, numOfElements / 3);
                    
                default:
                    return list.GetRange((numOfElements / 3)*2, numOfElements / 3);
                    
            }
        }

        private Album BuildQuestionByYearsSkills()
        {
            Album album = null;
            string year = getSkill("year");
            return album;
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
        public void updateRelevantSkills(bool correctAnswer)
        {
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasLocationsSkill = false, hasArtistsSkill = false, hasYearsSkill = false;
            string artistID = albumForQuestion.Artist_id;
            int songYear = albumForQuestion.Year;
            string artist_location = db.GetArtistLocation(artistID);
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
