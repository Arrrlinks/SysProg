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
}