using Newtonsoft.Json.Linq; // Library for json
using System.Xml;
using System.Xml.Linq; // Library for xml

namespace EasySave_CLI.Models; // Namespace for the models

public class log_m //Model for the history
{
    private static readonly format_m _format = new format_m(); // Instance of the format model
    private static readonly string? format = _format.RetrieveValueFromConfigFile("Format", "Format");

    public log_m() {} // Builder for the history
    
    public string? GetDate() // Function to get the current date in the format iso 8601
    {
        string? date = DateTime.UtcNow.ToString("o"); // Get the current date in the format iso 8601
        return date; // Return the current date in the format iso 8601
    }
    

    static void CreateTodayLogFile(string filePath, string fileName, string format = "json") // Function to create the today date log json file
    // CreateTodayLogFile("../../../logs/", $"{fileName}.json");
    {
        try // Try to create the json file
        {
            string fullPath = Path.Combine(filePath, fileName); // Get the full path of the json file
            if (!File.Exists(fullPath)) // If the json file doesn't exist
            {
                if (format == "json")
                {
                    File.WriteAllText(fullPath, "[]"); // Create the json file
                }
                else
                {
                    File.WriteAllText(fullPath, "<Root></Root>"); // Create the json file
                }
                
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"[CreateTodayLogFile] An error occured : {ex.Message}"); // Display an error message
        }
    }

    private static string DateFileNameFormat() // Function to get the date in the format "YYYYMMDD"
    // string fileName = DateFileNameFormat();
    // CreateTodayLogFile("../../../logs/", $"{fileName}.json");
    {
        string date = DateTime.UtcNow.ToString("o"); // Get the date in the format "YYYY-MM-DDTHH:MM:SS.0000000Z"
        string dateForFile = date.Substring(0, 4) + date.Substring(5, 2) + date.Substring(8, 2); // Get the date in the format "YYYYMMDD"
        return dateForFile; // Return the date in the format "YYYYMMDD"
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

    
    public string? RetrieveValueFromStateFile(string? itemName, string? key) // Function to retrieve a value from a json file
    // RetrieveValueFromStateFile("Save1", "SourcePath");
    {
        try // Try to retrieve the value from the json file
        {
            string jsonContent = File.ReadAllText("../../../state.json"); // Get the content of the json file
            JArray jsonArray = JArray.Parse(jsonContent); // Parse the content of the json file

            JObject jsonObject = jsonArray.Children<JObject>() // Get the json object
                .FirstOrDefault(item => item["Name"] != null && item["Name"].ToString() == itemName); // Get the json object

            if (jsonObject != null) // If the json object exists
            {
            
                if (jsonObject.TryGetValue(key, out var value)) // If the json object has the key
                {
                    return value.ToString(); // Return the value of the key
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

    public void AddEntryToLogFile(string inputEntry) // Function to add an entry to the today date log json file
    {
        try // Try to add the entry to the today date log json file
        {
            string fileName = DateFileNameFormat(); // Get the date in the format "YYYYMMDD"
            string filePath = $"../../../logs/{fileName}.{format}"; // Get the path of the today date log json file
            CreateTodayLogFile("../../../logs/", $"{fileName}.{format}", format); // Create the today date log json file
            if (File.Exists(filePath)) // If the today date log json file exists
            {
                if (format == "json") // If the format is json
                {
                string jsonContent = File.ReadAllText(filePath); // Get the content of the today date log json file
                JArray jsonArray = JArray.Parse(jsonContent); // Parse the content of the today date log json file
                JObject newEntry = JObject.Parse(inputEntry); // Get the new entry
                jsonArray.Add(newEntry); // Add the new entry to the today date log json file
                string updatedJsonContent = jsonArray.ToString(); // Get the updated content of the today date log json file
                File.WriteAllText(filePath, updatedJsonContent); // Write the updated content of the today date log json file
                }
                else // If the format is xml
                {
                    XmlDocument xmlDoc = new XmlDocument(); // Create a new xml document
                    xmlDoc.Load(filePath); // Load the xml document
                    XmlElement entryElement = xmlDoc.CreateElement("LogEntry"); // Create a new xml element
                    JObject entryObject = JObject.Parse(inputEntry); // Get the new entry
                    foreach (var property in entryObject.Properties()) // For each property of the new entry
                    {
                        XmlElement propertyElement = xmlDoc.CreateElement(property.Name); // Create a new xml element
                        propertyElement.InnerText = property.Value.ToString(); // Set the value of the xml element
                        entryElement.AppendChild(propertyElement); // Add the xml element to the xml document
                    }
                    xmlDoc.DocumentElement.AppendChild(entryElement);
                    xmlDoc.Save(filePath);
                }

            }
            else // If the today date log json file doesn't exist
            {
                Console.WriteLine($"[AddEntryToLogFile] The file {filePath} doesn't exist."); // Display an error message
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"[AddEntryToLogFile] An error occured : {ex.Message}"); // Display an error message
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