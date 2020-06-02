using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        MediaPlayer player = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            //AudioPlayer.AudioLoad();

            timer.Interval = TimeSpan.FromSeconds(0.1); //установка интервала тикания таймера в 1 секунду
            timer.Tick += timer_tick;
            player.MediaOpened += player_MediaOpened;
            playlist.SelectionChanged += playlist_Selected;
        }

        private void player_MediaOpened(object sender, EventArgs e) //событие при загрузке песни
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                timeMaxBlock.Text = player.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                slider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            }
            
        }

        private void timer_tick(object sender, EventArgs e)
        {
            timeBlock.Text = player.Position.ToString(@"mm\:ss");
            slider.Value = player.Position.TotalSeconds;
        }

        private void Pause_btn(object sender, RoutedEventArgs e)
        {
            player.Pause();
            timer.Stop();
        }

        private void Resume_btn(object sender, RoutedEventArgs e)
        {
            player.Play();
            timer.Start();
        }

        private void Next_btn(object sender, RoutedEventArgs e)
        {
            //player.Open(new Uri(AudioPlayer.NextSong(), UriKind.RelativeOrAbsolute));
            player.Play();
        }

        public void Start_btn(object sender, RoutedEventArgs e)
        {
            player.Play();
            timer.Start();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Pause();
            player.Position = TimeSpan.FromSeconds(slider.Value);
            player.Play();
        }

        private void Select_folder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(fbd.SelectedPath);

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (Path.GetExtension(file.FullName) == ".mp3")
                    {
                        AudioPlayer.AudioLoad(file.FullName);                        
                        AudioPlayer.myList.Add(file.Name);
                    }
                }
                playlist.ItemsSource = AudioPlayer.myList;
            }
        }

        private void playlist_Selected(object sender, RoutedEventArgs e)
        {
            foreach (string file in AudioPlayer.library)
            {
                if (file.Contains(playlist.SelectedValue.ToString()))
                {
                    player.Open(new Uri(file, UriKind.RelativeOrAbsolute));
                    player.Play();
                    timer.Start();
                    break;
                }
            }
            
            
        }

        
    }
}
