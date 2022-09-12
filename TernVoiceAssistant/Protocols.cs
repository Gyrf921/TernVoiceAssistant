using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TernVoiceAssistant
{
    internal class Protocols : OpenApplication
    {
        private static List<string> _nameProgremsForStart = new List<string>();
        private static string[] myArr = new string[PersonGrammar._NameProgram.Length];
        public static string StartProtocol(string _nameProt) 
        {
            switch (_nameProt)
            {
                case "стандартный":
                    _nameProgremsForStart.Add("гугл");
                    _nameProgremsForStart.Add("вконтакте");
                    _nameProgremsForStart.Add("телеграм");
                    _nameProgremsForStart.CopyTo(myArr);
                    return StartProgramm(myArr);
                    
                case "рабочий":
                    _nameProgremsForStart.Add("телеграм");
                    _nameProgremsForStart.Add("гугл");
                    _nameProgremsForStart.Add("рабочую таблицу");
                    _nameProgremsForStart.CopyTo(myArr);
                    return StartProgramm(myArr);
                    
                case "игровой":
                    _nameProgremsForStart.Add("дискорд");
                    _nameProgremsForStart.Add("стим");
                    _nameProgremsForStart.Add("гугл");
                    _nameProgremsForStart.Add("вконтакте");
                    _nameProgremsForStart.CopyTo(myArr);
                    return StartProgramm(myArr);
                    
                case "полный":
                    _nameProgremsForStart.AddRange(PersonGrammar._NameProgram);
                    _nameProgremsForStart.CopyTo(myArr);
                    return StartProgramm(myArr);
                    
            }
            return "";
        }
    }
}
