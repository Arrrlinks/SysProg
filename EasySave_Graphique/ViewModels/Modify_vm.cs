using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.ViewModels;

public class Modify_vm : Base_vm
{
    //attributs
    public ObservableCollection<backup> Backups { get; set; } //list of backups that update UI
    
    public RelayCommand AddCommand {get; set;}
    public RelayCommand RemoveCommand {get; set;}
    
    private backup _selectedBackup;
    
    //builder
    public Modify_vm()
    {
        //List of backups
        Backups = new ObservableCollection<backup>();
        //commands
        
        //relais command for the buttons
        AddCommand = new RelayCommand(execute => ModifyBackup());
        RemoveCommand = new RelayCommand(execute => removeBackup());
        
        Backups.Add(new backup
        {
            Name = "save1",
            Source = "C:/Users/Utilisateur/Desktop/Source",
            Target = "C:/Users/Utilisateur/Desktop/Target",
            Date = "01/01/2021",
            Size = "100Mo",
            filesNB = "10", 
            State = "Success"
        });
    }
    //methods
    public backup SelectedBackup
    {
        get { return _selectedBackup; }
        set
        {
            _selectedBackup = value; 
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