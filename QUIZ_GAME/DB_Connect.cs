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
            database = "test"; //change the name of the db
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
            string query = "INSERT INTO users (user_name, user_password, user_email) VALUES('" + name + "','" +password
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
                catch(Exception e) {
                    
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
            List<string>[] list = new List<string>[3];//
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

    }

}
