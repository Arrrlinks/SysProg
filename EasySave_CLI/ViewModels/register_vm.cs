using EasySave_CLI.Models; // Models for the saves and the history
using EasySave_CLI.Views; // Views for the saves and the history
using Program.Models; // Namespace for the models

namespace EasySave_CLI.ViewModels; // Namespace for the view models

public class register_vm // View model for the register
{
    //Attributes
    private register_v _view; // View for the register
    private save_m _save; // Model for the saves
    private log_m _log; // Model for the history
    
    //Builders
    public register_vm() // Builder for the register
    {
        _view = new register_v(); // Create a new view for the register
        _save = new save_m(); // Create a new model for the saves
        _log = new log_m(); // Create a new model for the history
    }
    public register_vm(register_v view, save_m save, log_m log) // Builder for the register
    {
        _view = view; // Set the view for the register
        _save = save; // Set the model for the saves
        _log = log; // Set the model for the history
    }
    
    //Methods
    public string SetBackup() // Function to set the name of the save
    {
        int backupNumber = _view.SetBackup(); // Get the number of the save
        while (backupNumber < 1 || backupNumber > 5) // While the number of the save is not between 1 and 5
        {
            backupNumber = _view.SetBackup(1); // Get the number of the save
        }
        return "Save" + backupNumber; // Return the name of the save
    }
    
    public string? SetPath(string mode) // Function to set the path of the save
    {
        string? Path; // Create a string for the path of the save
        if (mode == "source") // If the mode is "source"
        {
            Path = _view.SetPath(0); // Get the path of the save
        }
        else // If the mode is "target"
        {
            Path = _view.SetPath(1); // Get the path of the save
        }
        return Path; // Return the path of the save
    }
    
    public void SetSaveName(string name) // Function to set the name of the save
    {
        _save._name = name; // Set the name of the save
    }
    
    public void SetSaveSource(string? source) // Function to set the source path of the save
    {
        _save._source = source; // Set the source path of the save
    }
    
    public void SetSaveTarget(string? target) // Function to set the target path of the save
    {
        _save._target = target; // Set the target path of the save
    }
    
    public void Run() // Function to run the register
    {
        SetSaveName(SetBackup()); // Set the name of the save
        
        SetSaveSource(SetPath("source")); // Set the source path of the save
        
        SetSaveTarget(SetPath("target")); // Set the target path of the save
        
        double[] list = _save.GetFileSize(_save._source); // Get the size of the save
        
        if (_log.RetrieveValueFromStateFile(_save._name, "Name") != null) // If the save already exists
        {
                if (_save is { _target: not null, _source: not null })
                    _log.ModifyStateFile(_save._name, _save._source, _save._target, list,
                        "Waiting"); // Modify the save in the history
        }
        else // If the save doesn't exist
        {
            var entryObject = new // Create the save in the history
            {
                Date = _log.GetDate(), // Create the save in the history
                Name = _save._name, // Create the save in the history
                SourcePath = _save._source, // Create the save in the history
                TargetPath = _save._target, // Create the save in the history
                Size = _save._weight, // Create the save in the history
                TotalFiles = _save._nbFiles, // Create the save in the history
                FilesCopied = 0, // Create the save in the history
                FilesRemaining = _save._nbFiles, // Create the save in the history
                isComplete = false, // Create the save in the history
                Status = "Waiting" // Create the save in the history
            };
            string toAdd = Newtonsoft.Json.JsonConvert.SerializeObject(entryObject); // Create the save in the history
            _log.AddEntryToStateFile(toAdd); // Add the save to the history
        }
    }
}