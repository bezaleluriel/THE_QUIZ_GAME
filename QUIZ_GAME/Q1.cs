using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Q1 : Iquestion
    {
        //Commentnnnnn
        private int Level;
        private string question;
        private string User_email;
        private string clue;
        private string[] wrongAnswers;
        private string trueAnswer;
        private DB_Connect db;
        private string song_id;
        private string song = null, year = null, artist_name = null, artist_id = null;
        private List<string>[] yearSkillTable = null, songSkillTable = null, artistSkillTable = null;

        public string Question
        {
            get {return question; }
            set { question = value;}
        }

        public string Clue
        {
            get { return clue; }
            set { clue = value; }
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

        public Q1(int level1, string user_email1)
        {
            Level = level1;
            User_email = user_email1;
            db = new DB_Connect();
            Question = buildQuestion();
            updateRelevantSkills(true);
            WrongAnswers = buildAnswers();
            Clue = buildClue();
        }

        public void updateRelevantSkills(bool correctAnswer) {
            
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasLocationsSkill = false, hasSongsSkill = false, hasArtistsSkill = false, hasYearsSkill = false;
            //string songID = songForQuiestion.Song_id;
            //string artistID = songForQuiestion.Artist_id;
            //int songYear = songForQuiestion.Year;
            string artist_location = db.GetArtistLocation(artist_id);
            hasSongsSkill = db.CheckSpecificSkill("songs_skills", song_id, this.User_email);
            hasArtistsSkill = db.CheckSpecificSkill("artists_skills", artist_id, this.User_email);
            hasYearsSkill = db.CheckSpecificSkill("years_skills", year.ToString(), this.User_email);
            if (artist_location != null)
            {
                hasLocationsSkill = db.CheckSpecificSkill("locations_skills", artist_location, this.User_email);
                if (hasLocationsSkill)
                    db.UpdateRate("user_locations_skills", this.User_email, "artist_location", artist_location, toAdd);
                else
                    db.InsertNewSkill("user_locations_skills", this.User_email, artist_location, toAdd);
            }
            if (hasSongsSkill)
                db.UpdateRate("user_songs_skills", this.User_email, "song_id", song_id, toAdd);
            else
                db.InsertNewSkill("user_songs_skills", this.User_email, song_id, toAdd);
            if (hasArtistsSkill)
                db.UpdateRate("user_artists_skills", this.User_email, "artist_id", artist_id, toAdd);
            else
                db.InsertNewSkill("user_artists_skills", this.User_email, artist_id, toAdd);
            if (hasYearsSkill)
                db.UpdateRate("user_years_skills", this.User_email, "year", year.ToString(), toAdd);
            else
                db.InsertNewSkill("user_years_skills", this.User_email, year.ToString(), toAdd);
        
        }

        public string[] buildAnswers()
        {
            string query = "select year from songs where year != '" + TrueAnswer + "'";
            List<string>[] args = db.getYear(query);
            Random rnd = new Random();
            List<string> years = args[0];
            int yearsSize = (years).Count - 1;
            int index1 = rnd.Next(0, yearsSize/3);
            int index2 = rnd.Next((yearsSize / 3) + 1, (yearsSize/3) * 2);
            int index3 = rnd.Next(((yearsSize / 3) * 2) + 1, yearsSize);
            string[] answers = { years.ElementAt(index1), years.ElementAt(index2), years.ElementAt(index3) };
            return answers;
        }

        public string buildClue()
        {
            string query, localArtistId = null, sameYear = null;
            List<string>[] artists;
            List<string>[] years;
            List<string>[] args;
            bool stop = false;

            if (isArtistSkill())
            {
                query = "Select artist_id from user_artists_skills where user_email ='" + User_email +
                    "' and  rate > 0";
                artists = db.getArtistId(query);
                for (int i = 0; i < (artists[0]).Count; i++){
                    if (!stop)
                    {
                        localArtistId = (artists[0]).ElementAt(i);
                        query = "Select year from songs where artist_id ='" + localArtistId + "'";
                        years = db.getYear(query); //get all years this artist outed a new song.
                        for (int j = 0; j < (years[0]).Count; j++)
                        {
                            if ((years[0]).ElementAt(j) == year)
                            {
                                stop = true;
                                sameYear = (years[0]).ElementAt(j);
                                break;
                            }
                        }
                    }
                    if (stop) {
                        query = "select song_name from songs where artist_id = '" + localArtistId + "'";
                        args = db.getSongName(query);
                        string localSongName = (args[0]).ElementAt(0);
                        query = "select artist_name from artists where artist_id = '" + localArtistId + "'";
                        args = db.getArtistName(query);
                        string localArtistName = (args[0]).ElementAt(0);
                        clue = "The same year the song " + localSongName + " of artist " + localArtistName + " came out.";
                        break;
                    }
                }
                
            }
            if(stop == false)
            {
                query = "select song_name from songs where year = '" + year + "'";
                args = db.getSongName(query);
                clue = "At the same year the songs '" + (args[0]).ElementAt(0) + "', " +
                    (args[0]).ElementAt(1) + "' and " + (args[0]).ElementAt(2) + "' came out.";

            }
            
            return clue;
        }

        private bool isSongSkill() {
            songSkillTable = db.SelectUserSkills(User_email, "user_songs_skills");
            if (songSkillTable != null){ return true; }
            return false;
        }

        private bool isArtistSkill() {
            artistSkillTable = db.SelectUserSkills(User_email, "user_artists_skills");
            if (artistSkillTable != null) { return true; }
            return false;
        }

        private bool isYearSkill() {
            yearSkillTable = db.SelectUserSkills(User_email, "user_years_skills");
            if (yearSkillTable != null) { return true; }
            return false;
        }


        public string buildQuestion()
        {
            string query;
            List<string>[] args;
            bool isSkill = true;
            string skillTable = null;
            int skillSize = 0;

            
            if (isSongSkill() == true)
            {
                skillTable = "songs";
                skillSize = (songSkillTable[0]).Count - 1;
            }
            else
            {                
                if (isArtistSkill() == true)
                {
                    skillTable = "artist";
                    skillSize = (artistSkillTable[0]).Count - 1;
                }
                else
                {
                    if (isYearSkill() == true)
                    {
                        skillTable = "year";
                        skillSize = (yearSkillTable[0]).Count - 1;
                    }
                    else { isSkill = false; }
                }
            }

            if (!isSkill) { getQuestionWithNoSkill(Level); }
            else
            {
                Random rnd = new Random();
                switch (Level)
                {
                    case 1:
                        int index = rnd.Next(0, skillSize / 3);
                        getQuestionWithSkill(skillTable, index);
                        break;

                    case 2:
                        index = rnd.Next(skillSize / 3, (skillSize / 3) * 2);
                        getQuestionWithSkill(skillTable, index);
                        break;

                    case 3:
                        index = rnd.Next((skillSize / 3) * 2, skillSize);
                        getQuestionWithSkill(skillTable, index);
                        break;
                }
            }

            query = "Select artist_name from artists where artist_id = '" + artist_id + "'";
            args = db.getArtistName(query);
            artist_name = (args[0]).ElementAt(0);
            TrueAnswer = year;
            question = "In which year the song " + song + " of artist " + artist_name + " came out?";
            return question;
        }  
        
        private void getQuestionWithSkill(string skillTable, int index)
        {
            string query;
            List<string>[] args;

            switch (skillTable)
            {
                case "songs":
                    
                    song_id = (songSkillTable[1]).ElementAt(index);
                    query = "select artist_id, song_name, year from songs where song_id = '" + song_id + "'";
                    args = db.getSongYearAndArtistID(query);
                    song = (args[0]).ElementAt(0);
                    year = (args[1]).ElementAt(0);
                    artist_id = (args[2]).ElementAt(0);
                    break;

                case "artist":
                    artist_id = (artistSkillTable[1]).ElementAt(index);
                    query = "Select song_name, song_id, year from songs where artist_id = '" + artist_id + "' order by rand() limit 1";
                    args = db.getSongAndSongIDAndYear(query);
                    song = (args[0]).ElementAt(0);
                    song_id = (args[1]).ElementAt(0);
                    year = (args[2]).ElementAt(0);
                    break;

                case "year":
                    year = (yearSkillTable[1]).ElementAt(index);
                    query = "select song_name, song_id, artist_id from songs where year = '" + year + "' order by rand() limit 1";
                    args = db.getSongAndSongIDAndArtistId(query);
                    song = (args[0]).ElementAt(0);
                    song_id = (args[1]).ElementAt(0);
                    artist_id = (args[2]).ElementAt(0);
                    break;
            }

        }

        private void getQuestionWithNoSkill(int level)
        {
            string query;
            List<string>[] args;

            switch (Level)
            {
                case 1:
                    query = "Select artist_id from artists where artist_familiarity >= 0.8 order by rand() limit 1";
                    args = db.getArtistId(query);
                    artist_id = (args[0]).ElementAt(0);
                    break;

                case 2:
                    query = "Select artist_id from artists where artist_familiarity between 0.5 and 0.8 order by rand() limit 1";
                    args = db.getArtistId(query);
                    artist_id = (args[0]).ElementAt(0);
                    break;

                case 3:
                    query = "Select artist_id from artists where artist_familiarity >= 0.3 order by rand() limit 1";
                    args = db.getArtistId(query);
                    artist_id = (args[0]).ElementAt(0);
                    break;
            }
            query = "Select song_name, song_id, year from songs where artist_id = '" + artist_id + "' order by rand() limit 1";
            args = db.getSongAndSongIDAndYear(query);
            song = (args[0]).ElementAt(0);
            song_id = (args[1]).ElementAt(0);
            year = (args[2]).ElementAt(0);
        }

        public string buildTrueAnswer()
        {
            return null;
        }
    }  


}
