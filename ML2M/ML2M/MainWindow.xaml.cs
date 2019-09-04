﻿using ML2M.Controllers;
using ML2M.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ML2M
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPresentationSubscriber
    {
        private object _lockObj = new object();

        public IPresentationController PresentationController { get; set; }
        public ISongController SongController { get; set; }
        public ResourceConfiguration ResourceConfiguration { get; set; }
        public SongList SongList { get; set; }
        public SongList SongListView { get; set; }
        public SongList SelectedSongsView { get; set; }
        public PlayingSong PlayingSong { get; set; }
        public PresentationWindow PresentationWindow { get; private set; }
        public bool PresentationFullScreen { get; private set; }
        public bool PresentationHidden { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeThis();
        }

        private void InitializeThis()
        {
            PresentationController = new PresentationController();
            SongController = new SongController();
            ResourceConfiguration = ResourceConfiguration.CreateInstance();

            PresentationController.Subscribe(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SongList = new SongList(SongController.GetSongs(ResourceConfiguration).Select(s => SongListItem.CreateInstance(s)));
            ApplyMusicsToDataGrid();
        }

        private void ShowResults(SongList itemsSource)
        {
            dgMusicas.ItemsSource = itemsSource;
            tbMusicasStatusMsg.Visibility = Visibility.Hidden;
            dgMusicas.Visibility = Visibility.Visible;
        }

        private void HideResults(string msg)
        {
            dgMusicas.ItemsSource = null;
            tbMusicasStatusMsg.Text = msg;
            dgMusicas.Visibility = Visibility.Hidden;
            tbMusicasStatusMsg.Visibility = Visibility.Visible;
        }

        private void ApplyMusicsToDataGrid()
        {
            if (SongList != null && SongList.Count > 0)
            {
                string keywords = tbPesquisar.Text;
                if (string.IsNullOrEmpty(keywords))
                    SongListView = SongList.ToSongList();
                else
                    SongListView = SongList.Where(m => string.Format("{0} {1}", m.Song.Name, m.Song.Album).ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Any(e => keywords.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(e))).ToSongList();
                if (SongListView.Count > 0)
                {
                    ShowResults(SongListView);
                }
                else
                {
                    HideResults("Nenhuma música para a pesquisa feita.");
                }
            }
            else
            {
                HideResults("Não foram encontradas as letras nas configurações atuais.");
            }
        }

        private void HandleBuscarClick(object sender, RoutedEventArgs e)
        {
            ApplyMusicsToDataGrid();
        }

        private void HandleSongListItemCheck(object sender, RoutedEventArgs e)
        {
            UpdateSelectedMusics();
        }

        private void HandleSongListItemUncheck(object sender, RoutedEventArgs e)
        {
            UpdateSelectedMusics();
        }

        private void UpdateSelectedMusics()
        {
            SelectedSongsView = SongList.Where(s => s.Selected).ToSongList();
            lvSelecionadas.ItemsSource = SelectedSongsView;
        }

        private void HandleMostrarMusicaClick(object sender, RoutedEventArgs e)
        {
            object selected = lvSelecionadas.SelectedItem;
            if (selected != null)
            {
                SongListItem songListItem = selected as SongListItem;
                PlayingSong = PlayingSong.CreateInstance(songListItem.Song);                
                dpPlayingSong.DataContext = PlayingSong;
                if (PresentationWindow == null)
                {
                    PresentationWindow = new PresentationWindow(PresentationController, ResourceConfiguration);
                    PresentationWindow.Show();
                }
                PresentationController.ChangeSong(PlayingSong);
            }
        }

        private void HandleTrasClick(object sender, RoutedEventArgs e)
        {
            PresentationController.GoToPrevious();
        }

        private void HandleFrenteClick(object sender, RoutedEventArgs e)
        {
            PresentationController.GoToNext();
        }

        private void HandlePararClick(object sender, RoutedEventArgs e)
        {
            PresentationController.Stop();
        }

        public void HandlePresentationEvent(PresentationEvents presentationEvent, params object[] arg)
        {
            if (presentationEvent == PresentationEvents.Stop)
            {
                PresentationWindow = null;
                PresentationFullScreen = false;
                PresentationHidden = false;
                HandlePresentationWindowButtons();
            }
        }

        private void HandlePresentationWindowButtons()
        {
            if (PresentationHidden)
            {
                bEsconderMostrar.Content = "Mostrar";
            }
            else
            {
                bEsconderMostrar.Content = "Esconder";
            }
            if (!PresentationFullScreen)
            {
                bTelaCheiaNormal.Content = "Tela Cheia";
            }
            else
            {
                bTelaCheiaNormal.Content = "Tela Normal";
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            PresentationController.Stop();
        }

        private void HandleEsconderMostrarClick(object sender, RoutedEventArgs e)
        {
            if (PresentationWindow != null)
            {
                if (PresentationHidden)
                {
                    PresentationWindow.Visibility = Visibility.Visible;
                }
                else
                {
                    PresentationWindow.Visibility = Visibility.Hidden;
                }
                PresentationHidden = !PresentationHidden;
                HandlePresentationWindowButtons();
            }
        }

        private void HandleTelaCheiaNormalClick(object sender, RoutedEventArgs e)
        {
            if (PresentationWindow != null)
            {
                if (!PresentationFullScreen)
                {
                    PresentationWindow.WindowState = WindowState.Maximized;
                    PresentationWindow.WindowStyle = WindowStyle.None;
                }
                else
                {
                    PresentationWindow.WindowState = WindowState.Normal;
                    PresentationWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
                }
                PresentationFullScreen = !PresentationFullScreen;
                HandlePresentationWindowButtons();
            }
        }
    }
}