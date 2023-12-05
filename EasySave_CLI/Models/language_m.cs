using Newtonsoft.Json.Linq; // Library for json

namespace EasySave_CLI.Models;

public class language_m
{
    public string? RetrieveValueFromLanguageFile(string? itemName, string? key) // Function to retrieve a value from a json file
        // RetrieveValueFromStateFile("Save1", "SourcePath");
    {
        try // Try to retrieve the value from the json file
        {
            string jsonContent = File.ReadAllText("../../../language.json"); // Get the content of the json file
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
    
    public void ChangeLanguage(string lang) // Function to modify a json file
        // ChangeLanguage("en");
    {
        try // Try to modify the json file
        {
            string filePath = "../../../language.json"; // Get the path of the json file
            string jsonContent = File.ReadAllText(filePath); // Get the content of the json file
            JArray jsonArray = JArray.Parse(jsonContent); // Parse the content of the json file

            JObject jsonObject = jsonArray.Children<JObject>() // Get the json object
                .FirstOrDefault(item => item["Name"] != null && item["Name"].ToString() == "LanguageChosen"); // Get the json object

            if (jsonObject != null) // If the json object exists
            {
                JToken newJTokenValue = JToken.FromObject(lang); // Get the new value of the json object
                jsonObject["Lang"] = newJTokenValue; // Set the new value of the json object

                string updatedJsonContent = jsonArray.ToString(); // Get the updated content of the json file
                File.WriteAllText(filePath, updatedJsonContent); // Write the updated content of the json file
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"[ModifyJsonFile] An error occured : {ex.Message}"); // Display an error message
        }
    }
}