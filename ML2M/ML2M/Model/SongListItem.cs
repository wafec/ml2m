using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Model
{
    public class SongListItem
    {
        public Song Song { get; set; }
        public bool Selected { get; set; }

        public static SongListItem CreateInstance(Song song)
        {
            SongListItem songListItem = new SongListItem
            {
                Song = song,
                Selected = false
            };
            return songListItem;
        }

        public bool AreSame(SongListItem other)
        {
            if (other == null)
                return false;
            if (Song == null)
                return false;
            if (other.Song == null)
                return false;
            return other.Song.Name == Song.Name && other.Song.Album == Song.Album;
        }
    }
}
