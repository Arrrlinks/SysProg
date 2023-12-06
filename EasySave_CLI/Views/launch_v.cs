using System.Text.RegularExpressions; // Namespace for the regex
using EasySave_CLI.Models; // Namespace for the models

namespace EasySave_CLI.Views; // Namespace for the views

public class launch_v // View for the launch
{
    private static readonly language_m _language = new language_m(); // Instance of the language model
    private static readonly string? lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang"); // Get the language chosen
    public launch_v() {} // Builder for the launch

    public List<string> SetBackup() // Function to set the name of the save
    {
        Console.Clear();
        List<string> backups = new List<string>(); // Create a list for the saves
        Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "ChooseBackups")); // Ask the user to enter the saves
        string command = Console.ReadLine(); // Get the command
        Match hyphen = Regex.Match(command, @"^(\d)-(\d)$"); // Get the saves with a hyphen
        MatchCollection semicolon = Regex.Matches(command, @"\d+"); // Get the saves with a semicolon
        
        //-
        if (hyphen.Success) // If the command is valid
        { 
            int firstBackup = int.Parse(hyphen.Groups[1].Value); // Get the first save
            int lastBackup = int.Parse(hyphen.Groups[2].Value); // Get the last save
            if (firstBackup >= 1 && firstBackup <= 5 && 
                lastBackup >= 1 && lastBackup <= 5 &&
                firstBackup < lastBackup) // If the saves are between 1 and 5 and the first save is before the last save
            {
                for (int i = firstBackup; i <= lastBackup; i++) // For each save
                {
                    backups.Add("Save"+i); // Add the save to the list
                }
            }
        }
        //;
        else if (semicolon.Count > 0) // If the command is valid
        {
            foreach (Match match in semicolon) // For each save
            {
                int backup = int.Parse(match.Value); // Get the save
                if (backup >= 1 && backup <= 5) // If the save is between 1 and 5
                {
                    backups.Add("Save"+backup); // Add the save to the list
                }
            }
        }
        //1
        else if (int.TryParse(command, out int number) && number >= 1 && number <= 5) // If the command is valid
        {
            backups.Add("Save"+number); // Add the save to the list
        }
        else // If the command is not valid
        {
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "CommandNotValid")); // Display an error message
            backups.AddRange(SetBackup()); // Get the saves
        }
        return backups; // Return the saves
    }

    public bool setMode()
    {
        Console.Clear();
        string? mode = String.Empty;
        while (mode != "1" && mode != "2")
        {
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "ChooseMode")); // Ask the user to choose the mode
            mode = Console.ReadLine(); // Get the mode
            if(mode != "1" && mode != "2")
            {
                Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "InvalidMode")); // Display an error message
            }
        }
        if (mode == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void DisplaySave(string name, string source, string target) // Function to display the save
    {
        Console.WriteLine($"{_language.RetrieveValueFromLanguageFile(lang, "CopyingFilesFrom")} {source} {_language.RetrieveValueFromLanguageFile(lang, "To")} {target} {_language.RetrieveValueFromLanguageFile(lang, "ForTheSave")} {name}"); // Display the save
    }
}
