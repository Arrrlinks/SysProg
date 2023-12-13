using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Documents;
using EasySave_Graphique.Models;
using Program.Models;

namespace EasySave_Graphique.ViewModels;

public class Save_vm : Base_vm
{
    private save_m _saveM;

    private format_m FormatM;
    private int select;
    

    private state_m _state;
    
    public RelayCommand LaunchCommand { get; set; }
    
    public RelayCommand SaveCheckCommand { get; set; }
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI
    
    private backup_m _selectedBackupM;
    //builder
    public Save_vm()
    {
        int select = 0;

        _saveM = new save_m();
        FormatM = new format_m();
        
        LaunchCommand = new RelayCommand(execute => Save());
        
        _state = new state_m();
        Backups = _state.GetBackupsFromStateFile();
    }
    //methods
    public backup_m SelectedBackupM
    {
        get { return _selectedBackupM; }
        set
        {
            _selectedBackupM = value;
            OnPropertyChanged();
        }
    }

    private void Save()
    {
        string extention = FormatM.RetrieveValueFromConfigFile("Extensions", "Extensions");
        dynamic tr = extention.Replace('[', ' ');
        foreach (var VARIABLE in tr)
        {
            Console.WriteLine(VARIABLE);
        }

        foreach (var backup in Backups)
        {
            if (backup.selected)
            {
                Console.WriteLine(backup.Name);
                //_saveM.SaveLaunch(backup.Source, backup.Target, backup.Name);
                
            }
        }
    }
    
}