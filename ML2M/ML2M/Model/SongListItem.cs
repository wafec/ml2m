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
    }
}
