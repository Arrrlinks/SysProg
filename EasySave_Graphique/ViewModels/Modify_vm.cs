using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.ViewModels;

public class Modify_vm : Base_vm
{
    //attributs
    private state_m _state; // Model for the state
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI

    private FolderBrowserDialog _dialog;
    
    public RelayCommand AddCommand {get; set;}
    public RelayCommand RemoveCommand {get; set;}
    
    public RelayCommand SendCommand { get; set;  }
    
    public RelayCommand SourceCommand { get; set; }
    
    public RelayCommand TargetCommand { get; set; }
    private backup_m _selectedBackupM;
    
    //builder
    public Modify_vm()
    {
        _state = new state_m(); // Create a new state model
        _dialog = new FolderBrowserDialog();
        //List of backups
        //commands
        
        //relais command for the buttons
        SendCommand = new RelayCommand(execute => ModifyBackup());
        RemoveCommand = new RelayCommand(execute => removeBackup(), canExecute => _selectedBackupM != null);
        AddCommand = new RelayCommand(execute => addBackup());
        SourceCommand = new RelayCommand(execute => sourceGet(), canExecute => _selectedBackupM != null);
        TargetCommand = new RelayCommand(execute => targetGet(), canExecute => _selectedBackupM != null);
        Backups = _state.GetBackupsFromStateFile(); // Get the backups from the state file
    }
    //methods
    public backup_m SelectedBackup
    {
        get { return _selectedBackupM; }
        set
        {
            _selectedBackupM = value; 
            OnPropertyChanged();
        }
    }

    private void sourceGet()
    {
        _dialog.ShowDialog();
        string repo = _dialog.SelectedPath;
        
        if (Directory.Exists(repo))
        {
            SelectedBackup.Source = repo;
        }
        else
        {
            Console.WriteLine("bread");
        }
    }
    
    private void targetGet()
    {
        _dialog.ShowDialog();
        string repo = _dialog.SelectedPath;
        
        if (Directory.Exists(repo))
        {
            SelectedBackup.Target = repo;
        }
        else
        {
            Console.WriteLine("bread");
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
        foreach (var backup in Backups)
        {
            //envoi le backup dans la list, si il existe, il le modifie, sinon il le créer. a voir dans le log
            foreach (var f in Backups)
            {
                
            }
        }
    }
}