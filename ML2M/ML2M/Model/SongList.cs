using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Model
{
    public class SongList : List<SongListItem>
    {
        public SongList()
        {

        }

        public SongList(IEnumerable<SongListItem> songs) : base(songs)
        {

        }
    }

    public static class SongListExtensions
    {
        public static SongList ToSongList(this IEnumerable<SongListItem> songListItem)
        {
            return new SongList(songListItem);
        }
    }
}
