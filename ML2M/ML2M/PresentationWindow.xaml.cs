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

        public PresentationWindow(IPresentationController presentationController, ResourceConfiguration resourceConfiguration)
        {
            PresentationController = presentationController;
            ResourceConfiguration = resourceConfiguration;
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

        private void InitializePlayingVideo()
        {
            if (ResourceConfiguration.IsVideosPathValid())
            {
                var videos = ResourceConfiguration.GetVideos();
                if (videos != null && videos.Count > 0)
                {
                    Random random = new Random();
                    var selectedVideoIndex = random.Next(videos.Count);
                    Console.WriteLine(string.Format("Video: {0}", selectedVideoIndex));
                    var selectedVideo = videos[selectedVideoIndex];
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
                    Close();
                    break;
            }
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PlayingSong"));
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PresentationController != null)
            {
                PresentationController.Unsubscribe(this);
                PresentationController.Stop();
            }
        }
    }
}
