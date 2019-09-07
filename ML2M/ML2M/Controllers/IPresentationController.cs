using ML2M.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ML2M.Controllers
{
    public interface IPresentationController
    {
        void ChangeSong(PlayingSong playingSong);
        void GoToByKey(char key);
        void GoToNext();
        void GoToPrevious();
        void GoToLine(int line);
        void Reset();
        void Stop();
        void Subscribe(IPresentationSubscriber presentationSubscriber);
        void Unsubscribe(IPresentationSubscriber presentationSubscriber);
        SongItem GetCurrentSongItem();
        void SetKeyEvents(FrameworkElement element);
        void ChangeBackgroundVideoRandomly();
        PlayingSong GetSong();
    }
}
