using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    /// <summary>
    /// Class of question type 3, Implements the interface Iquestion.
    /// </summary>
    /// <seealso cref="QUIZ_GAME.Iquestion" />
    class Q3 : Iquestion
    {
        private int Level;
        private string question;
        private string User_email;
        //private string TrueAnswer;
        private string clue;
        // Song songForQuiestion;
        private DB_ConnectQ3Q4 db;
        string ar_id = "";
        string loc = "";

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

        public Q3(int level, string user_email)
        {
            db = new DB_ConnectQ3Q4();
            this.Level = level;
            this.Question = "Where the artist <artist_name> was born?";
            this.User_email = user_email;
            this.Question = buildQuestion();
            this.Clue = buildClue();
            //string[] Answers = buildAnswers();
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
                    return null;
            }
            throw new NotImplementedException();
        }


        public string buildQuestionToFirstLevel()
        {
            string skillParameter = "";


            string art_name = "";
            Random rnd2 = new Random();
            //For mixing the choices between with skills and without.
            int chooseWithSkills = rnd2.Next(100) % 10;

            if (chooseWithSkills < 3)
            {

                if ((skillParameter = getSkill("artist")) != null)
                {
                    DDDD:
                    ar_id = db.GetRandomArtistIDWithLocation(User_email);
                    loc = db.GetLocationByArtistId(ar_id);
                    art_name = db.getArtistNameById(ar_id);
                    TrueAnswer = loc;
                    if ((TrueAnswer == null) || (TrueAnswer == ""))
                    {
                        goto DDDD;
                    }
                }
                else if ((skillParameter = getSkill("location")) != null)
                {
                    EEEE:
                    loc = db.GetLocationBySkill(User_email, Level);
                    ar_id = db.GetArtistIdByLocation(loc);
                    art_name = db.getArtistNameById(ar_id);
                    TrueAnswer = loc;
                    if ((TrueAnswer == null) || (TrueAnswer == ""))
                    {
                        goto EEEE;
                    }
                }
            }
            //If there is no skill - choose question by other random parameter
            if (ar_id == "")
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
                            AAAA:
                            ar_id = db.GetUserIDByPopularity(Level);
                            loc = db.GetLocationByArtistId(ar_id);
                            art_name = db.getArtistNameById(ar_id);
                            TrueAnswer = loc;
                            if ((TrueAnswer == null) || (TrueAnswer == ""))
                            {
                                goto AAAA;
                            }
                            break;
                        case 2:
                            goto AAAA;
                            break;

                    }
                } while (ar_id == null);

            }

            this.Question = this.Question.Replace("<artist_name>", art_name);
            return this.Question;
        }

        public string buildQuestionToSecondLevel()
        {
            string skillParameter = "";
            string ar_id = "";

            string art_name = "";

            Random rnd2 = new Random();
            //For mixing the choices between with skills and without.
            int chooseWithSkills = rnd2.Next(100) % 10;

            if (chooseWithSkills < 3)
            {
                if ((skillParameter = getSkill("artist")) != null)
                {
                    FFFF:
                    ar_id = db.GetRandomArtistID(User_email);
                    loc = db.GetLocationByArtistId(ar_id);
                    art_name = db.getArtistNameById(ar_id);
                    TrueAnswer = loc;
                    if ((TrueAnswer == null) || (TrueAnswer == ""))
                    {
                        goto FFFF;
                    }
                }
                else if ((skillParameter = getSkill("location")) != null)
                {
                    GGGG:
                    loc = db.GetLocationBySkill(User_email, Level);
                    ar_id = db.GetArtistIdByLocation(loc);
                    art_name = db.getArtistNameById(ar_id);
                    TrueAnswer = loc;
                    if ((TrueAnswer == null) || (TrueAnswer == ""))
                    {
                        goto GGGG;
                    }
                }
            }
            //If there is no skill - choose question by other random parameter
            if (ar_id == "")
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
                            AAAA:

                            ar_id = db.GetUserIDByPopularity(Level);
                            loc = db.GetLocationByArtistId(ar_id);
                            art_name = db.getArtistNameById(ar_id);
                            TrueAnswer = loc;
                            if ((TrueAnswer == null) || (TrueAnswer == ""))
                            {
                                goto AAAA;
                            }
                            break;
                        case 2:
                            goto AAAA;
                            break;

                    }
                } while (ar_id == null);

            }

            this.Question = this.Question.Replace("<artist_name>", art_name);
            return this.Question;
        }

        public string buildQuestionToThirdLevel()
        {
            string skillParameter = "";
            string ar_id = "";

            string art_name = "";

            bool checker;
            Random rnd2 = new Random();
            //For mixing the choices between with skills and without.
            int chooseWithSkills = rnd2.Next(100) % 10;

            if (chooseWithSkills < 3)
            {

                if ((skillParameter = getSkill("artist")) != null)
                {
                    BBBB:
                    ar_id = db.GetRandomArtistID(User_email);
                    loc = db.GetLocationByArtistId(ar_id);
                    art_name = db.getArtistNameById(ar_id);
                    TrueAnswer = loc;
                    if ((TrueAnswer == null) || (TrueAnswer == ""))
                    {
                        goto BBBB;
                    }
                }
                else if ((skillParameter = getSkill("location")) != null)
                {
                    CCCC:
                    loc = db.GetLocationBySkill(User_email, Level);
                    ar_id = db.GetArtistIdByLocation(loc);
                    art_name = db.getArtistNameById(ar_id);
                    TrueAnswer = loc;
                    if ((TrueAnswer == null) || (TrueAnswer == ""))
                    {
                        goto CCCC;
                    }

                }
            }
            //If there is no skill - choose question by other random parameter
            if ((ar_id == ""))
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
                            AAAA:
                            ar_id = db.GetUserIDByPopularity(Level);
                            loc = db.GetLocationByArtistId(ar_id);
                            art_name = db.getArtistNameById(ar_id);
                            TrueAnswer = loc;
                            if ((TrueAnswer == null) || (TrueAnswer == ""))
                            {
                                goto AAAA;
                            }
                            break;
                        case 2:
                            goto AAAA;
                            break;

                    }
                } while (ar_id == null);

            }

            this.Question = this.Question.Replace("<artist_name>", art_name);
            return this.Question;
        }

        public string buildClue()
        {
            Song songForClue = null;


            //The place is the same place that the artist Moses was born
            this.Clue = "The place is the same place that the artist <artist_name> was born";

            songForClue = db.GetSongbyjoin(ar_id, User_email);
            if (songForClue != null)
            {
                this.Clue = this.Clue.Replace("<artist_name>", songForClue.Artist_name);
                return this.Clue;
            }
            //without skills
            string artistNameByPopAndLoc = db.getArtistNameByPopAndLoc(ar_id, User_email, loc);
            if (artistNameByPopAndLoc != "")
            {
                this.Clue = this.Clue.Replace("<artist_name>", artistNameByPopAndLoc);
                return this.Clue;
            }

            else if (loc != "")
            {
                Clue = "The first letter is X";
                this.Clue = this.Clue.Replace("X", loc[0].ToString());
                return this.Clue;
            }
            else
            {
                Clue = "NO LOCATION Found";
                return Clue;
            }
        }

        public string[] buildAnswers()
        {
            TrueAnswer = loc;
            string query = "select artist_location from artists_locations where artist_location != '" + loc + "'";
            List<string>[] args = db.getArtistNameByJoin3(query);
            Random rnd = new Random();
            List<string> artistsList = args[0];
            int locationSize = (artistsList).Count - 1;
            int index1 = rnd.Next(0, locationSize / 3);
            int index2 = rnd.Next((locationSize / 3) + 1, (locationSize / 3) * 2);
            int index3 = rnd.Next(((locationSize / 3) * 2) + 1, locationSize);
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
            string artistID = ar_id;
            string artist_location = db.GetArtistLocation(artistID);
            hasArtistsSkill = db.CheckSpecificSkill("artists_skills", artistID, this.User_email);
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
        }
    }
}
