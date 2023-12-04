using Newtonsoft.Json.Linq; // Library for json

namespace EasySave_CLI.Models; // Namespace for the models

public class history_m //Model for the history
{
    static void CreateTodayLogFile(string filePath, string fileName) // Function to create the today date log json file
    {
        try // Try to create the json file
        {
            string fullPath = Path.Combine(filePath, fileName); // Get the full path of the json file
            if (!File.Exists(fullPath)) // If the json file doesn't exist
            {
                File.WriteAllText(fullPath, "[]"); // Create the json file
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"An error occured : {ex.Message}"); // Display an error message
        }
    }

    private static string DateFileNameFormat() // Function to get the date in the format "YYYYMMDD"
    {
        string date = DateTime.UtcNow.ToString("o"); // Get the date in the format "YYYY-MM-DDTHH:MM:SS.0000000Z"
        string dateForFile = date.Substring(0, 4) + date.Substring(5, 2) + date.Substring(8, 2); // Get the date in the format "YYYYMMDD"
        return dateForFile; // Return the date in the format "YYYYMMDD"
    }

    public static void ModifyJsonFile(string filePath, string itemName, string key, string newValue) // Function to modify the json file values
    {
        try // Try to modify the json file
        {
            string fileName = DateFileNameFormat();
            CreateTodayLogFile("../../../logs/", $"{fileName}.json");
            string jsonContent = File.ReadAllText(filePath); // Read the json file
            JArray jsonArray = JArray.Parse(jsonContent); // Parse the json file
            JObject jsonObject = jsonArray.Children<JObject>() // Get the json object
                .FirstOrDefault(item => item["Name"] != null && item["Name"].ToString() == itemName); // Get the json object

            if (jsonObject != null) // If the json object is not null
            {
                jsonObject[key] = newValue; // Modify the json object
                string updatedJsonContent = jsonArray.ToString(); // Get the json object
                File.WriteAllText(filePath, updatedJsonContent); // Write the json object
            }
            else // If the json object is null
            {
                Console.WriteLine($"No element with the name {itemName} was found in the file {filePath}."); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"An error occured : {ex.Message}"); // Display an error message
        }
    }
    
    public static void AddEntryToLogFile(string inputEntry) // Function to add an entry to the log json file
    {
        try // Try to add an entry to the log json file
        {
            string fileName = DateFileNameFormat(); // Get the date in the format "YYYYMMDD"
            string filePath = $"../../../logs/{fileName}.json"; // Get the full path of the log json file
            CreateTodayLogFile("../../../logs/", $"{fileName}.json"); // Create the log json file if it doesn't exist
            if (File.Exists(filePath)) // If the log json file exists
            {
                string jsonContent = File.ReadAllText(filePath); // Read the log json file
                JArray jsonArray = JArray.Parse(jsonContent); // Parse the log json file
                JObject newEntry = JObject.Parse(inputEntry); // Parse the entry to add
                jsonArray.Add(newEntry); // Add the entry to the log json file
                string updatedJsonContent = jsonArray.ToString(); // Get the log json file
                File.WriteAllText(filePath, updatedJsonContent); // Write the log json file
            }
            else // If the log json file doesn't exist
            {
                Console.WriteLine($"The file {filePath} doesn't exist."); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"An error occured : {ex.Message}"); // Display an error message
        }
    }
}