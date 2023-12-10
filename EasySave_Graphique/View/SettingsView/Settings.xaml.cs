﻿using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using EasySave_Graphique.ViewModels;

namespace EasySave_Graphique.View.SettingsView;

public partial class Settings : UserControl
{
    private readonly Settings_vm _settingsViewModel = new();

    public Settings()
    {
        InitializeComponent();
        this.Loaded += Settings_Loaded;
    }

    private void Settings_Loaded(object sender, RoutedEventArgs e)
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
            LanguageComboBox.SelectedItem = languageItem["Lang"].ToString() == "fr" ? LanguageComboBox.Items[0] : LanguageComboBox.Items[1];
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(languageItem["Lang"].ToString());
            EasySave_Graphique.language.Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }
        var formatItem = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "Format");
        if (formatItem != null)
        {
            LogFileComboBox.SelectedItem = formatItem["Format"].ToString() == "json" ? LogFileComboBox.Items[0] : LogFileComboBox.Items[1];
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

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
}