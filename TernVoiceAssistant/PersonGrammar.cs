using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;


namespace TernVoiceAssistant
{
    public class PersonGrammar
    {
        public static CultureInfo _language = new CultureInfo("ru-RU");


        public static string[] _AudioType = new string[] { "любимую", "стандартную", "по умолчанию"}; // Запуск погоды
        public static string[] _Weather = new string[] { "погоду", "температуру", "ветер", "полный прогноз" }; // Запуск погоды
        public static string[] _StartAll = new string[] { "расскажи","запусти", "запуск", "старт", "открой", "выведи", "покажи", "включи", "активируй", "установить" }; // Общие слова для запуска чего либо
        public static string[] _NameProgram = new string[] { "стим", "валорант", "гугл", "вконтакте", "телеграм",  "рабочую таблицу", "дискорд"}; // Запуск приложения  
        public static string[][] _StartGameWithstartWord = new string[][] {  _StartAll ,  _NameProgram  };

        public static string[] _NameProtocol = new string[] { "стандартный", "рабочий", "игровой", "полный" };

        public static string[] _WordsForHello = new string[] { "здравствуй", "привет"}; //Приветствие , "здравствуй" , "доброе утро", "добрый день", "доброе вечер" 
        public static string[] _StartStopActiveLaunch = new string[] { "пассивный", "активный" };

        public static string[] _ValueSound = new string[] { "максимум","минимум","середина", "тише", "больше"};
        //"0", "1", "2", "3", "4", "5", "6" , "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "30", "40" , "50", "60", "70", "80", "90", "100" 

        //Комплименты выапхъывазхъпхвпх
        public static string[] _Compliments = new string[] {
            "Вы прекрасно выглядишь",
            "Вы чудо юдо",
            "Иди выключи компьютер" ,
            "Ты гений, миллиардер, плейбой, филантроп"
        };


        public static string[] _JustJoke = new string[] { 
            //Про штирлица
            "Штирлиц проводил Кэт до подъезда. Кэт сказала: Давайте встретимся завтра, у роддома... Штирлиц поднял глаза и увидел в окне свет - урод действительно был дома.",
            "В баре за стойкой мужчина в ОЗК смешивал в колбе ядохимикаты. \"Новичок\" подумал Штирлиц",
            "Штирлиц всегда спал как убитый. Его даже пару раз обводили мелом" ,
            "В дверь кто-то вежливо постучал ногой. - Безруков, догадался Штирлиц",
            "Штирлиц спросил Кэт: Вы любите фильмы про любовь? - Бесспорно ответила Кэт - А я с порно, - признался Штирлиц.",
            "Штирлиц стрелял в слепую, слепая бегала зигзагами",
            "Пахнет котом, подумал Штирлиц. Зачем он нюхает мою лапу? подумал кот",
            "Штирлиц безуспешно пытался установить личность. Личность всё время падала.",
            "Штирлицу в голову попала пуля. \"Разрывная\" - подумал Штирлиц пораскинув мозгами.",
            "- Штирлиц разделся и вошел в ванну. - Ах - сказала Анна, и обхватила его ногами...",
            "Штирлиц не любил массовых расстрелов, но отказываться было как-то неудобно...",
            "Штирлиц сел в машину и сказал Кэт- Трогай! Кэт потрогала и сказала - Ого!",
            "Штирлиц сел в машину и крикнул: - Гони! Через пять минут с заднего сиденья запахло свежим самогоном.",
            ////Про негров
            //"Как напугать негра? Взять его с собой на аукцион.",
            //"Какие у негра есть три белых вещи? Глаза, зубы и хозяин.",
            //"Что надо сделать, если вам навстречу бежит окровавленный негр? Перезарядить.",
            //"Какие три самых сложных года в жизни негра? Первый класс.",
            //"Что общего между кроссовками 'Найк' и Ку-Клукс-Кланом? Они заставляют негров быстро бегать.",
            //"Что надо бросить тонущему негру? Его семью.",
            //"Африканцы хотят попасть в ад, потому что там есть котлы с водой.",
            //"У кошек есть примета: если дорогу перебежит негр...",
            ////Про азиатов
            //"Китайцы взломали сайт Пентагона, каждый попробовал по паролю.",
            //"- Дорогие дети! Сегодня в программе \"Красный, желтый, зеленый\" я расскажу о том, как индеец и китаец нашли утопленника...",
            ////Чернуха
            //"Пап, хватит шутить на тему того, что я приёмный. Давай лучше в дурака сыграем! Отец: В подкидного?",
            //"Как разговаривают немые и безрукие? Болтают ногами",
            //"Мама, мне нанесли четыре ножевых ранения, я умираю... - Почему не пять?",
            //"На распродаже органов началась ужасная драка. Я еле успел унести ноги",
            //"Из студенческого общежития куда-то исчезли все кошки… Вот такие пироги.",
            ////Другое
            //"Признался друзьям о том, что они геи",
            //"Дрочат только слабаки. Настоящие мужчины eбут кулак.",
            //"Пупа и Лупа пришли в парфюмерную. Им выдали пробники духов, но у Лупы был заложен нос, поэтому Пупа нюхнул за Лупу",
            //"В магазине гражданин обращается к продавцу: — Скажите, пожалуйста, где сидит ваш заведующий? — А откуда вы знаете, что он сидит?"

        };
        public static string[] _Ternlaugh = new string[] {
            "ха-ха-ха-ха",
            "АХАХАХАХАХАХАХХАХА",
            "пхпхХПХХПхПХПХпх" ,
            "Хи хи Это было забавно хе хе"};



        public static Grammar HelloWorldGrammar()
        {
            Choices ch_StartCommands = new Choices();//Создание Выборки
            //Создаём массив из всех значений, которые будем использовать во время приветствия
            ch_StartCommands.Add(_WordsForHello); //Записываем массив в "Выборы"

            GrammarBuilder gb_StartStop = new GrammarBuilder(); //Создаём GrammarBuilder
            gb_StartStop.Culture = _language;//подключение русского языка
            gb_StartStop.Append(ch_StartCommands); //Заполняем шаблон GrammarBuilder «What is <x> plus <y>?»

            Grammar g_Hello = new Grammar(gb_StartStop); //управляющий Grammar

            return g_Hello;
        }
        public static Grammar StartStopActiveLaunchGrammar()
        {
            Choices ch_StartStopActiveLaunch = new Choices();
            ch_StartStopActiveLaunch.Add(_StartStopActiveLaunch);
            GrammarBuilder gb_SSAL = new GrammarBuilder();
            gb_SSAL.Culture = _language;
            gb_SSAL.Append("переход в");
            gb_SSAL.Append(ch_StartStopActiveLaunch);
            gb_SSAL.Append("режим");
            Grammar g_StartStop = new Grammar(gb_SSAL); //управляющий Grammar

            return g_StartStop;
        }
        public static Grammar OpenGameGrammar()
        {
            Choices ch_StartSMTH = new Choices(_StartAll);
            Choices ch_StartGame = new Choices(_NameProgram);
            
            GrammarBuilder gb_PlayStart = new GrammarBuilder();
            gb_PlayStart.Culture = _language;
            //Заполняем шаблон GrammarBuilder «"Запуск" + "Название игры"»
            gb_PlayStart.Append(ch_StartSMTH);
            gb_PlayStart.Append(ch_StartGame);
            

            Grammar g_StartStop = new Grammar(gb_PlayStart);

            return g_StartStop;
        }

        public static Grammar WeatherGrammar()
        {
            Choices ch_StartSMTH = new Choices();
            Choices ch_Weather = new Choices();
            ch_StartSMTH.Add(_StartAll);
            ch_Weather.Add(_Weather);
            GrammarBuilder gb_W = new GrammarBuilder();
            gb_W.Culture = _language;

            gb_W.Append(ch_StartSMTH);
            gb_W.Append(ch_Weather);

            Grammar g_Weather = new Grammar(gb_W); //управляющий Grammar
            return g_Weather;
        }

        public static Grammar TypeOfProtocolGrammar()
        {
            Choices ch_StartSMTH = new Choices(_StartAll);
            Choices ch_Protocol = new Choices(_NameProtocol);

            GrammarBuilder gb_P1 = new GrammarBuilder();
            GrammarBuilder gb_P2 = new GrammarBuilder();
            gb_P1.Culture = _language; gb_P2.Culture = _language;

            //Первый шаблон построения фразы
            gb_P1.Append(ch_StartSMTH);
            gb_P1.Append(ch_Protocol);
            gb_P1.Append("протокол");

            //второй шаблон построения фразы уже в другом GrammarBuilder
            gb_P2.Append(ch_StartSMTH);
            gb_P2.Append("протокола");
            gb_P2.Append(ch_Protocol);


            // Create a Choices for the two alternative phrases, convert the Choices  
            // to a GrammarBuilder, and construct the grammar from the result.  
            Choices bothChoices = new Choices(new GrammarBuilder[] { gb_P1, gb_P2 });

            Grammar g_V = new Grammar(bothChoices); //управляющий Grammar
            return g_V;
        }
        public static Grammar VoiceJokeGrammar()
        {
            Choices ch_StartSMTH = new Choices(_StartAll);
            Choices ch_DRSS = new Choices("анекдот", "шутку");
            GrammarBuilder gb_P1 = new GrammarBuilder();
            gb_P1.Culture = _language;

            gb_P1.Append(ch_StartSMTH);
            gb_P1.Append(ch_DRSS);

            Grammar g_V = new Grammar(gb_P1); //управляющий Grammar
            return g_V;
        }

        public static Grammar AudioGrammar()
        {
            Choices ch_StartSMTH = new Choices(_StartAll);
            Choices ch_Audio = new Choices(_AudioType);

            GrammarBuilder gb_P1 = new GrammarBuilder();
            GrammarBuilder gb_P2 = new GrammarBuilder();
            GrammarBuilder gb_P3 = new GrammarBuilder();
            GrammarBuilder gb_P4 = new GrammarBuilder();
            GrammarBuilder gb_P5 = new GrammarBuilder();
            gb_P1.Culture = _language; gb_P2.Culture = _language; gb_P3.Culture = _language;
            gb_P4.Culture = _language; gb_P5.Culture = _language;

            //Первый шаблон для открытия случайной песни
            gb_P1.Append(ch_StartSMTH);
            gb_P1.Append("случайную песню");

            //Первый шаблон для открытия установленной как стандартной песни
            gb_P2.Append(ch_StartSMTH);
            gb_P2.Append("мою любимую песню");

            //Первый шаблон для открытия выборочной песни
            gb_P3.Append(ch_StartSMTH);
            gb_P3.Append("выбор песни");

            //Установка текущей песни как стандарной
            gb_P4.Append("установить песню как");
            gb_P4.Append(ch_Audio);

            //Остановка песни
            gb_P5.Append("остановить музыку");

            Choices bothChoices = new Choices(new GrammarBuilder[] { gb_P1, gb_P2 , gb_P3, gb_P4, gb_P5 });

            Grammar g_V = new Grammar(bothChoices); //управляющий Grammar
            return g_V;
        }

        public static Grammar SetValueSoundGrammar()
        {
            Choices ch_value = new Choices();//Создание Выборки
            //Создаём массив из всех значений, которые будем использовать во время приветствия
            ch_value.Add(_ValueSound); //Записываем массив в "Выборы"

            GrammarBuilder gb_SetValue = new GrammarBuilder(); //Создаём GrammarBuilder
            gb_SetValue.Culture = _language;//подключение русского языка
            gb_SetValue.Append("громкость");
            gb_SetValue.Append(ch_value); //Заполняем шаблон GrammarBuilder «What is <x> plus <y>?»

            Grammar g_v = new Grammar(gb_SetValue); //управляющий Grammar

            return g_v;
        }
        public static Grammar OnOffPC_Grammar()
        {
            Choices ch_DRSS = new Choices("выключи компьютер", "перезагрузка");

            Grammar g_V = new Grammar(ch_DRSS); //управляющий Grammar
            return g_V;
        }
        public static Grammar Compliments_Grammar()
        {
            Choices ch_DRSS = new Choices("сделай комплимент", "у меня плохое настроение");

            Grammar g_V = new Grammar(ch_DRSS); //управляющий Grammar
            return g_V;
        }

    }
}
