using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TernVoiceAssistant.Audio
{
    internal class Music
    {
        public static void PlayRandomMusic(string _path, Audio.AudioPlayer _mp3Player)
        {
            string file = null;
            if (!string.IsNullOrEmpty(_path))
            {
                var extensions = new string[] { ".mp3" };
                try
                {
                    var di = new DirectoryInfo(_path);
                    var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                    Random R = new Random();
                    file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
                    _mp3Player.OpenAudioFile(file);
                }
                // probably should only catch specific exceptions
                // throwable by the above methods.
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " - ошибка при работе с файлом:" + _path);
                }
            }
        }
    }  
}
