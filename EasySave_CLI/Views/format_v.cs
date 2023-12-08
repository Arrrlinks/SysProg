using EasySave_CLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave_CLI.Views
{
    internal class format_v
    {
        private static readonly language_m _language = new language_m(); // Instance of the language model
        private static string? lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang");

        public void Display()
        {
            Console.Clear();
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang,"ChooseFormat"));
        }
        public void ChangedFormat(string choosedFormat)
        {
            Console.Clear();
            Console.WriteLine($"{_language.RetrieveValueFromLanguageFile(lang, "FormatChanged")}{choosedFormat}");
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "PressAnyKey"));
            Console.ReadKey();
        }
    }
}
