using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUIZ_GAME
{
    /// <summary>
    /// Album Class, Represents an album with it's properties.
    /// </summary>
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
        /// <summary>
        /// Gets or sets the name of the album's artist.
        /// </summary>
        /// <value>
        /// The name of the artist.
        /// </value>
        public string Artist_name
        {
            get { return artist_name; }
            set { artist_name = value; }
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
        /// Gets or sets the artist identifier.
        /// </summary>
        /// <value>
        /// The artist identifier.
        /// </value>
        public string Artist_id
        {
            get { return artist_id; }
            set { artist_id = value; }
        }
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
    }
}
