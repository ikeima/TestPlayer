using System;
using System.Windows;
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

            AudioPlayer.AudioLoad();

            timer.Interval = TimeSpan.FromSeconds(0.1); //установка интервала тикания таймера в 1 секунду
            timer.Tick += timer_tick;
            player.MediaOpened += player_MediaOpened;
        }

        private void player_MediaOpened(object sender, EventArgs e) //событие при загрузке песни
        {
            timeMaxBlock.Text = player.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            slider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
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
            player.Open(new Uri(AudioPlayer.NextSong(), UriKind.RelativeOrAbsolute));
            player.Play();
        }

        private void Start_btn(object sender, RoutedEventArgs e)
        {
            player.Open(new Uri(AudioPlayer.library[0], UriKind.RelativeOrAbsolute)); //загрузка в "плеер" аудио-файла и ее проигрывание
            player.Play();
            timer.Start();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Position = TimeSpan.FromSeconds(slider.Value);
        }


    }
}
