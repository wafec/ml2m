using ML2M.Controllers;
using ML2M.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ML2M
{
    /// <summary>
    /// Interaction logic for PresentationWindow.xaml
    /// </summary>
    public partial class PresentationWindow : Window, IPresentationSubscriber, INotifyPropertyChanged
    {
        public IPresentationController PresentationController { get; private set; }
        public ResourceConfiguration ResourceConfiguration { get; private set; }
        public PresentationSettings Settings { get; private set; }
        public PlayingSong PlayingSong { get; private set; }
        public Uri MediaSource { get; set; }
        public List<string> CurrentVideoSelection { get; private set; }
        public StateConfiguration StateConfiguration { get; private set; }
        private bool _servicesStopped = false;
        private object _lock = new object();

        public PresentationWindow(IPresentationController presentationController, ResourceConfiguration resourceConfiguration, StateConfiguration stateConfiguration)
        {
            PresentationController = presentationController;
            ResourceConfiguration = resourceConfiguration;
            StateConfiguration = stateConfiguration;
            InitializeSettings();
            InitializeComponent();
            InitializeContexts();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InitializeContexts()
        {
            this.DataContext = this;
            if (PresentationController != null)
                PresentationController.Subscribe(this);
        }

        private void InitializeSettings()
        {
            Settings = new PresentationSettings();
            UpdateFontSize();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePlayingVideo();
            UpdateFontSize();
        }

        private void ShuffleVideoOptions()
        {
            if (!ResourceConfiguration.IsVideosPathValid())
            {
                CurrentVideoSelection = new List<string>();
            }
            else
            {
                var random = new Random();
                var videos = new List<string>(ResourceConfiguration.GetVideos());
                var shuffle = new List<string>();
                while (videos.Count > 0)
                {
                    var videoIndex = random.Next(videos.Count);
                    var video = videos[videoIndex];
                    shuffle.Add(video);
                    videos.RemoveAt(videoIndex);
                }
                CurrentVideoSelection = shuffle;
            }            
        }

        private string GetSelectedVideo()
        {
            if (CurrentVideoSelection == null)
            {
                ShuffleVideoOptions();
            }
            if (CurrentVideoSelection.Count > 0)
            {
                var video = CurrentVideoSelection.First();
                CurrentVideoSelection.RemoveAt(0);
                if (CurrentVideoSelection.Count == 0)
                    CurrentVideoSelection = null;
                return video;
            }
            return "";
        }

        private void InitializePlayingVideo()
        {
            if (ResourceConfiguration.IsVideosPathValid())
            {
                var videos = ResourceConfiguration.GetVideos();
                if (videos != null && videos.Count > 0)
                {
                    var selectedVideo = GetSelectedVideo();
                    MediaSource = new Uri(selectedVideo);
                    PropertyChanged(this, new PropertyChangedEventArgs("MediaSource"));
                    sbPresentation.Begin();
                }
            }
        }

        private void UpdateFontSize()
        {
            Settings.Update(ActualWidth, ActualHeight);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.Update(ActualWidth, ActualHeight);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            Settings.Update(ActualWidth, ActualHeight);
        }

        public void HandlePresentationEvent(PresentationEvents presentationEvent, params object[] arg)
        {
            PlayingSong pArg = null;
            if (arg != null && arg.Length > 0 && arg[0] is PlayingSong)
                pArg = arg[0] as PlayingSong;
            switch (presentationEvent)
            {
                case PresentationEvents.NewSong:
                    PlayingSong = pArg;
                    InitializePlayingVideo();
                    break;
                case PresentationEvents.Stop:
                    StopExecutingEverythingDueClosingEvent();
                    Close();
                    break;
                case PresentationEvents.ChangeBackgroundVideo:
                    InitializePlayingVideo();
                    break;
            }
            if (pArg != null)
            {
                if (!string.IsNullOrEmpty(pArg.CurrentSongItem.VerseTip))
                {
                    Settings.TipVisibility = Visibility.Visible;
                }
                else
                {
                    Settings.TipVisibility = Visibility.Collapsed;
                }
                if (pArg.IsZero)
                {
                    Settings.TitleVisibility = Visibility.Visible;
                    Settings.SlidesVisibility = Visibility.Collapsed;
                }
                else
                {
                    Settings.TitleVisibility = Visibility.Collapsed;
                    Settings.SlidesVisibility = Visibility.Visible;
                }
            }
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PlayingSong"));
        }

        private void StopExecutingEverythingDueClosingEvent()
        {
            lock(_lock)
            {
                if (PresentationController != null && !_servicesStopped)
                {
                    PresentationController.Unsubscribe(this);
                    PresentationController.Stop();
                    sbPresentation.Stop();
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.ThreeDBorderWindow;
                    _servicesStopped = true;
                }
            }            
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            StopExecutingEverythingDueClosingEvent();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            StateConfiguration.PresentationWindowLeft = (int)Left;
            StateConfiguration.PresentationWindowTop = (int)Top;
            StateConfiguration.Save();
        }
    }
}
