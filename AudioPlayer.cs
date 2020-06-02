using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Player
{

    public class AudioPlayer
    {
        public static List<string> myList = new List<string>();

        public static MediaPlayer player = new MediaPlayer();

        private const int MAX = 500; //максимальная длина массива

        public static string[] library = new string[MAX]; //массив с полнымы путями аудиозаписей
        public static string filename;

        public static void AudioLoad(string path)
        {
            for (int i = 0; i < library.Length; i++) //поочередное заполнение массива
            {
                if (library[i] == null)
                {
                    library[i] = path;
                    break;
                }
                else continue;
            }
        }

        public static string NextSong() //метод переключения песни на следующую
        {
            Random rand = new Random();
            string filename = "";
            
            while(true)
            {
                filename = library[rand.Next(0, library.Length)]; //вытягивает случайную песню из массива
                if (filename == null)
                    continue;
                else return filename; //возвращает ее имя для плеера
            }
        }

        //public static string Add_To_Arr(string filename) //метод для выбора аудио-файла
        //{
        //    OpenFileDialog file = new OpenFileDialog();
        //    file.Filter = "Audio(MP3)|*.MP3;|All files (ALL)|*.*";
        //    file.ShowDialog();
        //    filename = file.FileName;

        //    return filename;
        //}
    }
}
