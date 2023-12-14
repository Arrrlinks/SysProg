using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EasySave_Graphique.Models;
using EasySave_Graphique.ViewModels;

namespace EasySave_Graphique.View.SaveView;

public partial class Save : UserControl
{
    private static Save_vm vm;
    public Save()
    {
        InitializeComponent();
        Save_vm vm = new Save_vm();
        DataContext = vm;
    }
}