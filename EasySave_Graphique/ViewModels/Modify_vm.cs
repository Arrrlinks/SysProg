using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.ViewModels;

public class Modify_vm : Base_vm
{
    //attributs
    private state_m _state; // Model for the state
    
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI
    
    private backup_m _selectedBackupM;
    //builder
    public Modify_vm()
    {
        _state = new state_m(); // Create a new state model
        Backups = _state.GetBackupsFromStateFile(); // Get the backups from the state file
        public RelayCommand AddCommand {get; set;}
        public RelayCommand RemoveCommand {get; set;}
    
        private backup _selectedBackup;
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
    
    private void removeBackup()
    {
        Backups.Remove(SelectedBackup);
    }
    private void ModifyBackup()
    {
        int i = 0;
        //modify the selected backup
        foreach (var backup in Backups)
        {
            if (backup.Name == SelectedBackup.Name)
            {
                i = 1;
                backup.Source = SelectedBackup.Source;
                backup.Target = SelectedBackup.Target;
                backup.Date = SelectedBackup.Date;
                backup.Size = SelectedBackup.Size;
                backup.filesNB = SelectedBackup.filesNB;
                backup.State = SelectedBackup.State;
            }
        }
        if (i==0)
        {
            Backups.Add(SelectedBackup);
        }
        
    }
}