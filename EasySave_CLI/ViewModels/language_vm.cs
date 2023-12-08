using EasySave_CLI.Views;
using EasySave_CLI.Models;

namespace EasySave_CLI.ViewModels;

public class language_vm
{
    language_v _view = new language_v();
    private static readonly language_m _language = new language_m(); // Instance of the language model
    private static string? lang = _language.RetrieveValueFromLanguageFile("ChosenLanguage", "Lang", true);
    
    public void Run()
    {
        
        _view.Display();
        string? choosedLanguage = Console.ReadLine();
        while (choosedLanguage != "en" && choosedLanguage != "fr")
        {
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "InvalidLanguage"));
            choosedLanguage = Console.ReadLine();
        }
        _language.ChangeLanguage(choosedLanguage);
        _view.ChangedLanguage();
    }
}