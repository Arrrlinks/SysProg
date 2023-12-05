namespace EasySave_CLI.Views; // Namespace for the views
using Newtonsoft.Json.Linq; // Library for json

public class state_v // View for the state
{
    public static void DisplayState() // Function to print the json file information
    { 
        try // Try to print the json file information
        {
            string jsonContent = File.ReadAllText("../../../state.json"); // Read the json file
            JArray jsonArray = JArray.Parse(jsonContent); // Parse the json file
            foreach (var item in jsonArray) // For each item in the json file
            {
                Console.WriteLine($"Name: {item["Name"]}"); // Display the name of the save
                Console.WriteLine($"    Date: {item["Date"]}"); // Display the date of the save
                Console.WriteLine($"    SourcePath: {item["SourcePath"]}"); // Display the source path of the save
                Console.WriteLine($"    TargetPath: {item["TargetPath"]}"); // Display the target path of the save
                Console.WriteLine($"    Size: {item["Size"]}"); // Display the size of the save
                Console.WriteLine($"    TotalFiles: {item["TotalFiles"]}"); // Display the total files of the save
                Console.WriteLine($"    FilesCopied: {item["FilesCopied"]}"); // Display the files copied of the save
                Console.WriteLine($"    FilesRemaining: {item["FilesRemaining"]}"); // Display the files remaining of the save
                Console.WriteLine($"    Status: {item["Status"]}"); // Display the status of the save
                Console.WriteLine(); // Display a blank line
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"Error reading or parsing the JSON file: {ex.Message}"); // Display an error message
        }
        Console.WriteLine("Press any key to continue..."); // Display a message
        Console.ReadKey(); // Wait for the user to press a key
    }
}