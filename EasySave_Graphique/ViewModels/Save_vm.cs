using System;
using System.Collections.ObjectModel;
using EasySave_Graphique.Models;
using Program.Models;

namespace EasySave_Graphique.ViewModels;

public class Save_vm : Base_vm
{
    private save_m _saveM;
    
    private int select;
    

    private state_m _state;
    
    public RelayCommand LaunchCommand { get; set; }
    
    public RelayCommand SelectAllCommand { get; set; }
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI
    
    private backup_m _selectedBackupM;
    //builder
    public Save_vm()
    {
        int select = 0;

        _saveM = new save_m();
        
        LaunchCommand = new RelayCommand(execute => Save());
        SelectAllCommand = new RelayCommand(execute => SelectAll());
        
        _state = new state_m();
        Backups = _state.GetBackupsFromStateFile();

        Modify_vm.BackupUpdated += UpdateSaveMenu;
    }

    private void UpdateSaveMenu()
    {
        Backups.Clear();
        ObservableCollection<backup_m> updatedBackups = _state.GetBackupsFromStateFile();
        foreach (var backup in updatedBackups)
        {
            Backups.Add(backup);
        }
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
        _saveM.SaveLaunch(SelectedBackupM.Source, SelectedBackupM.Target, SelectedBackupM.Name);

        //string extention = FormatM.RetrieveValueFromConfigFile("Extensions", "Extensions");
        //dynamic tr = extention.Replace('[', ' ');

        foreach (var backup in Backups)
        {
            if (backup.Selected)
            {
                _saveM.SaveLaunch(backup.Source, backup.Target, backup.Name);
                
            }
        }
    }

    private void SelectAll()
    {
        foreach (var backup in Backups)
        {
            if (backup.Selected)
            {
                backup.Selected = false;
            }
            else
            {
                backup.Selected = true;
            }
        }
    }
}