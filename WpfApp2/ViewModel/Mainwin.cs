using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.IO;
using WpfApp2.ViewModel.Helpers;

namespace WpfApp2.ViewModel
{
    internal class Mainwin : MainwindowHelpers
    {
        String[] files, paths;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        bool loadedPlaylist = false;
        int trackIndex, songDuration;
        bool ag;
        public Mainwin()
        {
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            try
            {
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show($"The music visualiser file was not found: '{e}'");
            }
            mediaPlayer.Play();
        }
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (ag)
            {
                mediaPlayer.Position = new TimeSpan(0);
            }
            else
            {
                playNextTrack();
            }
        }
        private void ButtonSelectSongs_Click()
        {
            selectSongs();
            mediaPlayer.Play();
        }

        private void playNextTrack()
        {
            if (trackIndex < PlaylistListBox.Items.Count - 1)
            {
                trackIndex += 1;
                mediaPlayer.Open(new Uri(paths[trackIndex]));
                mediaPlayer.Play();
                ButtonNext.IsEnabled = true;
            }
            else
            {
                mediaPlayer.Open(new Uri(paths[0]));
                mediaPlayer.Play();
            }
        }

        private void playPreviousTrack()
        {
            if (trackIndex < PlaylistListBox.Items.Count - 1)
            {
                trackIndex -= 1;
                mediaPlayer.Open(new Uri(paths[trackIndex]));
                mediaPlayer.Play();
                ButtonNext.IsEnabled = true;
            }
        }

        public void selectSongs()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // "C:\\Users\\admin\\Music";
            openFileDialog.Filter = "music files (*.wav)|*.wav|music files (*.mp3)|*.mp3|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                files = openFileDialog.SafeFileNames;
                paths = openFileDialog.FileNames;
                mediaPlayer.Open(new Uri(paths[0]));
                StartMusic();
                for (int i = 0; i < files.Length; i++)
                    PlaylistListBox.Items.Add(files[i]);
                loadedPlaylist = true;
            }
        }
        private void ButtonPrevious_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (trackIndex > 0)
                trackIndex -= 1;

            mediaPlayer.Open(new Uri(paths[trackIndex]));
            mediaPlayer.Play();
        }


        public void ButtonPause_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (loadedPlaylist == true)
            {
                mediaPlayer.Pause();
            }
        }

        public void ButtonPlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (loadedPlaylist == true)
            {
                mediaPlayer.Play();
            }
            else MessageBox.Show("You can add some songs obviously :)");
        }

        private void ButtonForwards_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, (int)(mediaPlayer.Position.Seconds + 10));
            mediaPlayer.Position = ts;
        }

        private void ButtonNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                playNextTrack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MediaPlayerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(MediaPlayerSlider.Value);
            TextBlockCurrentTimeSign.Text = TimeSpan.FromSeconds(MediaPlayerSlider.Value).ToString(@"m\:ss");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan))
            {
                MediaPlayerSlider.Minimum = 0;
                MediaPlayerSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                MediaPlayerSlider.Value = mediaPlayer.Position.TotalSeconds;
                songDuration = (int)(mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds);
                TextBlockSongDuration.Text = TimeSpan.FromSeconds(songDuration).ToString(@"m\:ss");
                if (MediaPlayerSlider.Maximum == MediaPlayerSlider.Value) //if end of the track the media player plays the next track
                    playNextTrack();
            }
        }
        private void PlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StartMusic();
        }

        private void StartMusic()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            mediaPlayer.Play();
            trackIndex = PlaylistListBox.SelectedIndex;
        }

        private void PlaylistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mediaPlayer.Open(new Uri(paths[PlaylistListBox.SelectedIndex]));

            StartMusic();
        }

        private void PlaylistListBox_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            PlaylistListBox.SelectionMode = SelectionMode.Extended;
            selectSongs();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Again1_Click(object sender, RoutedEventArgs e)
        {
            if (!ag)
            {
                Again1.BorderBrush = Brushes.MediumVioletRed;
                ag = true;
            }
            else
            {
                Again1.BorderBrush = Brushes.White;
                ag = false;
            }
        }

        private void ButtonPlay_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) { }
    }
}
