using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ML2M.Model;

namespace ML2M.Controllers
{
    public class PresentationController : IPresentationController
    {
        public PlayingSong PlayingSong { get; private set; }
        public List<IPresentationSubscriber> Subscribers { get; private set; }

        public PresentationController()
        {
            Subscribers = new List<IPresentationSubscriber>();
        }

        public void ChangeSong(PlayingSong playingSong)
        {
            PlayingSong = playingSong;
            SendEvent(PresentationEvents.NewSong, PlayingSong);
        }

        public SongItem GetCurrentSongItem()
        {
            if (PlayingSong != null)
                return PlayingSong.CurrentSongItem;
            return null;
        }

        public void GoToByKey(char key)
        {
            if (PlayingSong != null)
            {
                var index = PlayingSong.GetNextKeyIndex(key);
                if (index >= 0)
                {
                    PlayingSong.SetCurrentSongItem(index);
                    SendEvent(PresentationEvents.NextKey, PlayingSong);
                }
            }
        }

        public void GoToLine(int line)
        {
            if (PlayingSong != null && PlayingSong.Song != null && PlayingSong.Song.Items != null)
            {
                var item = PlayingSong.Song.Items.FirstOrDefault(i => i.Line == line);
                if (item != null)
                    PlayingSong.CurrentSongItem = item;
                SendEvent(PresentationEvents.NextLine, PlayingSong);
            }
        }

        public void GoToNext()
        {
            if (PlayingSong != null && PlayingSong.Song.Items != null)
            {
                var index = PlayingSong.GetCurrentItemIndex();
                if (index + 1 < PlayingSong.Song.Items.Count)
                    PlayingSong.CurrentSongItem = PlayingSong.Song.Items[index + 1];
                SendEvent(PresentationEvents.Next, PlayingSong);
            }
        }

        public void GoToPrevious()
        {
            if (PlayingSong != null && PlayingSong.Song != null && PlayingSong.Song.Items != null)
            {
                var index = PlayingSong.GetCurrentItemIndex();
                if (index - 1 >= 0)
                    PlayingSong.CurrentSongItem = PlayingSong.Song.Items[index - 1];
                SendEvent(PresentationEvents.Previous, PlayingSong);
            }
        }

        public void Reset()
        {
            if (PlayingSong != null)
            {
                PlayingSong.CurrentSongItem = null;
                SendEvent(PresentationEvents.Reset, PlayingSong);
            }
        }

        public void Stop()
        {
            PlayingSong = null;
            SendEvent(PresentationEvents.Stop, PlayingSong);
        }

        private void SendEvent(PresentationEvents presentationEvent, params object[] arg)
        {
            foreach (var subscriber in Subscribers)
                subscriber.HandlePresentationEvent(presentationEvent, arg);
        }

        public void Subscribe(IPresentationSubscriber presentationSubscriber)
        {
            if (!Subscribers.Contains(presentationSubscriber))
                Subscribers.Add(presentationSubscriber);
        }

        public void Unsubscribe(IPresentationSubscriber presentationSubscriber)
        {
            if (Subscribers.Contains(presentationSubscriber))
                Subscribers.Remove(presentationSubscriber);
        }

        public void SetKeyEvents(FrameworkElement element)
        {
            element.KeyDown += new System.Windows.Input.KeyEventHandler(HandleKeyDown);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.None)
            {
                if (e.Key == Key.F)
                    GoToNext();
                else if (e.Key == Key.T)
                    GoToPrevious();
                else if (e.Key == Key.S)
                    Stop();
            }
            else
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    GoToByKey(Enum.GetName(typeof(Key), e.Key)[0]);
                }
            }
        }

        public void ChangeBackgroundVideoRandomly()
        {
            SendEvent(PresentationEvents.ChangeBackgroundVideo);
        }
    }
}
