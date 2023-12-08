using EasySave_CLI.Models;

namespace EasySave_CLI.Views
{
    internal class format_v
    {
        private static readonly language_m _language = new language_m(); // Instance of the language model
        private static string? lang = _language.RetrieveValueFromLanguageFile("ChosenLanguage", "Lang", true);

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
