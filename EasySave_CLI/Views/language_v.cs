using EasySave_CLI.Models;
namespace EasySave_CLI.Views;

public class language_v
{
    private static readonly language_m _language = new language_m(); // Instance of the language model
    private static string? lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang");
    public void Display()
    {
        Console.Clear();
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "ChooseLanguage")); // Ask the user to choose a language
    }
    
    public void ChangedLanguage()
    {
        lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang");
        Console.Clear();
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "LanguageChanged"));
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "PressAnyKey"));
        Console.ReadKey();
    }
}