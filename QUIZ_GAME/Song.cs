using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{


    class Song
    {
        private string song_id;
        private string version_id;
        private string song_name;
        private string album_name;
        private float duration;
        private int year;
        private string artist_id;
        private string artist_name;

        public string Song_id
        {
            get{return song_id;}
            set{song_id = value;}
        }
        public string Version_id
        {
            get { return version_id; }
            set { version_id = value; }
        }
        public string Song_name
        {
            get { return song_name; }
            set { song_name = value; }
        }
        public string Album_name
        {
            get { return album_name; }
            set { album_name = value; }
        }
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public string Artist_id
        {
            get { return artist_id; }
            set { artist_id = value; }
        }
        public string Artist_name
        {
            get { return artist_name; }
            set { artist_name = value; }
        }

        public Song(string id)
        {
            this.Song_id = id;
        }
    }
}
