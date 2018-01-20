using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Q2: Iquestion
    {
        //Comment
        private int Level;
        private string question;
        private string User_email;
        private string clue;
        private string[] wrongAnswers;
        private string trueAnswer;
        private DB_Connect db;
        string song = null, album_name = null, artist_name = null, artist_id = null, song_id = null, year = null;
        List<string>[] songSkillTable = null, artistSkillTable = null;

        public string Question
        {
            get { return question; }
            set { question = value; }
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

        public Q2(int level1, string user_email1)
        {
            Level = level1;
            User_email = user_email1;
            db = new DB_Connect();
            Question = buildQuestion();
            string[] answers = buildAnswers();
            Clue = buildClue();
        }

        public string[] buildAnswers()
        {
            TrueAnswer = TrueAnswer.Replace("'", "''");
            string query = "select album_name from songs where album_name != '" + TrueAnswer + "'";
            List<string>[] args = db.getAlbumName(query);
            Random rnd = new Random();
            List<string> albums = args[0];
            int albumsSize = (albums).Count - 1;
            int index1 = rnd.Next(0, albumsSize / 3);
            int index2 = rnd.Next((albumsSize / 3) + 1, (albumsSize / 3) * 2);
            int index3 = rnd.Next(((albumsSize / 3) * 2) + 1, albumsSize);
            string[] answers = { albums.ElementAt(index1), albums.ElementAt(index2), albums.ElementAt(index3) };
            WrongAnswers = answers;
            return answers;
        }

        public void updateRelevantSkills(bool correctAnswer)
        {
            int toAdd = 0;
            if (correctAnswer)
                toAdd = 1;
            else
                toAdd = -1;
            bool hasLocationsSkill = false, hasSongsSkill = false, hasArtistsSkill = false, hasYearsSkill = false;
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


        public string buildClue()
        {
            string query, sameSongName = null;
            List<string>[] song_name = null;
            int i;

            if (isSongSkill())
            {
                Random rnd = new Random();
                int skillSize = (songSkillTable[0]).Count;
                int index = rnd.Next(0, skillSize);
                album_name = album_name.Replace("'", "''");
                for (i = 0; i < skillSize; i++)
                {
                    string s = (songSkillTable[1]).ElementAt(i);
                    if (!s.Equals(song_id))
                    {
                        
                        query = "SELECT song_name from songs WHERE album_name = '" + album_name +
                            "' AND song_id = '" + s + "'";
                        song_name = db.getSongName(query);                        
                    }
                    if (song_name != null) {
                        sameSongName = (song_name[0]).ElementAt(0);
                        break;
                    }
                }               
                
            }
            if(song_name == null)
            {
                query = "SELECT song_name from songs WHERE album_name = '" + album_name + "'";
                song_name = db.getSongName(query);
                i = 0;
                int j = 0;
                int size = (song_name[0]).Count;
                while (i < size)
                {
                    sameSongName = (song_name[0]).ElementAt(i);
                    if (!song.Equals(sameSongName)) {
                        break;
                    }else if(i + 1 == size)
                    {
                        char firstLetter;
                        do
                        {
                            firstLetter = TrueAnswer[j];
                            j++;
                        } while (!((firstLetter >= 65 && firstLetter <= 90) || (firstLetter >= 97 && firstLetter <= 122)));
                        clue = "album name starts with the letter " +firstLetter;
                        return clue;
                    }
                    i++;
                }
            }
            
            clue = "In the same album there is the song " + sameSongName + ".";
            return clue;
        }

        private bool isSongSkill()
        {
            if (songSkillTable == null)
            {
                songSkillTable = db.SelectUserSkills(User_email, "user_songs_skills");
                if (songSkillTable != null) { return true; }
                return false;
            }
            else return true;
        }

        private bool isArtistSkill()
        {
            if (artistSkillTable == null)
            {
                artistSkillTable = db.SelectUserSkills(User_email, "user_artists_skills");
                if (artistSkillTable != null) { return true; }
                return false;
            }
            else return true;
        }

        public string buildQuestion()
        {
            string query;
            List<string>[] args;
            bool isSkill = true;
            string skillTable = null;
            int skillSize = 0;

            Random rnd = new Random();
            //int chooseWithSkills = rnd.Next(100) % 3;
            int chooseWithSkills = 1;

            if (chooseWithSkills == 1)
            {
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
                    else { isSkill = false; }
                }
                if (isSkill)
                {
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
            }
            if(chooseWithSkills != 1 || !isSkill) { getQuestionWithNoSkill(Level); }

            query = "Select artist_name from artists where artist_id = '" + artist_id + "'";
            args = db.getArtistName(query);
            artist_name = (args[0]).ElementAt(0);
            TrueAnswer = album_name;
            question = "What is the album's name which the song " + song + " of the " + artist_name + ", belongs to?";
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
                    query = "select song_name, artist_id, year, album_name from songs where song_id = '" + song_id + "'";
                    args = db.getSongAndArtistIDAndYearAndAlbum(query);
                    song = (args[0]).ElementAt(0);
                    artist_id = (args[1]).ElementAt(0);
                    year = (args[2]).ElementAt(0);
                    album_name = (args[3]).ElementAt(0);
                    break;

                case "artist":
                    artist_id = (artistSkillTable[1]).ElementAt(index);
                    getInfo();
                    break;
            }

        }

        private void getInfo()
        {
            string query = "Select song_name, song_id, year, album_name from songs where artist_id = '" + artist_id + "' order by rand() limit 1";
            List<string>[] args = db.getSongAndSongIDAndYearAndAlbum(query);
            song = (args[0]).ElementAt(0);
            song_id = (args[1]).ElementAt(0);
            year = (args[2]).ElementAt(0);
            album_name = (args[3]).ElementAt(0);
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
            getInfo();
        }

        public string buildTrueAnswer()
        {
            return TrueAnswer;
        }
    }
}

