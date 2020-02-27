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

        public string ShortFormattedVerse
        {
            get
            {
                if (Verse != null)
                {
                    int n = 2000;
                    string verse = Verse.Replace("|", " ");
                    if (verse.Length < n)
                        return verse;
                    else
                        return verse.Substring(0, n) + "...";
                }
                return "";
            }
        }

        public string FormattedVerseTip
        {
            get
            {
                if (VerseTip != null)
                    return VerseTip.ToUpperInvariant();
                return "";
            }
        }
    }
}
