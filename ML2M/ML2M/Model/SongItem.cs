using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Model
{
    public class SongItem
    {
        public char Key { get; set; }
        public string VerseTip { get; set; }
        public string Verse { get; set; }
        public int Line { get; set; }
        public string FormattedVerse
        {
            get
            {
                if (Verse != null)
                    return Verse.Replace("|", "\n");
                return "";
            }
        }
    }
}
