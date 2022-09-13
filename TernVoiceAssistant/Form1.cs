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

        Dictionary<int, string> DictForItemStopWork = new Dictionary<int, string>() { { 0, "Приостановить работу" }, { 1, "Продолжить работу" } };

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
                        new MenuItem[] { menuItemExit, menuItemSetting, menuItemStopWork });

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
            sre.LoadGrammar(PersonGrammar.OnOffPCGrammar());
            sre.LoadGrammar(PersonGrammar.ComplimentsGrammar());
            sre.LoadGrammar(PersonGrammar.StopWorkInterfaceGrammar());
            sre.LoadGrammar(PersonGrammar.StartWorkInterfaceGrammar());
            sre.LoadGrammar(PersonGrammar.StopWorkTern());
            sre.LoadGrammar(PersonGrammar.SetOutputDeviceGrammar());

            sre.RecognizeAsync(RecognizeMode.Multiple);

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
            StopWork_Click();
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
                CallChoiseActiveOrPassiveMode(_SpokenText);

                

                if (_active_Or_passive == true)
                {

                    foreach (string _wordForStart in PersonGrammar._StartAll)
                    {
                        CallStartProtocol(_wordForStart, _SpokenText);

                        CallStartProgram(_wordForStart, _SpokenText);

                        CallTheWeather(_wordForStart, _SpokenText);

                        CallTheJoke(_wordForStart, _SpokenText);

                        CallPlayMusic(_wordForStart, _SpokenText);

                        CallCompliments(_wordForStart, _SpokenText);

                        CallShowIntarface(_wordForStart, _SpokenText);

                        CallSetOutputDevice(_wordForStart, _SpokenText);
                    }
                    foreach (string _wordForStop in PersonGrammar._StopAll)
                    {
                        CallStopMusic(_wordForStop, _SpokenText);

                        CallHideIntarface(_wordForStop, _SpokenText);

                        CallStopTern(_wordForStop, _SpokenText);
                    }

                    CallSetLikeMusic(_SpokenText);

                    CallHello(_SpokenText);

                    CallSettingsValue(_SpokenText);

                    WorkWithSystem.CoiseActionWithSystem(_SpokenText);

                }
            }
        }

        private void CallHideIntarface(string _wordForStop, string _SpokenText)
        {
            if (_SpokenText.IndexOf($"{_wordForStop} интерфейс") >= 0)
            {
                this.Hide();
                this.Visible = false;

                VoicуThisText("Скрываю интерфейс");
                return;
            }
        }
        private void CallStopMusic(string _wordForStop, string _SpokenText)
        {
            if (_SpokenText.IndexOf($"{_wordForStop} музыку") >= 0 || _SpokenText.IndexOf($"{_wordForStop} песню") >= 0)
            {
                VoicуThisText("останавливаю песню");
                Audio.AudioPlayer.Stop();
                return;
            }
        }
        private void CallStopTern(string _wordForStop, string _SpokenText)
        {
            if (_SpokenText.IndexOf($"{_wordForStop} себя") >= 0 || _SpokenText.IndexOf($"{_wordForStop} терн") >= 0) 
            {
                this.Close();

            }

        }

        private void CallSetOutputDevice(string _wordForStart, string _SpokenText)
        {
            foreach (string outD in PersonGrammar._SetOutputDevice)
            {
                if (_SpokenText.IndexOf($"{_wordForStart} устройство вывода как {outD}") >= 0)
                {
                    switch (outD) 
                    {
                        case "колонки":
                            Audio.AudioPlayer.SetOutputDevice(0);
                            break;
                        case "наушники":
                            Audio.AudioPlayer.SetOutputDevice(1);
                            break;
                        case "другой":
                            Audio.AudioPlayer.SetOutputDevice(2);
                            break;
                    }
                    VoicуThisText($"Устройство вывода - {outD}");
                }
            }

        }

        private void CallShowIntarface(string _wordForStart, string _SpokenText)
        {
            if (_SpokenText.IndexOf($"{_wordForStart} интерфейс") >= 0)
            {
                this.Show();
                this.Visible = true;

                VoicуThisText("Показываю интерфейс");
                return;
            }

        }

        private void CallStartProtocol(string _wordForStart, string _SpokenText) 
        {
            foreach (string _nameProt in PersonGrammar._NameProtocol)
            {

                if (_SpokenText.IndexOf(_wordForStart + " " + _nameProt + " протокол") >= 0 || _SpokenText.IndexOf(_wordForStart + " протокол " + _nameProt) >= 0)
                {
                    VoicуThisText("Запускаю " + OpenApplication.StartProtocol(_nameProt));
                    return;
                }
            }
        }

        private void CallStartProgram(string _wordForStart, string _SpokenText) 
        {
            foreach (string _wordGame in PersonGrammar._StartGameWithstartWord[1])
            {
                if (_SpokenText.IndexOf(_wordForStart) >= 0 & _SpokenText.IndexOf(_wordGame) >= 0)
                {
                    string[] ArrayforsinglStart = new string[1] { _wordGame };

                    string _NameForVoice = OpenApplication.StartProgramm(ArrayforsinglStart);
                    VoicуThisText("Запускаю " + _NameForVoice);
                    return;
                }
            } //Запуск Программ
        }

        private void CallTheWeather(string _wordForStart, string _SpokenText)
        {
            foreach (string _wordWeather in PersonGrammar._Weather)
            {
                if (_SpokenText.IndexOf($"{_wordForStart} {_wordWeather}") >= 0)
                {
                    DateTime localDate = DateTime.Now;
                    VoicуThisText(OpenWeather.OpenWeather.FullWeatherAnswer(_wordForStart, _SpokenText, localDate));
                    return;
                }
            }//Погода
        }
        private void CallTheJoke(string _wordForStart, string _SpokenText)
        {
            if (_SpokenText.IndexOf($"{_wordForStart} анекдот") >= 0 || _SpokenText.IndexOf($"{_wordForStart} шутку") >= 0)
            {
                Random _randomJoke = new Random();
                int _numberJoke = _randomJoke.Next(0, PersonGrammar._JustJoke.Length);
                VoicуThisText(PersonGrammar._JustJoke[_numberJoke]);

                int _numberLaught = _randomJoke.Next(0, PersonGrammar._Ternlaugh.Length);
                VoicуThisText(PersonGrammar._Ternlaugh[_numberLaught]);
                return;
            }//шутки
        }

        private void CallPlayMusic(string _wordForStart, string _SpokenText)
        {
            if (_SpokenText.IndexOf($"{_wordForStart} случайную песню") >= 0 || _SpokenText.IndexOf($"{_wordForStart} случайную музыку") >= 0)
            {
                VoicуThisText("включаю случайную песню");
                Thread.Sleep(2000);
                Audio.AudioPlayer.ChoosingRandomAudioFile(); //Указывается путь в котором находятся все песни
                Audio.AudioPlayer.Start();
                return;
            }
            else if (_SpokenText.IndexOf($"{_wordForStart} любимую песню") >= 0 || _SpokenText.IndexOf($"{_wordForStart} мою любимую музыку") >= 0)
            {
                VoicуThisText("включаю любимую песню");
                Thread.Sleep(2000);
                Audio.AudioPlayer.PlayingFavoriteSong(_nameFavoriteSong);
                Audio.AudioPlayer.Start();
                return;
            }
            else if (_SpokenText.IndexOf($"{_wordForStart} выбор песни") >= 0 || _SpokenText.IndexOf($"{_wordForStart} выбор музыки") >= 0)
            {
                VoicуThisText("выберете песню");
                Audio.AudioPlayer.ChoosingAudioFile();
                Audio.AudioPlayer.Start();
                return;
            }
        }
        private void CallCompliments(string _wordForStart, string _SpokenText)
        {
            foreach (string _comp in PersonGrammar._Compliments)
            {
                if (_SpokenText.IndexOf($"{_wordForStart} {_comp}") >= 0)
                {
                    Random _random = new Random();
                    int _numbercompl = _random.Next(0, PersonGrammar._Compliments.Length);
                    VoicуThisText(PersonGrammar._Compliments[_numbercompl].ToString());
                    return;
                }
            }
        }
        private void CallSetLikeMusic(string _SpokenText)
        { 
            foreach (string _typeSong in PersonGrammar._AudioType)
            {
                if (_SpokenText.IndexOf($"установить песню как {_typeSong}") >= 0)
                {
                    if (Audio.AudioPlayer._nameSong == null)
                    {
                        VoicуThisText("Сейчас не выбрана никакая песня");
                        return;
                    }
                    VoicуThisText("песня установленна как любимая");
                    _nameFavoriteSong = Audio.AudioPlayer._nameSong;
                    return;
                }
            }
        }

        private void CallHello(string _SpokenText)
        {
            foreach (string word in PersonGrammar._WordsForHello)
            {
                if (_SpokenText.IndexOf(word) >= 0)
                {
                    VoicуThisText("Здравствуйте, создатель.");
                    return;
                }
            }//Приветствие
        }

        private void CallSettingsValue(string _SpokenText) 
        {
            foreach (string _valueS in PersonGrammar._ValueSound)
            {
                if (_SpokenText.IndexOf($"громкость {_valueS}") >= 0)
                {
                    Audio.AudioPlayer.ChangingTheVolume(_valueS);
                    return;
                }
            }
            //Изменение громкости

        }

        private void StopWork_Click()
        {
            if (_active_Or_passive == true)
            {
                _active_Or_passive = false;
                menuItemStopWork.Text = DictForItemStopWork[1];
                VoicуThisText("Перехожу в пассивный режим");
                return;
            }
            menuItemStopWork.Text = DictForItemStopWork[0];
            VoicуThisText("Перехожу в активный режим");
            _active_Or_passive = true;
            return;
        }

        private void CallChoiseActiveOrPassiveMode(string _SpokenText)
        {
            if (_SpokenText.IndexOf("переход в " + PersonGrammar._StartStopActiveLaunch[0] + " режим") >= 0) //пассивный режим
            {
                if (_active_Or_passive == false)
                {
                    VoicуThisText("Я и так в пассивном режиме");
                    return;
                }
                StopWork_Click();
                return;

            }
            else if (_SpokenText.IndexOf("переход в " + PersonGrammar._StartStopActiveLaunch[1] + " режим") >= 0)
            {
                if (_active_Or_passive == true)
                {
                    VoicуThisText("Я и так в активном режиме");
                    return;
                }
                StopWork_Click();
                return;
            }
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
            this.Visible = false;
        }
    }
}
