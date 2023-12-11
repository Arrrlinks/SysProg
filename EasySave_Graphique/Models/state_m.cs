using System.Collections.ObjectModel;

namespace EasySave_Graphique.Models;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

public class state_m
{
    public string? GetDate() // Function to get the current date in the format iso 8601
    {
        string? date = DateTime.UtcNow.ToString("o"); // Get the current date in the format iso 8601
        return date; // Return the current date in the format iso 8601
    }
    
    public dynamic RetrieveValueFromStateFile(string? itemName, string? key) // Function to retrieve a value from a json file
    {
        try // Try to retrieve the value from the json file
        {
            JArray jsonArray = JArray.Parse(File.ReadAllText("../../../state.json")); // Parse the content of the json file

            JObject jsonObject = jsonArray.Children<JObject>() // Get the json object
                .FirstOrDefault(item => item["Name"] != null && item["Name"].ToString() == itemName); // Get the json object

            if (jsonObject != null) // If the json object exists
            {
                if (jsonObject.TryGetValue(key, out var value)) // If the json object has the key
                {
                    switch (value.Type) // Switch on the type of the value
                    {
                        case JTokenType.String:
                            return value.ToString();
                        case JTokenType.Integer:
                            return value.ToObject<int>();
                        case JTokenType.Float:
                            return value.ToObject<float>();
                        case JTokenType.Boolean:
                            return value.ToObject<bool>();
                        // Add more cases here for other types as needed
                        default:
                            return null;
                    }
                }
                else // If the json object doesn't have the key
                {
                    return null; // Return null
                }
            }
            else // If the json object doesn't exist
            {
                return null; // Return null
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"[RetrieveValueFromStateFile] An error occurred: {ex.Message}"); // Display an error message
            return null; // Return null
        }
    }
    
    public ObservableCollection<backup_m> GetBackupsFromStateFile()
    {
        ObservableCollection<backup_m> backups = new ObservableCollection<backup_m>();
        string jsonContent = File.ReadAllText("../../../state.json");
        JArray jsonArray = JArray.Parse(jsonContent);
        foreach (JObject jsonObject in jsonArray.Children<JObject>())
        {
            backups.Add(new backup_m()
            {
                Name = jsonObject["Name"]?.ToString(),
                Source = jsonObject["SourcePath"]?.ToString(),
                Target = jsonObject["TargetPath"]?.ToString(),
                Date = jsonObject["Date"]?.ToString(),
                Size = $"{jsonObject["Size"]} MB",
                filesNB = jsonObject["TotalFiles"]?.ToString(),
                State = jsonObject["Status"]?.ToString()
            });
        }

        return backups;
    }
    
    public void ModifyJsonFile(string filePath, string itemName, string key, object? newValue) // Function to modify a json file
    // ModifyJsonFile("../../../state.json", "Save1", "SourcePath", "C:/Users/alexa/Desktop/ESGI/ESGI 3/Projet C#/EasySave_CLI/Files/Save1");
    {
        try // Try to modify the json file
        {

            string jsonContent = File.ReadAllText(filePath); // Get the content of the json file
            JArray jsonArray = JArray.Parse(jsonContent); // Parse the content of the json file

            JObject jsonObject = jsonArray.Children<JObject>() // Get the json object
                .FirstOrDefault(item => item["Name"] != null && item["Name"].ToString() == itemName); // Get the json object

            if (jsonObject != null) // If the json object exists
            {
                JToken newJTokenValue = JToken.FromObject(newValue); // Get the new value of the json object
                jsonObject[key] = newJTokenValue; // Set the new value of the json object

                string updatedJsonContent = jsonArray.ToString(); // Get the updated content of the json file
                File.WriteAllText(filePath, updatedJsonContent); // Write the updated content of the json file
            }
            else // If the json object doesn't exist
            {
                Console.WriteLine($"[ModifyJsonFile] No element with the name {itemName} was found in the file {filePath}."); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"[ModifyJsonFile] An error occured : {ex.Message}"); // Display an error message
        }
    }
    
    public void AddEntryToStateFile(string inputEntry) // Function to add an entry to the state json file
    {
        try // Try to add the entry to the state json file
        {
            string filePath = "../../../state.json"; // Get the path of the state json file
            if (File.Exists(filePath)) // If the state json file exists
            {
                string jsonContent = File.ReadAllText(filePath); // Get the content of the state json file
                JArray jsonArray = JArray.Parse(jsonContent); // Parse the content of the state json file
                JObject newEntry = JObject.Parse(inputEntry); // Get the new entry
                jsonArray.Add(newEntry); // Add the new entry to the state json file
                string updatedJsonContent = jsonArray.ToString(); // Get the updated content of the state json file
                File.WriteAllText(filePath, updatedJsonContent); // Write the updated content of the state json file
            }
            else // If the state json file doesn't exist
            {
                Console.WriteLine($"[AddEntryToStateFile] The file {filePath} doesn't exist."); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"[AddEntryToStateFile] An error occured : {ex.Message}"); // Display an error message
        }
    }
    
    public void ModifyStateFile(string name, string? source, string? target, double[] fileSizeList, string? status, int iteration = 0, bool isComplete = false) // Function to modify a save in the history
    {
        ModifyJsonFile("../../../state.json", name, "Date", GetDate()); // Modify the date of the save in the history
        if (source != null) ModifyJsonFile("../../../state.json", name, "SourcePath", @source); // Modify the source path of the save in the history
        if (target != null) ModifyJsonFile("../../../state.json", name, "TargetPath", @target); // Modify the target path of the save in the history
        ModifyJsonFile("../../../state.json", name, "Size", fileSizeList[0]); // Modify the size of the save in the history
        ModifyJsonFile("../../../state.json", name, "TotalFiles", fileSizeList[1]); // Modify the number of files of the save in the history
        ModifyJsonFile("../../../state.json", name, "FilesCopied", iteration); // Modify the number of files copied of the save in the history
        ModifyJsonFile("../../../state.json", name, "FilesRemaining", fileSizeList[1] - iteration); // Modify the number of files remaining of the save in the history
        ModifyJsonFile("../../../state.json", name, "isComplete", isComplete); // Modify the completeness of the save in the history
        ModifyJsonFile("../../../state.json", name, "Status", status); // Modify the status of the save in the history
    }
}
