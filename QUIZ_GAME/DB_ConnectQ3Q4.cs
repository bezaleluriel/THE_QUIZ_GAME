using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace QUIZ_GAME
{
    class DB_ConnectQ3Q4
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DB_ConnectQ3Q4() { Initialize(); }

        private void Initialize()
        {
            server = "localhost";
            database = "mydb"; //change the name of the db
            uid = "root";
            password = "1234";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                               database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public bool Insert(string name, string password, string email)
        {
            string query = "INSERT INTO users (user_name, user_password, user_email) VALUES('" + name + "','" + password
                           + "','" + email + "')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    //close connection
                    this.CloseConnection();
                    return false;
                }


                //close connection
                this.CloseConnection();
                return true;
            }
            return false;
        }

        //Select statement
        public List<string>[] Select(string email, string password)
        {
            string query = "SELECT user_email, user_password FROM users Where user_email = '" + email
                           + "' AND user_password = '" + password + "'";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["user_email"] + "");
                    list[1].Add(dataReader["user_password"] + "");
                }

                if (dataReader.HasRows == false)
                {
                    list = null;
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<string>[] SelectUserSkills(string user_email, string table_name, Boolean justPositiveRate)
        {
            string query;
            if (!justPositiveRate)
                query = "Select * from " + table_name + " where user_email ='" + user_email + "' order by rate desc;";
            else
                query = "Select * from " + table_name + " where user_email ='" + user_email + "' and rate>0 order by rate desc;";
            //Create a list to store the result
            List<string>[] list = new List<string>[1];
            list[0] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                    list[0].Add(dataReader.GetString(1));
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return null;
            }
        }


        public Song GetSongByID(string songID)
        {
            Song songToReturn = null;
            string query = "Select * from (select * from songs where song_id='" + songID + "') as TheSong natural join artists;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    string song_id = dataReader.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader.GetString(2);
                    songToReturn.Song_name = dataReader.GetString(3);
                    songToReturn.Album_name = dataReader.GetString(4);
                    songToReturn.Duration = dataReader.GetFloat(5);
                    songToReturn.Year = dataReader.GetInt32(6);
                    songToReturn.Artist_id = dataReader.GetString(0);
                    songToReturn.Artist_name = dataReader.GetString(7);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return songToReturn;
        }

        public Song GetRandomSongByArtistID(string artistID)
        {
            Song songToReturn = null;
            string query = "Select * from (select * from songs where artist_id='" + artistID + "') as TheSong natural join artists order by rand() limit 1;";
            MySqlDataReader dataReader;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    string song_id = dataReader.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader.GetString(2);
                    songToReturn.Song_name = dataReader.GetString(3);
                    songToReturn.Album_name = dataReader.GetString(4);
                    songToReturn.Duration = dataReader.GetFloat(5);
                    songToReturn.Year = dataReader.GetInt32(6);
                    songToReturn.Artist_id = dataReader.GetString(0);
                    songToReturn.Artist_name = dataReader.GetString(7);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return songToReturn;
        }

        public string GetRandomArtistID(string userEmail)
        {
            Song songToReturn = null;
            string query;
            string artistId = "";
            //SELECT artist_location FROM mydb.artists_locations WHERE artist_id = 'AR003FB1187B994355';
            //  query = "Select * from user_artists_skills where user_email = " + userEmail + "order by rate desc;";
            query = "Select artist_id from(Select* from user_artists_skills where user_email ='" + userEmail + "' order by rate desc) as bla order by rand() limit 1;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    artistId = dataReader.GetString(0);

                }
                /*
                query = "SELECT artist_location FROM mydb.artists_locations WHERE artist_id = '"+ artistId +"';";
                MySqlCommand cmd2 = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd2.ExecuteReader();
                string artist_location = dataReader.GetString(0);*/
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return artistId;
        }



        /*
         * public Song GetRandomSongByYear(string year,bool popular)
        {
            Song songToReturn = null;
            string query;
            if (popular)
                query = "select * from (select * from songs where year = " + year +") as artistYears natural join (select * from artists where artist_familiarity>0.7)as populars order by rand() limit 1;";
            else
                query = "Select * from (select * from songs where year=" + year + ") as TheSong natural join artists order by rand() limit 1;";

            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    string song_id = dataReader.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader.GetString(2);
                    songToReturn.Song_name = dataReader.GetString(3);
                    songToReturn.Album_name = dataReader.GetString(4);
                    songToReturn.Duration = dataReader.GetFloat(5);
                    songToReturn.Year = dataReader.GetInt32(6);
                    songToReturn.Artist_id = dataReader.GetString(0);
                    songToReturn.Artist_name = dataReader.GetString(7);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return songToReturn;
        }
         */


        public Song GetSongOfArtistByPopularity(double from, double to)
        {

            Song songToReturn = null;
            string song_id = "";
            MySqlDataReader rdr = null;
            //TODO: Maybe this qury isn't good because it returns null sometimes.
            string query = " Select * from (select * from songs where year>0) as one  natural join(Select* from artists where artist_familiarity between " + from.ToString("0.000000") + " and " + to.ToString("0.000000") + " order by rand() limit 1) as two order by rand() limit 1;";
            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    song_id = rdr.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Artist_id = rdr.GetString(0);
                    songToReturn.Version_id = rdr.GetString(2);
                    songToReturn.Song_name = rdr.GetString(3);
                    songToReturn.Album_name = rdr.GetString(4);
                    songToReturn.Duration = rdr.GetFloat(5);
                    songToReturn.Year = rdr.GetInt32(6);
                    songToReturn.Artist_name = rdr.GetString(7);
                }

            }
            else
            {
                return null;
            }
            rdr.Close();
            this.CloseConnection();
            return songToReturn;

        }

        public Song GetRandomTopSongSkillsSong()
        {
            Song songToReturn = null;
            string song_id = "";
            MySqlDataReader rdr = null;
            string query = "Select * from (SELECT song_id,SUM(rate) as AccumulatedRate from user_songs_skills GROUP BY song_id order by AccumulatedRate desc limit 5) as topSongs natural join songs natural join artists order by rand() limit 1;";
            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    song_id = rdr.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = rdr.GetString(3);
                    songToReturn.Song_name = rdr.GetString(4);
                    songToReturn.Album_name = rdr.GetString(5);
                    songToReturn.Duration = rdr.GetFloat(6);
                    songToReturn.Year = rdr.GetInt32(7);
                    songToReturn.Artist_id = rdr.GetString(0);
                    songToReturn.Artist_name = rdr.GetString(8);
                }
            }
            else
            {
                return null;
            }
            rdr.Close();
            this.CloseConnection();
            return songToReturn;
        }

        public Song GetOthersSongSkillsSong(string locationInTable)
        {
            Song songToReturn = null;
            MySqlDataReader dataReader;
            //Create a list to store the result
            List<string>[] songsList = new List<string>[1];
            songsList[0] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                string firstQuery = "SELECT song_id,SUM(rate) as AccumulatedRate from user_songs_skills GROUP BY song_id order by AccumulatedRate desc;";
                //Create Command
                MySqlCommand cmd = new MySqlCommand(firstQuery, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                    songsList[0].Add(dataReader[0] + "");
                int numOfSongs = songsList[0].Count;

                Random rnd = new Random();
                int randomChoose = 0;
                switch (locationInTable)
                {
                    case "TOP":
                        randomChoose = rnd.Next(0, (numOfSongs / 3) + 1);
                        break;
                    case "MIDDLE":
                        randomChoose = rnd.Next(numOfSongs / 3, ((numOfSongs / 3) * 2) + 1);
                        break;
                    case "BOTTOM":
                        randomChoose = rnd.Next((numOfSongs / 3) * 2, numOfSongs + 1);
                        break;
                }
                string song_id = songsList[0].ElementAt(randomChoose);
                string secondQuery = "Select * from (select * from songs where song_id='" + song_id + "') as TheSong natural join artists;";
                cmd = new MySqlCommand(secondQuery, connection);
                dataReader.Close();
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    song_id = dataReader.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader.GetString(2);
                    songToReturn.Song_name = dataReader.GetString(3);
                    songToReturn.Album_name = dataReader.GetString(4);
                    songToReturn.Duration = dataReader.GetFloat(5);
                    songToReturn.Year = dataReader.GetInt32(6);
                    songToReturn.Artist_id = dataReader.GetString(0);
                    songToReturn.Artist_name = dataReader.GetString(7);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return songToReturn;
        }

        public string GetLocationByArtistId(string artistId)
        {
            string loc = "";
            string query;
            //SELECT artist_location FROM mydb.artists_locations WHERE artist_id = 'AR003FB1187B994355';
            //  query = "Select * from user_artists_skills where user_email = " + userEmail + "order by rate desc;";
            query = "SELECT artist_location FROM mydb.artists_locations WHERE artist_id = '" + artistId + "';";

            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    loc = dataReader.GetString(0);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return loc;
        }

        public string getArtistNameById(string artistId)
        {

            string query;
            //SELECT artist_location FROM mydb.artists_locations WHERE artist_id = 'AR003FB1187B994355';
            //  query = "Select * from user_artists_skills where user_email = " + userEmail + "order by rate desc;";
            query = "SELECT artist_name FROM artists WHERE artist_id = '" + artistId + "';";

            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    artistId = dataReader.GetString(0);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return artistId;
        }

        public string GetLocationBySkill(string userEmail, int level)
        {

            string query;
            string artistId = "";
            string loc = "";
            switch (level)
            {
                case 1:
                    query = "Select artist_location from(Select * from user_locations_skills where user_email = '" + userEmail + "' order by rate desc) as bla order by rand() > 0.7 limit 1;";
                    break;
                case 2:
                    query = "Select artist_location from(Select * from user_locations_skills where user_email = '" + userEmail + "' order by rate desc) as bla order by rand() between 0.3 and 0.7 limit 1;";
                    break;
                case 3:
                    query = "Select artist_location from(Select * from user_locations_skills where user_email = '" + userEmail + "' order by rate desc) as bla order by rand() < 0.3 limit 1;";
                    break;
                default:
                    query = "";
                    break;
            }

            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    loc = dataReader.GetString(0);

                }

            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return loc;
        }

        public string GetArtistIdByLocation(string loc)
        {
            string query;
            string artistId = "";
            query = "Select artist_id from artists_locations where artist_location = '" + loc + "' order by rand() limit 1;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    artistId = dataReader.GetString(0);

                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return artistId;
        }

        public string GetUserIDByPopularity(int level)
        {
            string query;
            string artistId = "";


            switch (level)
            {
                case 1:
                    query = "Select artist_id FROM artists WHERE artist_familiarity >= 0.8 order by rand() limit 1;";
                    break;
                case 2:
                    query = "Select artist_id FROM artists WHERE artist_familiarity between 0.5 and 0.7 order by rand() limit 1;";
                    break;
                case 3:
                    query = "Select artist_id FROM artists WHERE artist_familiarity <= 0.4 order by rand() limit 1;";
                    break;
                default:
                    query = "";
                    break;
            }


            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    artistId = dataReader.GetString(0);

                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return artistId;
        }

        public string getLocationByOtherSkills()
        {
            string query;
            string loc = "";
            string artistId = "";
            query = "Select * from user_locations_skills order by rate desc;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    loc = dataReader.GetString(0);

                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return loc;
        }

        public Song GetSongbyjoin(string artistId, string userEmail)
        {

            string query;
            string song_id;
            Song songToReturn = null;
            //SELECT * from (SELECT * from user_songs_skills WHERE user_email = 'abc1@gmail.com' AND  rate > 0 ) as ba natural join (select * from songs where artist_id='ARL752Q1187FB35EFE') as blasongs natural join (select * from artists) as blaa;
            query = "SELECT * from (SELECT * from user_songs_skills WHERE user_email = '" + userEmail + "' AND  rate > 0 ) as ba natural join (select * from songs where artist_id='" + artistId + "') as blasongs natural join (select * from artists) as blaa;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    //song = dataReader.GetString(0);
                    song_id = dataReader.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader.GetString(4);
                    songToReturn.Song_name = dataReader.GetString(6);
                    songToReturn.Album_name = dataReader.GetString(7);
                    songToReturn.Duration = dataReader.GetFloat(7);
                    songToReturn.Year = dataReader.GetInt32(8);
                    songToReturn.Artist_id = dataReader.GetString(0);
                    songToReturn.Artist_name = dataReader.GetString(9);

                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return songToReturn;
        }

        public string getArtistNameByPopAndLoc(string arId, string userEmail, string loc)
        {
            string artName = "";
            string query;
            query = "select artist_name from (SELECT * FROM artists WHERE artist_familiarity > 0.6) as blabla natural join (select * from artists_locations where artist_location = '" + loc + "') as bb order by rand() limit 1;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    artName = dataReader.GetString(0);

                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return artName;
        }

        public List<string>[] getYear(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["year"] + "");
                }

                if (dataReader.HasRows == false)
                {
                    list = null;
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<string>[] getArtistNameByJoin3(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    list[0].Add(dataReader["artist_location"] + "");
                }

                if (dataReader.HasRows == false)
                {
                    list = null;
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<string>[] getArtistNameByJoin4(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    list[0].Add(dataReader["artist_name"] + "");
                }

                if (dataReader.HasRows == false)
                {
                    list = null;
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return null;
            }
        }

        public string GetArtistLocation(string artist_id)
        {
            string artistLocation = null;
            string query = "select artist_location from artists_locations where artist_id='" + artist_id + "';";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    artistLocation = dataReader.GetString(0);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader.Close();
            //close Connection
            this.CloseConnection();
            return artistLocation;
        }
        public bool CheckSpecificSkill(string skillProperty, string skill, string user_email)
        {
            MySqlDataReader dataReader;
            bool skillExist = false;
            string query = "";
            switch (skillProperty)
            {
                case "songs_skills":
                    query = "select rate from user_songs_skills where user_email='" + user_email + "' and song_id ='" + skill + "';";
                    break;
                case "artists_skills":
                    query = "select rate from user_artists_skills where user_email='" + user_email + "' and artist_id ='" + skill + "';";
                    break;
                case "locations_skills":
                    query = "select rate from user_locations_skills where user_email='" + user_email + "' and artist_location ='" + skill + "';";
                    break;
                //Default = Years skills
                default:
                    query = "select rate from user_years_skills where user_email='" + user_email + "' and year =" + skill + ";";
                    break;
            }
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    skillExist = true;
                }
                else
                {
                    skillExist = false;
                }
            }
            else
            {
                return false;
            }
            if (dataReader != null)
                dataReader.Close();

            //close Connection
            this.CloseConnection();
            return skillExist;
        }
        public void UpdateRate(string table, string email, string param, string paramInfo, int toAdd)
        {
            string query = "UPDATE " + table +
                           " SET rate = rate + " + toAdd +
                           " WHERE user_email = '" + email + "' and " + param + " = '" + paramInfo + "'";

            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public bool InsertNewSkill(string table, string email, string param, int toAdd)
        {
            string query = "INSERT INTO " + table + " VALUES('" + email + "','" + param + "'," + toAdd + ")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    //close connection
                    this.CloseConnection();
                    return false;
                }


                //close connection
                this.CloseConnection();
                return true;
            }
            return false;
        }
    }
}
