using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TernVoiceAssistant.Audio
{
    public class AudioPlayer
    {
        private static string _audioPath = @"E:\Программирование\МузыкаДляТерна\";

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutGetVolume(IntPtr hwo, out uint pdwVolume);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern int waveOutSetVolume(uint uDeviceID, uint dwVolume);
        //dwVolume новая громкость - Значение 0xFFFF FFFF соответствует полной громкости, а значение 0x0000 0000 — тишине ||| FFFF - 65535

        private static uint hWO = 0;
        public static uint HWO
        {
            get => hWO;
            set => hWO = value;
        }

        private static uint earpiece = 0x00000000;
        public static uint Earpiece
        {
            get => earpiece;
            set => earpiece = value;  
        }

        private static uint leftEarpiece = 0x0000;
        public static uint LeftEarpiece 
        {
            get => leftEarpiece;
            set => leftEarpiece = value; 
        }

        private static uint rightEarpiece = 0x0000;
        public static uint RightEarpiece
        {
            get => rightEarpiece;
            set => rightEarpiece = value;  
        }

        /// <summary>
        /// Метод соединяющий громкости для правого и левого наушника
        /// </summary>
        /// <param name="leftEar">Громкость звука для левого наушника 0 - 100 (withConvert = true) или 0 - 65535, 0x0000 - 0xffff (withConvert = false)</param>
        /// <param name="rightEar">Громкость звука для правого наушника  0 - 65535 или 0x0000 - 0xffff</param>
        /// <param name="withConvert">Проверка, введено число от 0 до 100 и нужно ли преобразовывать их или нет</param>
        static public void SetTheVolume(int leftEar, int rightEar, bool withConvert = false)
        {
            if (withConvert == true) 
            {
                LeftEarpiece = (uint)(65535.0 / 100.0 * leftEar);
                RightEarpiece = (uint)(65535.0 / 100.0 * rightEar);
                Earpiece = (LeftEarpiece << 16) | RightEarpiece;
            }
            else
                Earpiece = ((uint)leftEar << 16) | (uint)rightEar;
            
            waveOutSetVolume(HWO, Earpiece);
        }
        static public void SetTheVolume(uint leftEar, uint rightEar)
        {
            LeftEarpiece = leftEar;
            RightEarpiece = rightEar;
            Earpiece = (leftEar << 16) | rightEar;
            waveOutSetVolume(HWO, Earpiece);
        }
        /// <summary>
        /// Метод для изменения громкости звука
        /// </summary>
        /// <param name="AllEarpiece">число от 0x00000000 до 0xFFFFFFFF измеряющее громкость</param>
        static public void SetTheVolume(uint AllEarpiece)
        {
            Earpiece = AllEarpiece;
            waveOutSetVolume(hWO, AllEarpiece);
        }


        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int  uReturnLength, int hwdCallBack);

        public static string _nameSong;

        public static void OpenAudioFile(string _file)
        { 
            string _format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string _command = string.Format(_format, _file);
            _nameSong = _file;
            mciSendString(_command, null, 0, 0);
        }

        public static void Start()
        {
            string _command = "play MediaFile";
            mciSendString(_command, null, 0, 0);
        }
        public static  void Stop()
        {
            string _command = "stop MediaFile";
            mciSendString(_command, null, 0, 0);
        }


        public static void ChangingTheVolume(string _valueS)
        {
            switch (_valueS)
            {
                case "максимум":
                    SetTheVolume(Convert.ToInt32(100), Convert.ToInt32(100), true);
                    break;

                case "минимум":
                    SetTheVolume(Convert.ToInt32(10), Convert.ToInt32(5), true);
                    break;

                case "середина":
                    SetTheVolume(Convert.ToInt32(50), Convert.ToInt32(50), true);
                    break;

                case "тише":
                    if (LeftEarpiece > 0x3fac)
                    {
                        MessageBox.Show("Музыка тише");
                        SetTheVolume(Convert.ToUInt32(LeftEarpiece - 0x3fac), Convert.ToUInt32(RightEarpiece - 0x3fac));
                    }
                    else
                        SetTheVolume(Convert.ToUInt32(0x0014), Convert.ToUInt32(0x0014));
                    break;

                case "больше":
                    if (LeftEarpiece < 0xff14)
                        SetTheVolume(Convert.ToUInt32(LeftEarpiece + 0x3fac), Convert.ToUInt32(RightEarpiece + 0x3fac));
                    else 
                        SetTheVolume(Convert.ToUInt32(0xFFFF), Convert.ToUInt32(0xFFFF));
                    break;
            }
        }

        public static void ChoosingAudioFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mp3 Files|*.mp3";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenAudioFile(ofd.FileName);
                }
            }
        }


        public static void PlayingFavoriteSong(string _favoriteFile)
        {
            string file = _audioPath + _favoriteFile;//название песни с полным путём
            if (!string.IsNullOrEmpty(file))
            {
                try
                {
                    OpenAudioFile(file);
                }
                // probably should only catch specific exceptions
                // throwable by the above methods.
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " - ошибка при работе с файлом:" + file);
                }
            }
            else
            {
                MessageBox.Show("указан пустой файл и путь");
            }
        }

        public static void ChoosingRandomAudioFile()
        {
            string file = null;
            if (!string.IsNullOrEmpty(_audioPath))
            {
                var extensions = new string[] { ".mp3" };
                try
                {
                    var di = new DirectoryInfo(_audioPath);
                    var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                    Random R = new Random();
                    file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
                    OpenAudioFile(file);
                }
                // probably should only catch specific exceptions
                // throwable by the above methods.
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " - ошибка при работе с файлом:" + _audioPath);
                }
            }
        }

    }
}
