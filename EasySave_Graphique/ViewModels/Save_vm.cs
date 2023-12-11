using System.Collections.ObjectModel;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.ViewModels;

public class Save_vm : Base_vm
{
    public ObservableCollection<backup_m> Backups { get; set; } //list of backups that update UI
    
    private backup_m _selectedBackup;
    //builder
    public Save_vm()
    {
        Backups = new ObservableCollection<backup_m>();
        Backups.Add(new backup_m()
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
    public backup_m SelectedBackup
    {
        get { return _selectedBackup; }
        set
        {
            _selectedBackup = value;
            OnPropertyChanged();
        }
    }
}