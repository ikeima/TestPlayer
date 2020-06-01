using Microsoft.Win32;
using System;
using System.Windows.Media;

namespace Player
{

    public class AudioPlayer
    {
        public static MediaPlayer player = new MediaPlayer();

        private const int MAX = 5; //максимальная длина массива

        public static string[] library = new string[MAX]; //массив с именами аудиозаписей
        public static string filename = null; //переменная для записи имени одного аудио-файла

        public static void AudioLoad()
        {
            for (int i = 0; i < library.Length; i++) //поочередное заполнение массива
            {
                filename = Add_To_Arr(filename);
                library[i] = filename;
            }
        }

        public static string NextSong() //метод переключения песни на следующую
        {
            Random rand = new Random();
            string audio = library[rand.Next(0, library.Length)]; //вытягивает случайную песню из массива

            return audio; //возвращает ее имя для плеера
        }

        public static string Add_To_Arr(string filename) //метод для выбора аудио-файла
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Audio(MP3)|*.MP3;|All files (ALL)|*.*";
            file.ShowDialog();
            filename = file.FileName;

            return filename;
        }
    }
}
