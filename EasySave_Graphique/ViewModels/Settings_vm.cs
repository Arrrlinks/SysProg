using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace EasySave_Graphique.ViewModels;

public class Settings_vm
{
    public ObservableCollection<string> _languages { get; set; }
    public ObservableCollection<string> _logTypes { get; set; }
    
    public Settings_vm()
    {
        _languages = new ObservableCollection<string>() { "English", "French" };
        _logTypes = new ObservableCollection<string>() { "json", "XML" };
    }
}