namespace EasySave_CLI.Views; // Namespace for the views

public class history_v // View for the history
{
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
                    Console.WriteLine($"Logs from {selectedDate.ToString("dd/MM/yyyy")} :\n{fileContent}"); // Display the file content
                }
                else // If the file doesn't exist
                {
                    Console.WriteLine($"File for the date {selectedDate.ToString("dd/MM/yyyy")} doesn't exists."); // Display an error message
                }
            }
            else // If the date is not valid
            {
                Console.WriteLine("Invalid date format. Please use the format dd/MM/yyyy."); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"An error occured : {ex.Message}"); // Display an error message
        }
        Console.WriteLine("Press any key to continue..."); // Ask the user to press a key to continue
        Console.ReadKey();
    }
    
    public static void DisplayLog() // Function to display the log file
    {
        Console.WriteLine("Choose a date (JJ/MM/YYYY) : "); // Ask the user to choose a date
        string? inputDate = Console.ReadLine(); // Get the user input

        if (inputDate != null) DisplayLogFile(inputDate); // If the user input is not null
        else Console.WriteLine("Invalid date format. Please use the format dd/MM/yyyy."); // If the user input is null
    }
}
