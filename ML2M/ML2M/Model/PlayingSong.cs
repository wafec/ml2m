﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Model
{
    public class PlayingSong
    {
        public Song Song { get; set; }
        public List<SongItem> KeySongItems { get; set; }
        public SongItem CurrentSongItem { get; set; }
        public bool IsZero { get; set; }

        public static PlayingSong CreateInstance(Song song)
        {
            PlayingSong pSong = new PlayingSong
            {
                Song = song,
                KeySongItems = new List<SongItem>(),
                CurrentSongItem = song.Items.First(),
                IsZero = true
            };
            char? lastKey = null;
            foreach (var item in song.Items)
            {
                if (lastKey != item.Key)
                {
                    pSong.KeySongItems.Add(item);
                    lastKey = item.Key;
                }
            }
            return pSong;
        }

        public int GetCurrentItemIndex()
        {
            if (Song != null && CurrentSongItem != null)
            {
                var item = Song.Items.Where(i => i.Line == CurrentSongItem.Line).FirstOrDefault();
                if (item != null)
                    return Song.Items.IndexOf(item);
                return -1;
            }
            else
            {
                return -1;
            }
        }

        public int GetNextKeyIndex(char? requestedKey)
        {
            var index = GetCurrentItemIndex();
            if (index >= 0)
            {
                var key = Song.Items[index].Key;
                var item = Song.Items[index];
                while (item.Key == key && index + 1 < Song.Items.Count)
                {
                    item = Song.Items[++index];
                }
                if (item.Key != key)
                {
                    if (requestedKey != null)
                    {
                        while (item.Key != requestedKey && index + 1< Song.Items.Count)
                        {
                            item = Song.Items[++index];
                        }
                        if (item.Key == requestedKey)
                        {
                            return index;
                        }
                    }
                    else
                    {
                        return index;
                    }                    
                }
                item = Song.Items.FirstOrDefault(i => i.Key == requestedKey);
                if (item != null)
                    return Song.Items.IndexOf(item);
            }
            return -1;
        }

        public void SetCurrentSongItem(int index)
        {
            if (Song != null && Song.Items != null && index < Song.Items.Count && index >= 0)
            {
                CurrentSongItem = Song.Items[index];
            }
        }
    }
}
