using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using EasySave_Graphique.Models;
using Application = System.Windows.Forms.Application;

namespace EasySave_Graphique.ViewModels;

public class Settings_vm
{
    private readonly Settings_m _settingsModel = new();
    public ObservableCollection<string> Extensions { get; set; } = new();
    public string Language { get; set; }
    public string Format { get; set; }
    public string SaveMode { get; set; }

    public void UpdateConfigFile(string key)
    {
        string value;
        switch (key)
        {
            case "Extensions":
                value = string.Join(",", Extensions);
                break;
            case "Lang":
                value = Language;
                break;
            case "Format":
                value = Format;
                break;
            case "SaveMode":
                value = SaveMode;
                break;
            default:
                throw new ArgumentException($"Invalid key: {key}");
        }
        _settingsModel.UpdateConfigFile(key, value);
    }
    
    public void ChangeLanguage(string language)
    {
        if (language != "en" && language != "fr") return;
        CultureInfo cultureInfo = new CultureInfo(language);
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        UpdateConfigFile("Lang");
        Application.Restart();
        Environment.Exit(0);
    }
}