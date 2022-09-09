using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace TernVoiceAssistant
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer ss = new SpeechSynthesizer();
        private static bool _active_Or_passive = false; // active - true | passive - false
        static CultureInfo _language = new CultureInfo("ru-RU");
        static TextBox textBox;

        private Audio.AudioPlayer _mp3Player = new Audio.AudioPlayer();
        private string _audioPath = @"E:\Программирование\МузыкаДляТерна\";
        private string _nameFavoriteSong = @"remember_-_Kozhura_74181540.mp3";

        uint LeftVolume = 0x0000;
        uint RightVolume = 0x0000;

        private ContextMenu contextMenu1;
        private MenuItem menuItemExit;

        public Form1()
        {
            #region
            InitializeComponent();
            ss.SetOutputToDefaultAudioDevice();
            ss.Volume = 100;// от 0 до 100 громкость голоса
            ss.Rate = 2; //от -10 до 10 скорость голоса

            //notifyIconTern.Text = "текст в трее";
            //notifyIconTern.Visible = true;
            //this.Resize += new EventHandler(Form1_Resize);
            #endregion


            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItemExit = new System.Windows.Forms.MenuItem();

            // Инициализировать contextMenu1
            contextMenu1.MenuItems.AddRange(
                        new MenuItem[] { this.menuItemExit });

            // Инициализация один из элементов меню управления в трее - menuItemExit
            menuItemExit.Name = "menuItemExit"; 
            menuItemExit.Index = 0;
            menuItemExit.Text = "Закрыть";
            menuItemExit.Click += new EventHandler(menuItemExit_Click);



            // Свойство Icon задает значок, который будет отображаться
            // в системном трее для этого приложения.
            //notifyIcon1.Icon = new Icon(@"C:\Users\PC\source\repos\TernVoiceAssistant\TernVoiceAssistant\image\voice.ico");

            // Свойство ContextMenu задает меню, которое будет
            // появляется при щелчке правой кнопкой мыши на значке системного трея.
            notifyIconTern.ContextMenu = this.contextMenu1;

            //// Обработайте событие двойного щелчка, чтобы активировать форму.
            //notifyIconTern.DoubleClick += new System.EventHandler(this.notifyIconTern_Click);
        }

        private void menuItemExit_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void notifyIconTern_Click(object sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }



        public void VoicуThisText(string txtForVoice)
        {
            ss.SpeakAsync(txtForVoice);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {


            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(_language);
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Sre_SpeechRecognized);

            Grammar g_StartStop = PersonGrammar.HelloWorldGrammar();
            Grammar g_StartProgram = PersonGrammar.OpenGameGrammar();
            Grammar g_StartStopActiveLaunch = PersonGrammar.StartStopActiveLaunchGrammar();
            Grammar g_WeatherGrammar = PersonGrammar.WeatherGrammar();
            Grammar g_TypeOfProtocol = PersonGrammar.TypeOfProtocolGrammar();
            Grammar g_VoiceJokeGrammar = PersonGrammar.VoiceJokeGrammar();
            Grammar g_AudioGrammar = PersonGrammar.AudioGrammar();
            Grammar g_SetValueAudio = PersonGrammar.SetValueSoundGrammar();
            Grammar g_OnOffPC = PersonGrammar.OnOffPC_Grammar();
            Grammar g_Compl = PersonGrammar.Compliments_Grammar();


            sre.LoadGrammar(g_StartStop);
            sre.LoadGrammar(g_StartProgram);
            sre.LoadGrammar(g_StartStopActiveLaunch);
            sre.LoadGrammar(g_WeatherGrammar);
            sre.LoadGrammar(g_TypeOfProtocol);
            sre.LoadGrammar(g_VoiceJokeGrammar);
            sre.LoadGrammar(g_AudioGrammar);
            sre.LoadGrammar(g_SetValueAudio);
            sre.LoadGrammar(g_OnOffPC);
            sre.LoadGrammar(g_Compl);

            sre.RecognizeAsync(RecognizeMode.Multiple);

           //this.Visible = false;
        }


        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {   
            string _SpokenText = e.Result.Text; //сказанный текст
            float confidence = e.Result.Confidence; //Точность сказаного текста(процент совпадения)

           
            if (confidence >= 0.6)
            {
               
                if (_SpokenText.IndexOf(PersonGrammar._StartStopActiveLaunch[0]) >= 0 && _active_Or_passive == true) //пассивный режим
                {
                    VoicуThisText("Перехожу в пассивный режим");
                    _active_Or_passive = false;
                }
                else if (_SpokenText.IndexOf(PersonGrammar._StartStopActiveLaunch[1]) >= 0 && _active_Or_passive == false) 
                {
                    VoicуThisText("Перехожу в активный режим");
                    _active_Or_passive = true;
                }
            }
            if (_active_Or_passive == true)
            {

                if (confidence >= 0.6)
                {
                    foreach (string _valueS in PersonGrammar._ValueSound)
                    {
                        if (_SpokenText.IndexOf($"громкость {_valueS}") >= 0 || _SpokenText.IndexOf($"громкость {_valueS} ") >= 0)
                        {
                            switch (_valueS) 
                            {
                                case "максимум":
                                    SetValueNoize(Convert.ToInt32(100), Convert.ToInt32(100)); //Изменение громкости
                                    break;
                                case "минимум":
                                    SetValueNoize(Convert.ToInt32(10), Convert.ToInt32(10)); //Изменение громкости
                                    break;
                                case "середина":
                                    SetValueNoize(Convert.ToInt32(50), Convert.ToInt32(50)); //Изменение громкости
                                    break;
                                case "тише":
                                    ShowCurrentVolume();
                                    if (LeftVolume > 0x1388)
                                    {
                                        SetValueNoize16(Convert.ToUInt32(LeftVolume - 0x1388), Convert.ToUInt32(RightVolume - 0x1388)); //Изменение громкости
                                    }
                                    else
                                    {
                                        SetValueNoize16(Convert.ToUInt32(0x14), Convert.ToUInt32(0x14)); //Изменение громкости
                                    }                                    
                                    break;
                                case "больше":
                                    ShowCurrentVolume();
                                    if (LeftVolume < 0xEC77)
                                    {
                                        SetValueNoize16(Convert.ToUInt32(LeftVolume + 0x1388), Convert.ToUInt32(RightVolume + 0x1388)); //Изменение громкости
                                    }    
                                    break;
                            }

                            break;
                        }
                    }
                }//Изменение громкости
                if (confidence >= 0.6)
                {
                    foreach (string word in PersonGrammar._WordsForHello)
                    {
                        if (confidence >= 0.7)
                        {
                            if (_SpokenText.IndexOf(word) >= 0)
                            {
                                VoicуThisText("Здравствуйте, создатель.");
                                textBox.Text += "  " + word;
                            }
                        }
                    }//Приветствие
                    foreach (string _wordGame in PersonGrammar._StartAll)
                    {
                        foreach (string _nameProt in PersonGrammar._NameProtocol)
                        {
                            
                            if (_SpokenText.IndexOf(_wordGame +" "+ _nameProt + " протокол") >= 0 || _SpokenText.IndexOf(_wordGame + " протокол " + _nameProt) >= 0)
                            {
                                
                                
                                VoicуThisText("Запускаю " + Protocols.StartProtocol(_nameProt));
                            }
                        }
                    }
                }// Протоколы
                foreach (string _wordStart in PersonGrammar._StartGameWithstartWord[0])
                {
                    if (confidence >= 0.7)
                    {
                        if (_SpokenText.IndexOf(_wordStart) >= 0)
                        {
                            foreach (string _wordGame in PersonGrammar._StartGameWithstartWord[1])
                            {
                                if (confidence >= 0.7)
                                {
                                    if (_SpokenText.IndexOf(_wordGame) >= 0)
                                    {
                                        if (_SpokenText == _wordStart + " " + _wordGame)
                                        {
                                            string[] ArrayforsinglStart = new string[1] { _wordGame };
                                            StartProgramm(ArrayforsinglStart);
                                        }
                                    }
                                }
                            }
                        }
                    }
                } //Запуск Программ
                if (confidence >= 0.7)
                {
                    foreach (string _DoWeather in PersonGrammar._StartAll)
                    {
                        foreach (string _wordWeather in PersonGrammar._Weather)
                        {
                            if (_SpokenText.IndexOf($"{_DoWeather} {_wordWeather}") >= 0)
                            {
                                DateTime localDate = DateTime.Now;
                                VoicуThisText(OpenWeather.OpenWeather.FullWeatherAnswer(_DoWeather, _SpokenText, localDate));
                                textBox.Text += OpenWeather.OpenWeather.FullWeatherAnswer(_DoWeather, _SpokenText, localDate);
                            }
                        }
                    }
                }//Погода
                if (confidence >= 0.7)
                {
                    if (_SpokenText.IndexOf($"расскажи анекдот") >= 0 || _SpokenText.IndexOf($"терн расскажи анекдот") >= 0)
                    {
                        Random _randomJoke = new Random();
                        int _numberJoke = _randomJoke.Next(0, PersonGrammar._JustJoke.Length);
                        VoicуThisText(PersonGrammar._JustJoke[_numberJoke]);

                        int _numberLaught = _randomJoke.Next(0, PersonGrammar._Ternlaugh.Length);
                        VoicуThisText(PersonGrammar._Ternlaugh[_numberLaught]);
                    }
                       
                }//создание файлов
                if (confidence >= 0.6)
                {
                    foreach (string _SlartSong in PersonGrammar._StartAll)
                    {
                        if (_SpokenText.IndexOf($"{_SlartSong} случайную песню") >= 0)
                        {
                            VoicуThisText("включаю случайную песню");
                            Thread.Sleep(2000);
                            ChoosingRandomAudioFile(_audioPath); //Указывается путь в котором находятся все песни
                            _mp3Player.Start();
                            break;
                        }
                        else if (_SpokenText.IndexOf($"{_SlartSong} мою любимую песню") >= 0)
                        {
                            VoicуThisText("включаю любимую песню");
                            Thread.Sleep(2000);
                            PlayingFavoriteSong(_audioPath, _nameFavoriteSong);
                            _mp3Player.Start();
                            break;
                        }
                        else if(_SpokenText.IndexOf($"{_SlartSong} выбор песни") >= 0)
                        {
                            VoicуThisText("выберете песню");
                            ChoosingAudioFile();
                            _mp3Player.Start();
                            break;
                        }
                        
                    }
                    foreach (string _typeSong in PersonGrammar._AudioType)
                    {
                        if (_SpokenText.IndexOf($"установить песню как {_typeSong}") >= 0)
                        {
                            VoicуThisText("песня установленна как любимая");
                            _nameFavoriteSong = _mp3Player._nameSong;
                            break;
                        }
                    }
                    if (_SpokenText.IndexOf($"остановить музыку") >= 0)
                    {
                        VoicуThisText("останавливаю песню");
                        _mp3Player.Stop();  
                    }
                }//Музыка
                if (confidence >= 0.7)
                {
                    if (_SpokenText.IndexOf($"выключи компьютер") >= 0)
                    {
                        WorkWithSystem.TurnOffPC();
                    }
                    if (_SpokenText.IndexOf($"перезагрузка") >= 0)
                    {
                        WorkWithSystem.RestartPC();
                    }
                }//выкл ПК
                if (confidence >= 0.7)
                {
                    foreach (string _comp in PersonGrammar._Compliments)
                    {
                        if (_SpokenText.IndexOf($"сделай комплимент") >= 0 || _SpokenText.IndexOf($"у меня плохое настроение") >= 0)
                        {  
                            Random _random = new Random();
                            int _numbercompl = _random.Next(0, PersonGrammar._Compliments.Length);
                            VoicуThisText(PersonGrammar._Compliments[_numbercompl].ToString());
                            break;
                        }
                    }
                }//создание файлов

            }
        }

        private void StartProgramm(string[] _namePrograms)
        {
            string _NameForVoice = OpenApplication.StartProgramm(_namePrograms);
            VoicуThisText("Запускаю " + _NameForVoice);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChoosingAudioFile();
        }

        private void StartAudio(string _file)
        {
            _mp3Player.Start();
        }
        private void StopAudio(string _file)
        {
            _mp3Player.Stop();
        }

        private void ChoosingAudioFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mp3 Files|*.mp3";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _mp3Player.OpenAudioFile(ofd.FileName);
                }
            }
        }

        private void PlayingFavoriteSong(string _path, string _favoriteFile)
        {
            string file = _path + _favoriteFile;//название песни с полным путём
            if (!string.IsNullOrEmpty(file))
            {
                try
                {
                    _mp3Player.OpenAudioFile(file);
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
                VoicуThisText("указан пустой файл и путь");
            }
        }

        private void ChoosingRandomAudioFile(string _path)
        {
            string file = null;
            if (!string.IsNullOrEmpty(_path))
            {
                var extensions = new string[] { ".mp3"};
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
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message +" - ошибка при работе с файлом:"+ _path);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            _mp3Player.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            // FF = 255 - максимальная громкость, для удобства использования применяется 100% система исчисления

            SetValueNoize(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox4.Text));
        }

        public void SetValueNoize(int leftEarpiece10, int rightEarpiece10)
        {
            int leftEar = 0xffff;
            int righttEar = 0xffff;

            leftEar = (int)(65535.0 / 100.0 * leftEarpiece10);
            righttEar = (int)(65535.0 / 100.0 * rightEarpiece10);

            int fullValue = 0x00000000;
            fullValue = (leftEar << 16) | leftEar;

            Audio.AudioPlayer.SetTheVolume(Convert.ToString(fullValue, 16));
            textBox.Text = Convert.ToString(fullValue, 16);
        }

        public void SetValueNoize16(uint leftEarpiece, uint rightEarpiece)
        {

            uint fullValue = 0x00000000;
            fullValue = (leftEarpiece << 16) | rightEarpiece;

            Audio.AudioPlayer.SetTheVolume(Convert.ToString(fullValue, 16));
            textBox.Text += Convert.ToString(fullValue, 16);
        }

        private void ShowCurrentVolume()
        {
            uint volume;
            Audio.AudioPlayer.waveOutGetVolume(IntPtr.Zero, out volume);
            uint left = (uint)(volume & 0xFFFF);
            uint right = (uint)((volume >> 16) & 0xFFFF);
            LeftVolume = left;
            RightVolume = right;
        }



        //Установка песни как стандартной
        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mp3 Files|*.mp3";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _nameFavoriteSong = ofd.FileName;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
