using EasySave_CLI.Models; // Namespace for the models
namespace EasySave_CLI.Views; // Namespace for the views

public class mode_v // View for the mode
{
    public mode_v() {} // Builder for the mode
    
    private static readonly language_m _language = new language_m(); // Instance of the language model
    private static string? lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang"); // Get the language
    public string? SetMode(int error) // Function to set the mode
    {
        if (error == 1) // If the user entered a wrong command
        {
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "CommandNotValid")); // Display an error message
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "PressAnyKey")); // Display a message
            Console.ReadKey(); // Wait for the user to press a key
        }
        Console.Clear(); // Clear the console
        lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang"); // Get the language
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "ModeSelect")); // Display the modes
        string? mode = Console.ReadLine(); // Get the mode
        return mode; // Return the mode
    }
}