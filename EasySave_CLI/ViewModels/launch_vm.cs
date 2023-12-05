using EasySave_CLI.Models; // Namespace for the models
using EasySave_CLI.Views; // Namespace for the views
using Program.Models; // Namespace for the models

namespace EasySave_CLI.ViewModels; // Namespace for the view models

public class launch_vm // View model for the launch
{
    //Attributes
    private launch_v _view; // View for the launch
    private save_m _save; // Model for the saves
    private log_m _log; // Model for the history

    //Builders
    public launch_vm() // Builder for the launch
    {
        _view = new launch_v(); // Create a new view for the launch
        _save = new save_m(); // Create a new model for the saves
        _log = new log_m(); // Create a new model for the history
    }
    public launch_vm(launch_v view, save_m save, log_m log) // Builder for the launch
    {
        _view = view; // Set the view for the launch
        _save = save; // Set the model for the saves
        _log = log; // Set the model for the history
    }
    
    static bool IsDirectory(string path) // Function to check if a path is a directory
    {
        try // Try to check if the path is a directory
        {
            return Directory.Exists(path); // Return if the path is a directory
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"Error checking directory: {ex.Message}"); // Display an error message
            return false; // Return false
        }
    }

    public void SaveBackup(string? source, string? target, string? name) // Function to save a backup
    {
        if (@source != null && IsDirectory(@source)) // If the source path is a directory
        {
            _save.SaveLaunch(source, target, name); // Save the backup
        }
        else // If the source path is a file
        {
            _save.CopyFile(source, target, source, name); // Save the backup
        }
    }
    
    //Methods
    public List<string> SetBackup() // Function to set the name of the save
    {
        List<string> backups = _view.SetBackup(); // Get the name of the save
        return backups; // Return the name of the save
    }

    public void Run() // Function to run the backup
    {
        List<string> backups = SetBackup(); // Get the name of the save
        foreach (string backup in backups) // For each save
        {
            string? name = _log.RetrieveValueFromStateFile(backup, "Name"); // Get the name of the save
            string? source = _log.RetrieveValueFromStateFile( backup, "SourcePath"); // Get the source path of the save
            string? target = _log.RetrieveValueFromStateFile( backup, "TargetPath"); // Get the target path of the save
            _view.DisplaySave(name, source, target); // Display the save
            SaveBackup(source, target, name); // Save the backup
        }
    }
}