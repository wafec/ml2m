using ML2M.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ML2M.Controllers
{
    public class SongController : ISongController
    {
        public List<Song> GetSongs(ResourceConfiguration resourceConfiguration)
        {
            var result = new List<Song>();
            foreach (var lyric in resourceConfiguration.GetLyrics())
            {
                string[] lyricLines = File.ReadAllLines(lyric, Encoding.UTF8);
                Song song = new Song();
                List<SongItem> items = new List<SongItem>();
                char? lastUsedKey = null;
                string lastVerseTip = null;
                char pKey = 'A';
                List<char> userDefinedKeys = new List<char>();
                for (int i = 0; i < lyricLines.Length; i++)
                {
                    var lyricLine = lyricLines[i];
                    if (i == 0)
                    {
                        song.Name = lyricLine;
                    }
                    else if (i == 1)
                    {
                        song.Album = lyricLine;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lyricLine))
                        {
                            if (lyricLine.Trim().StartsWith("[") && lyricLine.Trim().EndsWith("]"))
                            {
                                string command = lyricLine.Substring(1, lyricLine.Length - 2);
                                string[] keyVerseTip = command.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (keyVerseTip.Length > 0)
                                {
                                    lastUsedKey = keyVerseTip[0][0];
                                    userDefinedKeys.Add(lastUsedKey.Value);
                                }
                                if (keyVerseTip.Length > 1)
                                    lastVerseTip = keyVerseTip[1];
                                if (keyVerseTip.Length == 0)
                                    song.HasErrors = true;
                            }
                            else
                            {
                                SongItem item = new SongItem();
                                item.Line = i;
                                if (lastUsedKey == null)
                                {
                                    var usedKey = pKey++;
                                    while (userDefinedKeys.Contains((usedKey)))
                                    {
                                        if (pKey >= 'Z')
                                        {
                                            pKey = 'A';
                                            continue;
                                        }
                                        pKey++;
                                    }
                                    lastUsedKey = item.Key = usedKey;
                                } else
                                {
                                    item.Key = lastUsedKey.Value;
                                }
                                item.VerseTip = lastVerseTip ?? "";
                                item.Verse = lyricLine;

                                items.Add(item);
                            }                            
                        } else
                        {
                            lastUsedKey = null;
                            lastVerseTip = null;
                        }
                    }
                }
                song.Items = items;
                Console.WriteLine(string.Format("IsValid: {0}, {1}", song.IsValid(), song.ToString()));
                if (song.IsValid())
                    result.Add(song);
            }
            return result;
        }
    }
}
