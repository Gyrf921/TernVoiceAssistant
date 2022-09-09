using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TernVoiceAssistant.Audio
{
    class AudioPlayer
    {
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint waveOutGetVolume(IntPtr hwo, out uint pdwVolume);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern int waveOutSetVolume(uint uDeviceID, uint dwVolume);
        //dwVolume новая громкость - Значение 0xFFFF FFFF соответствует полной громкости, а значение 0x0000 0000 — тишине ||| FFFF - 65535


        static public void SetTheVolume(string valueSound)
        {
            uint hWO = 0; //номер устройства для которого меняется звук, по дефолту 0
            uint valueSoundToEx = 0x00000000;
            valueSoundToEx = Convert.ToUInt32(valueSound, 16);
            waveOutSetVolume(hWO, valueSoundToEx);
        }


        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int  uReturnLength, int hwdCallBack);

        public string _nameSong;
        public void OpenAudioFile(string _file)
        { 
            string _format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string _command = string.Format(_format, _file);
            _nameSong = _file;
            mciSendString(_command, null, 0, 0);
        }

        public void Start()
        {
            string _command = "play MediaFile";
            mciSendString(_command, null, 0, 0);
        }
        public void Stop()
        {
            string _command = "stop MediaFile";
            mciSendString(_command, null, 0, 0);
        }

        public string NextSong()
        {


            return "";
        }
        public string PreviousSong()
        {


            return "";
        }

        public void SetValueNoize(int leftEarpiece10, int rightEarpiece10)
        {
            int leftEar = 0xffff;
            int righttEar = 0xffff;

            leftEar = (int)(65535.0 / 100.0 * leftEarpiece10);
            righttEar = (int)(65535.0 / 100.0 * rightEarpiece10);

            int fullValue = 0x00000000;
            fullValue = (leftEar << 16) | leftEar;

            SetTheVolume(Convert.ToString(fullValue, 16));   
        }

    }
}
