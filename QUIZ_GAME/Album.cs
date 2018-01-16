using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    class Album
    {
        private string album_name;
        private string artist_id;
        private string artist_name;
        private int year;

        public Album(string artistID)
        {
            this.Artist_id = artist_id;
        }

        public string Artist_name
        {
            get { return artist_name; }
            set { artist_name = value; }
        }
        public string Album_name
        {
            get { return album_name; }
            set { album_name = value; }
        }
        public string Artist_id
        {
            get { return artist_id; }
            set { artist_id = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
    }
}
