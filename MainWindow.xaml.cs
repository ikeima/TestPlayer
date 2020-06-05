using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer player = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();
        bool isResume = false;
        string filename = null;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1); //установка интервала тикания таймера в 1 секунду
            timer.Tick += timer_tick;
            player.MediaOpened += player_MediaOpened;
            playlist.SelectionChanged += playlist_Selected;
        }

        public void TitleChange()
        {
            foreach (var name in AudioPlayer.myList)
            {
                if (name == playlist.SelectedValue.ToString())
                {
                    titleBlock.Text = name;
                    break;
                }
            }
        }

        private void player_MediaOpened(object sender, EventArgs e) //событие при загрузке песни
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                timeMaxBlock.Text = player.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                slider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
                TitleChange();
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            timeBlock.Text = player.Position.ToString(@"mm\:ss");
            slider.Value = player.Position.TotalSeconds;
        }

        public void Resume_player()
        {
            if (!isResume)
            {
                pauseResumeBtn.Content = "Pause";
                player.Play();
                timer.Start();
                image.Visibility = Visibility.Visible;
                isResume = true;
            }
        }

        public void Pause_player()
        {
            if (isResume)
            {
                pauseResumeBtn.Content = "Resume";
                player.Pause();
                timer.Stop();
                image.Visibility = Visibility.Hidden;
                isResume = false;
            }
        }

        private void Pause_resume_btn(object sender, RoutedEventArgs e)
        {
            if (isResume) Pause_player();
            else Resume_player();
        }

        private void Next_btn(object sender, RoutedEventArgs e)
        {
            filename = AudioPlayer.NextSong();
            player.Open(new Uri(filename, UriKind.RelativeOrAbsolute));

            foreach (var name in AudioPlayer.myList)
            {
                if (filename.Contains(name))
                {
                    titleBlock.Text = name;
                    playlist.SelectedItem = name;
                    break;
                }
            }
            player.Play();
        }

        public void Start_btn(object sender, RoutedEventArgs e)
        {
            player.Play();
            timer.Start();
            image.Visibility = Visibility.Visible;
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
                    image.Visibility = Visibility.Visible;
                    timer.Start();
                    break;
                }
            }


        }


    }
}
