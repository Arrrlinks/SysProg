using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using EasySave_Graphique.ViewModels;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EasySave_Graphique.View.SettingsView;

public partial class Settings : UserControl
{
    private readonly Settings_vm _settingsViewModel = new();
    private static readonly object _lock = new object();
    public Settings()
    {
        InitializeComponent();
        this.Loaded += Settings_Loaded;
    }

    private void Settings_Loaded(object sender, RoutedEventArgs e)
    {
        lock (_lock)
        {
            ExtensionsListBox.Items.Clear();
            string configJson = File.ReadAllText("../../../config.json");
            var config = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(configJson);
            var extensionsItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Extensions");


            if (extensionsItem != null)
            {
                foreach (var extension in extensionsItem["Extensions"].EnumerateArray())
                {
                    ExtensionsListBox.Items.Add("." + extension.GetString());
                }
            }
            var languageItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Lang");
            if (languageItem != null)
            {
                LanguageComboBox.SelectionChanged -= LanguageComboBox_SelectionChanged;
                LanguageComboBox.SelectedItem = LanguageComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Name == languageItem["Lang"].ToString());
                LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;

                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(languageItem["Lang"].ToString());
                EasySave_Graphique.language.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            var formatItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Format");
            if (formatItem != null)
            {
                LogFileComboBox.SelectedItem = formatItem["Format"].ToString() == "json" ? LogFileComboBox.Items[0] : LogFileComboBox.Items[1];
            }
            var saveModeItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "SaveMode");
            if (saveModeItem != null)
            {
                SaveModeComboBox.SelectedItem = saveModeItem["SaveMode"].ToString() == "complete" ? SaveModeComboBox.Items[0] : SaveModeComboBox.Items[1];
            }
            var SizeLimitItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "SizeLimit");
            if (SizeLimitItem != null)
            {
                SizeLimitTextBox.Text = SizeLimitItem["SizeLimit"].ToString();
            }
        }
    }

    private void ExtensionTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string enteredExtension = ExtensionTextBox.Text.Trim();
            if (!enteredExtension.StartsWith("."))
            {
                enteredExtension = "." + enteredExtension;
            }
            if (!string.IsNullOrEmpty(enteredExtension) && !ExtensionsListBox.Items.Contains(enteredExtension))
            {
                ExtensionsListBox.Items.Add(enteredExtension);
            }
            ExtensionTextBox.Clear();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedExtensions = new List<object>(ExtensionsListBox.SelectedItems.Cast<object>());
        foreach (var extension in selectedExtensions)
        {
            ExtensionsListBox.Items.Remove(extension);
        }
        _settingsViewModel.UpdateConfigFile("Extensions");
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _settingsViewModel.Extensions.Clear();
        foreach (var item in ExtensionsListBox.Items)
        {
            _settingsViewModel.Extensions.Add(item.ToString().Replace(".", ""));
        }
        _settingsViewModel.UpdateConfigFile("Extensions");
        _settingsViewModel.ChangeLanguage(_settingsViewModel.Language);
    }
    
    private void SizeLimitTextBox_TextInput(object sender, KeyEventArgs keyEventArgs)
    {
        _settingsViewModel.SizeLimit = SizeLimitTextBox.Text;
        _settingsViewModel.UpdateConfigFile("SizeLimit");
    }
    
    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateLanguageSetting();
    }

    public void UpdateLanguageSetting()
    {
        string selectedLanguage = ((ComboBoxItem)LanguageComboBox.SelectedItem).Content.ToString();
        _settingsViewModel.Language = selectedLanguage == "French" ? "fr" : "en";
        _settingsViewModel.UpdateConfigFile("Lang");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(_settingsViewModel.Language);
        EasySave_Graphique.language.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
    }

    private void LogFileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedLogFileType = ((ComboBoxItem)LogFileComboBox.SelectedItem).Content.ToString();
        _settingsViewModel.Format = selectedLogFileType == "JSON" ? "json" : "xml";
        _settingsViewModel.UpdateConfigFile("Format");
    }
    
    private void SaveModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string? selectedSaveMode = ((ComboBoxItem)SaveModeComboBox.SelectedItem).Content.ToString()?.Replace("è", "e");
        _settingsViewModel.SaveMode = selectedSaveMode == "Complete" ? "complete" : "differential";
        _settingsViewModel.UpdateConfigFile("SaveMode");
    }
    
    
}