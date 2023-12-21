using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq; // Library for json
using System.Xml;
using System.Xml.Linq;

namespace EasySave_Graphique.Models; // Namespace for the models

public class log_m //Model for the history
{
    private static readonly object _lock = new object();
    private static readonly format_m _format = new format_m(); // Instance of the format model
    private static readonly string? format = _format.RetrieveValueFromConfigFile("Format", "Format");

    public log_m() {} // Builder for the history

    static void CreateTodayLogFile(string filePath, string fileName, string format = "json") // Function to create the today date log json file
    // CreateTodayLogFile("../../../logs/", $"{fileName}.json");
    {
        lock (_lock)
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
    }

    private static string DateFileNameFormat() // Function to get the date in the format "YYYYMMDD"
    // string fileName = DateFileNameFormat();
    {
        string date = DateTime.UtcNow.ToString("o"); // Get the date in the format "YYYY-MM-DDTHH:MM:SS.0000000Z"
        string dateForFile = date.Substring(0, 4) + date.Substring(5, 2) + date.Substring(8, 2); // Get the date in the format "YYYYMMDD"
        return dateForFile; // Return the date in the format "YYYYMMDD"
    }
    
    public ObservableCollection<history_m> GetHistoryFromLogFile(string date)
    {
        lock (_lock)
        {
            ObservableCollection<history_m> history = new ObservableCollection<history_m>();

            string jsonFilePath = $"../../../logs/{date}.json";
            string xmlFilePath = $"../../../logs/{date}.xml";

            if (File.Exists(jsonFilePath))
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                JArray jsonArray = JArray.Parse(jsonContent);
                foreach (JObject jsonObject in jsonArray.Children<JObject>())
                {
                    history.Add(new history_m()
                    {
                        Name = jsonObject["Name"]?.ToString(),
                        Source = jsonObject["SourcePath"]?.ToString(),
                        Target = jsonObject["TargetPath"]?.ToString(),
                        Date = jsonObject["Date"]?.ToString(),
                        FileName = jsonObject["FileName"]?.ToString(),
                        Size = $"{jsonObject["FileSize"]} MB",
                        TimeMs = $"{double.Parse(jsonObject["TimeMs"]?.ToString() ?? "-1")/1000:F2} s"
                    });
                }
            }

            if (File.Exists(xmlFilePath))
            {
                string xmlContent = File.ReadAllText(xmlFilePath);
                XDocument xmlDoc = XDocument.Parse(xmlContent);
                foreach (XElement element in xmlDoc.Root.Elements())
                {
                    history.Add(new history_m()
                    {
                        Name = element.Element("Name")?.Value.Trim(),
                        Source = element.Element("SourcePath")?.Value.Trim(),
                        Target = element.Element("TargetPath")?.Value.Trim(),
                        Date = element.Element("Date")?.Value.Trim(),
                        FileName = element.Element("FileName")?.Value.Trim(),
                        Size = $"{element.Element("FileSize")?.Value.Trim()} MB",
                        TimeMs = $"{element.Element("TimeMs")?.Value.Trim()} ms"
                    });
                }
            }

            // Sort the history by date in descending order
            return new ObservableCollection<history_m>(history.OrderByDescending(h => DateTime.Parse(h.Date)));
        }
    }

    public void AddEntryToLogFile(string inputEntry) // Function to add an entry to the today date log json file
    {
        lock (_lock)
        {
            try // Try to add the entry to the today date log json file
            {
                string fileName = DateFileNameFormat(); // Get the date in the format "YYYYMMDD"
                string filePath = $"../../../logs/{fileName}.{format}"; // Get the path of the today date log json file
                CreateTodayLogFile("../../../logs/", $"{fileName}.{format}",
                    format); // Create the today date log json file
                if (File.Exists(filePath)) // If the today date log json file exists
                {
                    if (format == "json") // If the format is json
                    {
                        string jsonContent =
                            File.ReadAllText(filePath); // Get the content of the today date log json file
                        JArray jsonArray =
                            JArray.Parse(jsonContent); // Parse the content of the today date log json file
                        JObject newEntry = JObject.Parse(inputEntry); // Get the new entry
                        jsonArray.Add(newEntry); // Add the new entry to the today date log json file
                        string updatedJsonContent =
                            jsonArray.ToString(); // Get the updated content of the today date log json file
                        File.WriteAllText(filePath,
                            updatedJsonContent); // Write the updated content of the today date log json file
                    }
                    else // If the format is xml
                    {
                        XmlDocument xmlDoc = new XmlDocument(); // Create a new xml document
                        xmlDoc.Load(filePath); // Load the xml document
                        XmlElement entryElement = xmlDoc.CreateElement("LogEntry"); // Create a new xml element
                        JObject entryObject = JObject.Parse(inputEntry); // Get the new entry
                        foreach (var property in entryObject.Properties()) // For each property of the new entry
                        {
                            XmlElement propertyElement =
                                xmlDoc.CreateElement(property.Name); // Create a new xml element
                            propertyElement.InnerText = property.Value.ToString(); // Set the value of the xml element
                            entryElement.AppendChild(propertyElement); // Add the xml element to the xml document
                        }

                        xmlDoc.DocumentElement.AppendChild(entryElement);
                        xmlDoc.Save(filePath);
                    }

                }
                else // If the today date log json file doesn't exist
                {
                    Console.WriteLine(
                        $"[AddEntryToLogFile] The file {filePath} doesn't exist."); // Display an error message
                }
            }
            catch (Exception ex) // If an error occured
            {
                Console.WriteLine($"[AddEntryToLogFile] An error occured : {ex.Message}"); // Display an error message
            }
        }
    }
}
