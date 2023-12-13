using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.ViewModels;

public class Save_vm : Base_vm
{
    private int select;
    
    List<backup_m> backupsChecked;

    private state_m _state;
    
    public RelayCommand LaunchCommand { get; set; }
    
    public RelayCommand SaveCheckCommand { get; set; }
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI
    
    private backup_m _selectedBackupM;
    //builder
    public Save_vm()
    {
        int select = 0;
        
        LaunchCommand = new RelayCommand(execute => Save());
        SaveCheckCommand = new RelayCommand(execute => CheckboxChecked());
        
        _state = new state_m();
        backupsChecked = new List<backup_m>();
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

    public void CheckboxChecked()
    {
        Console.WriteLine("hhh");
        //ajoute ou supprime les backups de la liste des backups à sauvegarder selon si la checkbox est cochée ou non
        /*    
        if (backup.selected)
            {
                backupsChecked.Add(backup);
                backup.selected = true;
            }
            else
            {
                backupsChecked.Remove(backup);
                backup.selected = false;
            }

            foreach (var bac in backupsChecked)
            {
                Console.WriteLine(bac.Name);
            } */
    }

    private void Save()
    {
        foreach (var backup in Backups)
        {
            if (backup.Selected)
            {
                Console.WriteLine(backup.Name);
            }
        }
    }
    
}