using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Add MySql Library
using MySql.Data.MySqlClient;
using System.Windows;

namespace QUIZ_GAME
{
    class DB_Connect
    {//

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DB_Connect() { Initialize(); }

        private void Initialize()
        {
            server = "localhost";
            database = "mydb"; //change the name of the db
            uid = "root";
            password = "1234";
            //password = "a1b2c3";
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
            string query = "SELECT * FROM users Where user_email = '" + email
                + "' AND user_password = '" + password + "'";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

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
                    list[2].Add(dataReader["user_name"] + "");

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

        public List<string>[] SelectUserSkills(string user_email, string table_name,Boolean justPositiveRate)
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
        public List<string>[] GetHighScores()
        {
            string query = "SELECT user_name,score FROM high_scores natural join users order by score desc limit 10;";
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {
                    list[0].Add(dataReader.GetString(0));
                    list[1].Add(dataReader.GetInt32(1).ToString());
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

        public Song GetSongOfArtistByPopularity(double from, double to)
        {

            Song songToReturn = null;
            string song_id = "";
            MySqlDataReader rdr = null;
            //TODO: Maybe this qury isn't good because it returns null sometimes.
            //string query = " Select * from (select * from songs where year>0) as one  natural join" +
            //    "(Select* from artists where artist_familiarity between " + from.ToString("0.000000") +
            //    " and " + to.ToString("0.000000") + " order by rand() limit 1) as two order by rand() limit 1;";
            string query = " Select * from " +
                "(Select* from artists where artist_familiarity between "+ from.ToString("0.000000") + " and "+ to.ToString("0.000000") + " order by rand() limit 1) as two" +
                " natural join (select * from songs where year>0) as one  order by rand() limit 1;";
            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    song_id = rdr.GetString(4);
                    songToReturn = new Song(song_id);
                    songToReturn.Artist_id = rdr.GetString(0);
                    songToReturn.Version_id = rdr.GetString(5);
                    songToReturn.Song_name = rdr.GetString(6);
                    songToReturn.Album_name = rdr.GetString(7);
                    songToReturn.Duration = rdr.GetFloat(8);
                    songToReturn.Year = rdr.GetInt32(9);
                    songToReturn.Artist_name = rdr.GetString(1);
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


        public Song GetRandomTopYearSkillsSong()
        {

            Song songToReturn = null;
            string song_id = "";
            MySqlDataReader rdr = null;
            string query = "Select * from (SELECT year,SUM(rate) as AccumulatedRate " +
                "from user_years_skills where year!=0 " +
                "GROUP BY year order by AccumulatedRate desc limit 5 ) as topYears" +
                " natural join (select * from songs where year>0) as two natural join artists" +
                " order by rand() limit 1;";
            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);
                try
                {
                    rdr = cmd.ExecuteReader();
                }catch(Exception e)
                {
                    //rdr.Close();
                    this.CloseConnection();
                    return null;
                }
                
                if (rdr.Read())
                {
                    song_id = rdr.GetString(3);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = rdr.GetString(4);
                    songToReturn.Song_name = rdr.GetString(5);
                    songToReturn.Album_name = rdr.GetString(6);
                    songToReturn.Duration = rdr.GetFloat(7);
                    songToReturn.Year = rdr.GetInt32(1);
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
                        randomChoose = rnd.Next((numOfSongs / 3) * 2, numOfSongs );
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

        public Song GetOthersYearSkillsSong(string locationInTable)
        {
            Song songToReturn = null;
            MySqlDataReader dataReader,dataReader2;
            //Create a list to store the result
            List<string>[] yearsList = new List<string>[1];
            yearsList[0] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                string firstQuery = "SELECT year,SUM(rate) as AccumulatedRate from user_years_skills where year>0 GROUP BY year order by AccumulatedRate desc;";
                //Create Command
                MySqlCommand cmd = new MySqlCommand(firstQuery, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                    yearsList[0].Add(dataReader[0] + "");
                int numOfYears = yearsList[0].Count;
                if (numOfYears == 0) {
                    dataReader.Close();
                    this.CloseConnection();
                    return null;
                }
                    
                Random rnd = new Random();
                int randomChoose = 0;
                switch (locationInTable)
                {
                    case "TOP":
                        randomChoose = rnd.Next(0, (numOfYears / 3) + 1);
                        break;
                    case "MIDDLE":
                        randomChoose = rnd.Next(numOfYears / 3, ((numOfYears / 3) * 2) + 1);
                        break;
                    case "BOTTOM":
                        randomChoose = rnd.Next((numOfYears / 3) * 2, numOfYears);
                        break;
                }
                string year = yearsList[0].ElementAt(randomChoose);
                string secondQuery = "Select * from (select * from songs where year=" + year + ") as TheSong natural join artists order by rand() limit 1;";
                cmd = new MySqlCommand(secondQuery, connection);
                dataReader.Close();
                //Create a data reader and Execute the command
                dataReader2 = cmd.ExecuteReader();
                if (dataReader2.Read())
                {
                    string song_id = dataReader2.GetString(1);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader2.GetString(2);
                    songToReturn.Song_name = dataReader2.GetString(3);
                    songToReturn.Album_name = dataReader2.GetString(4);
                    songToReturn.Duration = dataReader2.GetFloat(5);
                    songToReturn.Year = dataReader2.GetInt32(6);
                    songToReturn.Artist_id = dataReader2.GetString(0);
                    songToReturn.Artist_name = dataReader2.GetString(7);
                }
            }
            else
            {
                return null;
            }
            //close Data Reader
            dataReader2.Close();
            //close Connection
            this.CloseConnection();
            return songToReturn;
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
        public Song GetRandomSongByYear(string year,bool popular)
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

        public Song GetRandomSongByArtistIDAndYear(string artistID,string year)
        {
            Song songToReturn = null;
            string query = "Select * from (select * from songs where artist_id='" + artistID + "' and year =" +year+") as TheSong natural join artists order by rand() limit 1;";
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

        public string[] get3RandomSongs()
        {
            string[] songsToReturn = new string[3];
            string query = "select song_name from songs order by rand() limit 3;";
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
                //while (dataReader.Read())
                //    list[0].Add(dataReader.GetString(1));
                for(int i = 0; i < 3; i++)
                {
                    dataReader.Read();
                    songsToReturn[i] = dataReader.GetString(0);
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
            }
            else
            {
                return null;
            }
            return songsToReturn;
        }

        public Album GetRandAlbumOfArtist(string artistID)
        {
            Album albumToReturn = null;
            string query = "Select * from (select * from albums where artist_id='" + artistID + "') as TheAlbum natural join artists order by rand() limit 1;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    albumToReturn = new Album(artistID);
                    albumToReturn.Artist_name =  dataReader.GetString(3);
                    albumToReturn.Year = int.Parse(dataReader.GetString(2));
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
            return albumToReturn;

        }

        public Album GetRandAlbumOfYear(string year)
        {
            Album albumToReturn = null;
            string query = "Select * from (select * from albums where year='" + year + "') as TheAlbum natural join artists order by rand() limit 1;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    albumToReturn = new Album(dataReader.GetString(0));
                    albumToReturn.Artist_name =  dataReader.GetString(3);
                    albumToReturn.Year = int.Parse(dataReader.GetString(2));
                    albumToReturn.Album_name = dataReader.GetString(1);
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
            return albumToReturn;
        }

        public Album GetSpecificAlbumOfArtist(string artistID,string albumName)
        {
            Album albumToReturn = null;
            albumName = albumName.Replace("'", "''");
            string query = "Select * from (select * from albums where artist_id='" + artistID + "' and album_name ='" + albumName + "') as TheAlbum natural join artists ;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    albumToReturn = new Album(dataReader.GetString(0));
                    albumToReturn.Artist_name = dataReader.GetString(3);
                    albumToReturn.Year = int.Parse(dataReader.GetString(2));
                    albumToReturn.Album_name = dataReader.GetString(1);
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
            return albumToReturn;

        }

        //Returning another song from song skills that released at year
        public Song SongFromSongsSkillsByYear(string userEmail,int year)
        {
            Song songToReturn = null;
            string query = "select * from (Select * from user_songs_skills where user_email='" + userEmail + "' and rate>0) as user natural join (select * from songs where year=" + year.ToString() + ") as years natural join artists order by rand() limit 1;";
            MySqlDataReader dataReader;
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    string song_id = dataReader.GetString(2);
                    songToReturn = new Song(song_id);
                    songToReturn.Version_id = dataReader.GetString(4);
                    songToReturn.Song_name = dataReader.GetString(5);
                    songToReturn.Album_name = dataReader.GetString(6);
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
        public string GetArtistLocation(string artist_id)
        {
            string artistLocation = null;
            string query = "select artist_location from artists_locations where artist_id='" + artist_id+"';";
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
        public bool CheckSpecificSkill(string skillProperty,string skill,string user_email)
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
                    skillExist= true;
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
            if(dataReader!=null)
                dataReader.Close();

            //close Connection
            this.CloseConnection();
            return skillExist;
        }

        ////*****Shani Functions ********///////
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

        public List<string>[] getArtistId(string query)
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
                    list[0].Add(dataReader["artist_id"] + "");
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

        public List<string>[] getSongName(string query)
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
                    list[0].Add(dataReader["song_name"] + "");
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

        public List<string>[] getArtistName(string query)
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
       
     

        public List<string>[] getSongAndSongIDAndYear(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

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
                    list[0].Add(dataReader["song_name"] + "");
                    list[1].Add(dataReader["song_id"] + "");
                    list[2].Add(dataReader["year"] + "");
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

        public List<string>[] getSongAndSongIDAndArtistId(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

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
                    list[0].Add(dataReader["song_name"] + "");
                    list[1].Add(dataReader["song_id"] + "");
                    list[2].Add(dataReader["artist_id"] + "");
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

        public List<string>[] getSongAndSongIDAndYearAndAlbum(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[5];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();

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
                    list[0].Add(dataReader["song_name"] + "");
                    list[1].Add(dataReader["song_id"] + "");
                    list[2].Add(dataReader["year"] + "");
                    list[3].Add(dataReader["album_name"] + "");
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

        public List<string>[] getSongAndArtistIDAndYearAndAlbum(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[5];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();

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
                    list[0].Add(dataReader["song_name"] + "");
                    list[1].Add(dataReader["artist_id"] + "");
                    list[2].Add(dataReader["year"] + "");
                    list[3].Add(dataReader["album_name"] + "");
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

        public List<string>[] getSongYearAndArtistID(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

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
                    list[0].Add(dataReader["song_name"] + "");
                    list[1].Add(dataReader["year"] + "");
                    list[2].Add(dataReader["artist_id"] + "");
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
        public List<string>[] SelectUserSkills(string user_email, string table_name)
        {
            string query = "Select * from " + table_name + " where user_email ='" + user_email + "' order by rate desc;";
            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
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
                    list[0].Add(dataReader[0] + "");
                    list[1].Add(dataReader[1] + "");
                    list[2].Add(dataReader[2] + "");
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
        public List<string>[] getAlbumName(string query)
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
                    list[0].Add(dataReader["album_name"] + "");
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
        /**************MATAN FUNCTIONS********************/
        public List<string>[] selectQ7(string query, string type)
        {
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

                switch (type)
                {
                    case "artist_name,artist_location":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["artist_name"] + "");
                            list[0].Add(dataReader["artist_location"] + "");
                        }
                        break;
                    case "artist_name":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["artist_name"] + "");
                        }
                        break;
                    case "count":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader[0] + "");
                        }
                        break;
                    case "artist_name,artist_id":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["artist_name"] + "");
                            list[0].Add(dataReader["artist_id"] + "");
                        }
                        break;
                    case "3_artist_names":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["artist_name"] + "");
                        }
                        break;
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


        public List<string>[] selectQ8(string query, string type)
        {
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

                switch (type)
                {
                    case "artist_name":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["artist_name"] + "");
                        }
                        break;
                    case "song_name":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["song_name"] + "");
                        }
                        break;
                    case "count":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader[0] + "");
                        }
                        break;
                    case "song_name,song_id":
                        while (dataReader.Read())
                        {
                            list[0].Add(dataReader["song_name"] + "");
                            list[0].Add(dataReader["song_id"] + "");
                        }
                        break;
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
        /*************************************************************************/

        /*************************************************************************/
        public void insertHighScore(string user_email,int score)
        {
            string query = "INSERT INTO high_scores (user_email,score) VALUES('" + user_email + "',"+ score.ToString() +" ); ";

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
                }
                //close connection
                this.CloseConnection();
            }
        }




    }


}
