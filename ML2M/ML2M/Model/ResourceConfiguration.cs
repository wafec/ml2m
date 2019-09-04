using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ML2M.Model
{
    public class ResourceConfiguration
    {
        public string LyricsPath { get; set; }
        public string VideosPath { get; set; }

        public bool IsLyricsPathValid()
        {
            return Directory.Exists(LyricsPath);
        }

        public bool IsVideosPathValid()
        {
            return Directory.Exists(VideosPath);
        }

        public List<string> GetLyrics()
        {
            var result = new List<string>();
            if (IsLyricsPathValid())
            {
                foreach (var lyric in Directory.GetFiles(LyricsPath))
                {
                    var lyricLines = File.ReadAllLines(lyric, Encoding.Default);
                    if (lyricLines.Length > 3)
                        result.Add(lyric);
                }
            }
            return result;
        }

        public List<string> GetVideos()
        {
            var result = new List<string>();
            if (IsVideosPathValid())
            {
                foreach (var video in Directory.GetFiles(VideosPath))
                {
                    if (new string[] { ".mp4" }.Any(ext => video.EndsWith(ext)))
                        result.Add(video);
                }
            }
            return result;
        }

        private static string GetDefaultSettingsPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
        }

        public void Save()
        {
            string settingsContent = JsonConvert.SerializeObject(this);
            File.WriteAllText(GetDefaultSettingsPath(), settingsContent);
        }

        public static ResourceConfiguration CreateInstance()
        {
            ResourceConfiguration resourceConfiguration = new ResourceConfiguration();
            var settingsPath = GetDefaultSettingsPath();
            if (File.Exists(settingsPath))
            {
                resourceConfiguration = JsonConvert.DeserializeObject<ResourceConfiguration>(File.ReadAllText(settingsPath));
            }
            else
            { 
                resourceConfiguration.LyricsPath = Path.Combine(Directory.GetCurrentDirectory(), "Lyrics");
                resourceConfiguration.VideosPath = Path.Combine(Directory.GetCurrentDirectory(), "Videos");

                resourceConfiguration.Save();
            }
            return resourceConfiguration;
        }
    }
}
