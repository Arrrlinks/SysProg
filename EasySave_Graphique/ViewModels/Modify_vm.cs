using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.ViewModels;

public class Modify_vm : Base_vm
{
    //attributs
    public RelayCommand AddCommand {get; set;}
    public RelayCommand RemoveCommand {get; set;}
    private state_m _state; // Model for the state
    
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI
    
    private backup_m _selectedBackup;
    //builder
    public Modify_vm()
    {
        _state = new state_m(); // Create a new state model
        RemoveCommand = new RelayCommand(execute => removeBackup(), canExecute => _selectedBackup != null);
        AddCommand = new RelayCommand(execute => addBackup());
        Backups = _state.GetBackupsFromStateFile(); // Get the backups from the state file
        SelectedBackup = new backup_m();
    }
    //methods
    public backup_m SelectedBackup
    {
        get { return _selectedBackup; }
        set
        {
            _selectedBackup = value; 
            OnPropertyChanged();
        }
    }
    
    private void addBackup()
    {
        Backups.Add(new backup_m()
        {
            Name = "",
            Source = "",
            Target = "",
            Date = "---",
            Size = "0Mo",
            filesNB = "0", 
            State = "Stop"
        });
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