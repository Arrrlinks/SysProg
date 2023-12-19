using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
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

    public RelayCommand SourceCommand { get; set; }
    
    public RelayCommand TargetCommand { get; set; }
    
    public RelayCommand SaveCommand { get; set;  }
    private backup_m _selectedBackupM;
    
    public static event Action BackupUpdated;
    
    //builder
    public Modify_vm()
    {
        _state = new state_m(); // Create a new state model
        _dialog = new FolderBrowserDialog();
        //List of backups
        //commands
        
        //relais command for the buttons
        RemoveCommand = new RelayCommand(execute => removeBackup(), canExecute => _selectedBackupM != null);
        AddCommand = new RelayCommand(execute => addBackup());
        SourceCommand = new RelayCommand(execute => sourceGet(), canExecute => _selectedBackupM != null);
        TargetCommand = new RelayCommand(execute => targetGet(), canExecute => _selectedBackupM != null);
        SaveCommand = new RelayCommand(execute => ReplaceStateFile());
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
            Date = _state.GetDate(),
            Size = "0",
            FilesNB = "0", 
            State = "Not Started"
        });
    }
    
    private void removeBackup()
    {
        Backups.Remove(SelectedBackup);
    }

    private void ReplaceStateFile()
    {
        _state.ReplaceStateFile(Backups);
        BackupUpdated?.Invoke();
        
    }
}