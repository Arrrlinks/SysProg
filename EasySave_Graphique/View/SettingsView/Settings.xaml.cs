using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using EasySave_Graphique.Models;
using EasySave_Graphique.ViewModels;

namespace EasySave_Graphique.View.SettingsView;

public partial class Settings : UserControl
{

    public Settings()
    {
        InitializeComponent();
        Settings_vm vm = new Settings_vm();
        DataContext = vm;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
    }
}