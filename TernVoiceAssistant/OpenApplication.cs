using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace TernVoiceAssistant
{
    internal class OpenApplication
    {

        public static string StartProgramm(string[] _namePrograms) 
        {
            string _NameForVoice = "";
            foreach (string _nameProgram in _namePrograms)
            {
                switch (_nameProgram)
                {
                    case "стим":
                        Process.Start(@"E:\Program Files (x86)\Steam\steam.exe");//путь к Стиму
                        break;
                    case "гугл":
                        Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");//путь к гуглу
                        break;
                    case "вконтакте":
                        Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "https://vk.com/");//ссылка на вк
                        break;
                    case string _wordInCase when (_wordInCase == "телеграм" || _wordInCase == "телеграмм"):
                        Process.Start(@"E:\Доп.проги\Telega\Telegram Desktop\Telegram.exe");//путь к телеграму
                        break;
                    case "рабочую таблицу":
                        Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "https://docs.google.com/spreadsheets/d/1c_YCoF7NfEPbIzega-rFxkveMq8ubC8F/edit#gid=1216431847");//путь к гуглу
                        break;
                    case "дискорд":
                        Process.Start(@"C:\Users\PC\AppData\Local\Discord\app-1.0.9004\Discord.exe");//путь к дискорду
                        break;
                    case " ":
                        break;
                    case "":
                        break;
                }
                _NameForVoice += _nameProgram + ", ";
            }
            return _NameForVoice;
        }
    }
}
