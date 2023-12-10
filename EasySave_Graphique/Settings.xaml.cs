using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySave_Graphique
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            this.Loaded += Settings_Loaded;
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            string configJson = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(configJson);
            var extensionsItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Extensions");

            if (extensionsItem != null)
            {
                // If the dictionary was found, add the extensions to the ExtensionsListBox
                foreach (var extension in extensionsItem["Extensions"].EnumerateArray())
                {
                    ExtensionsListBox.Items.Add("." + extension.GetString());
                }
            }
            var languageItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "ChosenLanguage");
            if (languageItem != null)
            {
                LanguageComboBox.SelectedItem = languageItem["Lang"].ToString() == "fr" ? LanguageComboBox.Items[0] : LanguageComboBox.Items[1];
            }
            var formatItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Format");
            if (formatItem != null)
            {
                LogFileComboBox.SelectedItem = formatItem["Format"].ToString() == "json" ? LogFileComboBox.Items[0] : LogFileComboBox.Items[1];
            }
        }
        
        private void ExtensionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.Key == Key.Enter)
            {
                // Get the entered extension
                string enteredExtension = ExtensionTextBox.Text.Trim();

                // Add a dot at the beginning if it's not there
                if (!enteredExtension.StartsWith("."))
                {
                    enteredExtension = "." + enteredExtension;
                }

                // Check if the extension is not empty and is not already in the ListBox
                if (!string.IsNullOrEmpty(enteredExtension) && !ExtensionsListBox.Items.Contains(enteredExtension))
                {
                    // Add the extension to the ListBox
                    ExtensionsListBox.Items.Add(enteredExtension);
                }

                // Clear the TextBox
                ExtensionTextBox.Clear();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Copy the selected items to a separate list
            var selectedExtensions = new List<object>(ExtensionsListBox.SelectedItems.Cast<object>());

            // Remove the selected extensions from the ListBox
            foreach (var extension in selectedExtensions)
            {
                ExtensionsListBox.Items.Remove(extension);
            }

            // Update the config.json file
            UpdateConfigFile("Extensions", string.Join(",", ExtensionsListBox.Items));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the items from the ListBox
            string extensions = string.Join(",", ExtensionsListBox.Items);

            // Remove the dots from the extensions
            extensions = extensions.Replace(".", "");

            // Update the config.json file
            UpdateConfigFile("Extensions", extensions);

            // Close the window
            Window.GetWindow(this).Close();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected language
            string selectedLanguage = ((ComboBoxItem)LanguageComboBox.SelectedItem).Content.ToString();

            // Map the selected value to the value that should be saved in the config.json file
            string languageToSave = selectedLanguage == "French" ? "fr" : "en";

            // Update the config.json file
            UpdateConfigFile("ChosenLanguage", languageToSave);
        }

        private void LogFileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected log file type
            string selectedLogFileType = ((ComboBoxItem)LogFileComboBox.SelectedItem).Content.ToString();

            // Map the selected value to the value that should be saved in the config.json file
            string logFileTypeToSave = selectedLogFileType == "JSON" ? "json" : "xml";

            // Update the config.json file
            UpdateConfigFile("Format", logFileTypeToSave);
        }

        private void UpdateConfigFile(string key, string value)
        {
            string configFilePath = "config.json";
            List<Dictionary<string, object>> config;

            // Check if the config.json file exists
            string configJson;
            if (!File.Exists(configFilePath) || string.IsNullOrWhiteSpace(File.ReadAllText(configFilePath)))
            {
                // If the file does not exist or is empty, create a list with default values
                config = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "Name", "ChosenLanguage" }, { "Lang", "fr" } },
                    new Dictionary<string, object> { { "Name", "Format" }, { "Format", "json" } },
                    new Dictionary<string, object> { { "Name", "Extensions" }, { "Extensions", new List<string> { "txt", "json" } } }
                };
            }
            else
            {
                try
                {
                    // If the file exists, read it
                    configJson = File.ReadAllText(configFilePath);

                    // Try to parse the JSON into a list of dictionaries
                    config = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(configJson);
                }
                catch (JsonException)
                {
                    // If the JSON is invalid, create a list with default values
                    config = new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object> { { "Name", "ChosenLanguage" }, { "Lang", "fr" } },
                        new Dictionary<string, object> { { "Name", "Format" }, { "Format", "json" } },
                        new Dictionary<string, object> { { "Name", "Extensions" }, { "Extensions", new List<string> { "txt", "json" } } }
                    };
                }
            }

            // Find the dictionary in the list that has the given key as the "Name" value
            var item = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == key);

            if (item != null)
            {
                // If the dictionary was found, update its second value
                if (key == "ChosenLanguage" || key == "Format")
                {
                    item[key] = value;
                }
                else if (key == "Extensions")
                {
                    item[key] = ExtensionsListBox.Items.Cast<string>().Select(i => i.TrimStart('.')).ToList();
                }
            }

            // Serialize the list back to JSON
            configJson = JsonSerializer.Serialize(config);

            // Write the updated JSON back to the config.json file
            File.WriteAllText(configFilePath, configJson);
        }
    }
}