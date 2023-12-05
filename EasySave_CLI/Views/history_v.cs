using EasySave_CLI.Models;

namespace EasySave_CLI.Views; // Namespace for the views

public class history_v // View for the history
{
    private static readonly language_m _language = new language_m(); // Instance of the language model
    private static readonly string? lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang");

    static void DisplayLogFile(string? inputDate) // Function to display the log file 
    {
        try // Try to display the log file
        {
            if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDate)) // If the date is valid
            {
                string filePath = $"../../../logs/{selectedDate.ToString("yyyyMMdd")}.json"; // Get the file path
                if (File.Exists(filePath)) // If the file exists
                {
                    string fileContent = File.ReadAllText(filePath); // Read the file
                    Console.WriteLine($"{_language.RetrieveValueFromLanguageFile(lang, "LogsFrom")} {selectedDate.ToString("dd/MM/yyyy")} :\n{fileContent}"); // Display the file content
                }
                else // If the file doesn't exist
                {
                    Console.WriteLine($"{_language.RetrieveValueFromLanguageFile(lang, "FileForTheDate")} {selectedDate.ToString("dd/MM/yyyy")} {_language.RetrieveValueFromLanguageFile(lang, "DoesntExists")}"); // Display an error message
                }
            }
            else // If the date is not valid
            {
                Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "InvalidDateFormat")); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"{_language.RetrieveValueFromLanguageFile(lang, "ErrorOccured")} {ex.Message}"); // Display an error message
        }
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "PressAnyKey")); // Ask the user to press any key to continue
        Console.ReadKey();
    }
    
    public static void DisplayLog() // Function to display the log file
    {
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "EnterDate")); // Ask the user to enter a date
        string? inputDate = Console.ReadLine(); // Get the user input

        if (inputDate != null) DisplayLogFile(inputDate); // If the user input is not null
        else Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "InvalidDateFormat")); // If the user input is null
    }
}
