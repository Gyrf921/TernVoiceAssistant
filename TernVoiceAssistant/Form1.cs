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
        static readonly CultureInfo _language = new CultureInfo("ru-RU");

        
        private string _nameFavoriteSong = @"remember_-_Kozhura_74181540.mp3";
        private MenuItem menuItemExit, menuItemSetting, menuItemStopWork;

        Dictionary<int, string> DictForItemStopWork = new Dictionary<int, string>() { { 0, "Приостановить работу"}, { 1, "Продолжить работу"} };

        public Form1()
        {
            
            InitializeComponent();
            ss.SetOutputToDefaultAudioDevice();
            ss.Volume = 100;// от 0 до 100 громкость голоса
            ss.Rate = 2; //от -10 до 10 скорость голоса

            this.components = new System.ComponentModel.Container();
            ContextMenu contextMenu1 = new System.Windows.Forms.ContextMenu();
            menuItemExit = new System.Windows.Forms.MenuItem();
            menuItemSetting = new System.Windows.Forms.MenuItem();
            menuItemStopWork = new System.Windows.Forms.MenuItem();
            // Инициализировать contextMenu1
            contextMenu1.MenuItems.AddRange(
                        new MenuItem[] { menuItemExit , menuItemSetting, menuItemStopWork });

            // Инициализация один из элементов меню управления в трее - menuItemExit
            menuItemExit.Name = "menuItemExit"; 
            menuItemExit.Index = 0;
            menuItemExit.Text = "Закрыть";
            menuItemExit.Click += new EventHandler(menuItemExit_Click);

            menuItemSetting.Name = "menuItemSetting";
            menuItemSetting.Index = 1;
            menuItemSetting.Text = "Настройки";
            menuItemSetting.Click += new EventHandler(menuItemSetting_Click);

            menuItemStopWork.Name = "menuItemStopWork";
            menuItemStopWork.Index = 2;
            if(_active_Or_passive == true)
                menuItemStopWork.Text = DictForItemStopWork[0];
            else
                menuItemStopWork.Text = DictForItemStopWork[1];
            menuItemStopWork.Click += new EventHandler(menuItemStopWork_Click);


            notifyIconTern.ContextMenu = contextMenu1;


            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(_language);
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Sre_SpeechRecognized);

            sre.LoadGrammar(PersonGrammar.HelloWorldGrammar());
            sre.LoadGrammar(PersonGrammar.OpenGameGrammar());
            sre.LoadGrammar(PersonGrammar.StartStopActiveLaunchGrammar());
            sre.LoadGrammar(PersonGrammar.WeatherGrammar());
            sre.LoadGrammar(PersonGrammar.TypeOfProtocolGrammar());
            sre.LoadGrammar(PersonGrammar.VoiceJokeGrammar());
            sre.LoadGrammar(PersonGrammar.AudioGrammar());
            sre.LoadGrammar(PersonGrammar.SetValueSoundGrammar());
            sre.LoadGrammar(PersonGrammar.OnOffPC_Grammar());
            sre.LoadGrammar(PersonGrammar.Compliments_Grammar());

            sre.RecognizeAsync(RecognizeMode.Multiple);

            this.Visible = false;
        }

        private void menuItemExit_Click(object Sender, EventArgs e)
        {
            this.Close();
        }
        private void menuItemSetting_Click(object Sender, EventArgs e)
        {
            new Form1().Show();
        }
        private void menuItemStopWork_Click(object Sender, EventArgs e)
        {
            if (menuItemStopWork.Text == DictForItemStopWork[0]) 
            {
                menuItemStopWork.Text = DictForItemStopWork[1];
                VoicуThisText("Перехожу в пассивный режим");
                _active_Or_passive = false;
                return;
            }
            menuItemStopWork.Text = DictForItemStopWork[0];
            VoicуThisText("Перехожу в активный режим");
            _active_Or_passive = true;
            return;
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
                    return;
                }
                else if (_SpokenText.IndexOf(PersonGrammar._StartStopActiveLaunch[1]) >= 0 && _active_Or_passive == false) 
                {
                    VoicуThisText("Перехожу в активный режим");
                    _active_Or_passive = true;
                    
                }

                if (_active_Or_passive == true)
                {
                    if (confidence >= 0.6)
                    {
                        foreach (string _wordForStart in PersonGrammar._StartAll)
                        {
                            foreach (string _nameProt in PersonGrammar._NameProtocol)
                            {

                                if (_SpokenText.IndexOf(_wordForStart + " " + _nameProt + " протокол") >= 0 || _SpokenText.IndexOf(_wordForStart + " протокол " + _nameProt) >= 0)
                                {
                                    VoicуThisText("Запускаю " + Protocols.StartProtocol(_nameProt));
                                    return;
                                }
                            }

                            foreach (string _wordGame in PersonGrammar._StartGameWithstartWord[1])
                            {
                                if (_SpokenText.IndexOf(_wordForStart) >= 0 & _SpokenText.IndexOf(_wordGame) >= 0)
                                {
                                    string[] ArrayforsinglStart = new string[1] { _wordGame };
                                    StartProgramm(ArrayforsinglStart);
                                    return;
                                }
                            } //Запуск Программ

                            foreach (string _wordWeather in PersonGrammar._Weather)
                            {
                                if (_SpokenText.IndexOf($"{_wordForStart} {_wordWeather}") >= 0)
                                {
                                    DateTime localDate = DateTime.Now;
                                    VoicуThisText(OpenWeather.OpenWeather.FullWeatherAnswer(_wordForStart, _SpokenText, localDate));
                                    return;
                                }
                            }//Погода
                            if (_SpokenText.IndexOf($"анекдот") >= 0 || _SpokenText.IndexOf($"шутку") >= 0)
                            {
                                Random _randomJoke = new Random();
                                int _numberJoke = _randomJoke.Next(0, PersonGrammar._JustJoke.Length);
                                VoicуThisText(PersonGrammar._JustJoke[_numberJoke]);

                                int _numberLaught = _randomJoke.Next(0, PersonGrammar._Ternlaugh.Length);
                                VoicуThisText(PersonGrammar._Ternlaugh[_numberLaught]);
                                return;
                            }//шутки

                            if (_SpokenText.IndexOf($"{_wordForStart} случайную песню") >= 0)
                            {
                                VoicуThisText("включаю случайную песню");
                                Thread.Sleep(2000);
                                Audio.AudioPlayer.ChoosingRandomAudioFile(); //Указывается путь в котором находятся все песни
                                Audio.AudioPlayer.Start();
                                return;
                            }
                            else if (_SpokenText.IndexOf($"{_wordForStart} мою любимую песню") >= 0)
                            {
                                VoicуThisText("включаю любимую песню");
                                Thread.Sleep(2000);
                                Audio.AudioPlayer.PlayingFavoriteSong(_nameFavoriteSong);
                                Audio.AudioPlayer.Start();
                                return;
                            }
                            else if (_SpokenText.IndexOf($"{_wordForStart} выбор песни") >= 0)
                            {
                                VoicуThisText("выберете песню");
                                Audio.AudioPlayer.ChoosingAudioFile();
                                Audio.AudioPlayer.Start();
                                return;
                            }
                        }


                        foreach (string _typeSong in PersonGrammar._AudioType)
                        {
                            if (_SpokenText.IndexOf($"установить песню как {_typeSong}") >= 0)
                            {
                                VoicуThisText("песня установленна как любимая");
                                _nameFavoriteSong = Audio.AudioPlayer._nameSong;
                                return;
                            }
                        }
                        foreach (string _comp in PersonGrammar._Compliments)
                        {
                            if (_SpokenText.IndexOf($"сделай комплимент") >= 0 || _SpokenText.IndexOf($"у меня плохое настроение") >= 0)
                            {
                                Random _random = new Random();
                                int _numbercompl = _random.Next(0, PersonGrammar._Compliments.Length);
                                VoicуThisText(PersonGrammar._Compliments[_numbercompl].ToString());
                                return;
                            }
                        }
                        foreach (string _valueS in PersonGrammar._ValueSound)
                        {
                            if (_SpokenText.IndexOf($"громкость {_valueS}") >= 0) // || _SpokenText.IndexOf($"громкость {_valueS} ") >= 0
                            {
                                Audio.AudioPlayer.ChangingTheVolume(_valueS);
                                return;
                            }
                        }
                        //Изменение громкости

                        foreach (string word in PersonGrammar._WordsForHello)
                        {

                            if (_SpokenText.IndexOf(word) >= 0)
                            {
                                VoicуThisText("Здравствуйте, создатель.");
                                return;
                            }
                            
                        }//Приветствие

                        //Музыка
                        WorkWithSystem.CoiseActionWithSystem(_SpokenText);//выкл ПК
                        
                        if (_SpokenText.IndexOf($"остановить музыку") >= 0)
                        {
                            VoicуThisText("останавливаю песню");
                            Audio.AudioPlayer.Stop();
                            return;
                        }
                        
                    }//создание файлов
                }
            } 
        }

        private void StartProgramm(string[] _namePrograms)
        {
            string _NameForVoice = OpenApplication.StartProgramm(_namePrograms);
            VoicуThisText("Запускаю " + _NameForVoice);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Audio.AudioPlayer.ChoosingAudioFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Audio.AudioPlayer.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // FF = 255 - максимальная громкость, для удобства использования применяется 100% система исчисления

            Audio.AudioPlayer.SetTheVolume(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox4.Text), true);
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
