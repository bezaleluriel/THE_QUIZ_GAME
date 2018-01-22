using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{

    /// <summary>
    /// Song Class, Represents a song from the songs table in the database.
    /// </summary>
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

        /// <summary>
        /// Gets or sets the song identifier.
        /// </summary>
        /// <value>
        /// The song identifier.
        /// </value>
        public string Song_id
        {
            get{return song_id;}
            set{song_id = value;}
        }
        /// <summary>
        /// Gets or sets the version identifier.
        /// </summary>
        /// <value>
        /// The version identifier.
        /// </value>
        public string Version_id
        {
            get { return version_id; }
            set { version_id = value; }
        }
        /// <summary>
        /// Gets or sets the name of the song.
        /// </summary>
        /// <value>
        /// The name of the song.
        /// </value>
        public string Song_name
        {
            get { return song_name; }
            set { song_name = value; }
        }
        /// <summary>
        /// Gets or sets the name of the album.
        /// </summary>
        /// <value>
        /// The name of the album.
        /// </value>
        public string Album_name
        {
            get { return album_name; }
            set { album_name = value; }
        }
        /// <summary>
        /// Gets or sets the song duration.
        /// </summary>
        /// <value>
        /// The song duration.
        /// </value>
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The song year.
        /// </value>
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        /// <summary>
        /// Gets or sets the song's artist identifier.
        /// </summary>
        /// <value>
        /// The song's artist identifier.
        /// </value>
        public string Artist_id
        {
            get { return artist_id; }
            set { artist_id = value; }
        }
        /// <summary>
        /// Gets or sets the name of the artist that sang the song.
        /// </summary>
        /// <value>
        /// The name of the artist that sang the song.
        /// </value>
        public string Artist_name
        {
            get { return artist_name; }
            set { artist_name = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="id">The ID of the song.</param>
        public Song(string id)
        {
            this.Song_id = id;
        }
    }
}
