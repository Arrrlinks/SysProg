using EasySave_CLI.Models; // Model for the history

namespace Program.Models; // Namespace for the models

public class save_m // Model for the saves
{
    //Attributes
    public string _name { get; set; } // Name of the save
    public string? _source { get; set; } // Source path of the save
    public string? _target { get; set; } // Target path of the save
    public double _weight { get; set; } // Weight of the save
    public double _nbFiles { get; set; } // Number of files of the save
    private log_m _log; // Model for the history
    
    //Builders
    public save_m() // Builder for the save
    {
        _log = new log_m(); // Create a new history model
    }
    public save_m(string name, string? source, string? target) // Builder for the save
    {
        _name = name; // Set the name of the save
        _source = source; // Set the source path of the save
        _target = target; // Set the target path of the save
        _weight = 0; // Set the weight of the save
    }
    //Methods
    
    public double[] GetFileSize(string? source = null) // Function to get the size of a file
    {
        source ??= _source; // Set the source path of the save
        double[] weightList = new double[2]; // Create a list of double
        double length = 0; // Create a double for the length of the file
        double weight = 0; // Create a double for the weight of the file
        try
        {
            string[] data = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories); // Get the files of the save
            length = data.Length; // Get the length of the save
            foreach (string file in data) // For each file in the save
            {
                weight += new FileInfo(file).Length; // Get the weight of the save
            } 
        }
        catch (Exception e) // If an error occured
        {
            length = 1; // Set the length of the save to 1
            weight = new FileInfo(source).Length; // Get the weight of the save
        }
        
        weightList[0] = weight; // Set the weight of the save
        weightList[1] = length; // Set the length of the save
        return weightList; // Return the list of double
    }

    static void CopyFileIfFile(string sourceFilePath, string destinationFolderPath) // Function to copy a file
    {
        if (File.Exists(sourceFilePath)) // If the file exists
        {
            string fileName = Path.GetFileName(sourceFilePath); // Get the name of the file
            if (Directory.Exists(destinationFolderPath)) // If the destination folder exists
            {
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName); // Get the destination path of the file
                File.Copy(sourceFilePath, destinationFilePath, true); // Copy the file
            }
            else // If the destination folder doesn't exist
            {
                throw new ArgumentException("[CopyFileIfFile] The destination folder path is not valid."); // Display an error message
            }
        }
    }
    
    static long CalculateDateDifference(string isoDate1, string isoDate2)
    {
        try
        {
            DateTime date1 = DateTime.Parse(isoDate1, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime date2 = DateTime.Parse(isoDate2, null, System.Globalization.DateTimeStyles.RoundtripKind);

            long differenceInMilliseconds = (long)(date2 - date1).TotalMilliseconds;

            return differenceInMilliseconds;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating date difference: {ex.Message}");
            return -1;
        }
    }
    
    public void CopyFile(string? file, string? target = null, string? source = null, string? name = null, int iteration = 0) // Function to copy a file
    {
        source ??= _source; // Set the source path of the save
        target ??= _target; // Set the target path of the save
        string debut = DateTime.UtcNow.ToString("o"); // Get the start of the save
        
        string? fileName = Path.GetFileName(file); // Get the name of the file
        
        double[] fileSizeList = GetFileSize(file); // Get the size of the file
        
        if (name != null) // If the name of the save is valid
        {
            if (_log.RetrieveValueFromStateFile(name, "Name") != null) // If the save exists in the history
            {
                _log.ModifyJsonFile("../../../state.json", name, "Date", _log.GetDate()); // Modify the date of the save in the history
                if (source != null) _log.ModifyJsonFile("../../../state.json", name, "SourcePath", @source); // Modify the source path of the save in the history
                if (target != null) _log.ModifyJsonFile("../../../state.json", name, "TargetPath", @target); // Modify the target path of the save in the history
                _log.ModifyJsonFile("../../../state.json", name, "Size", fileSizeList[0]); // Modify the size of the save in the history
                _log.ModifyJsonFile("../../../state.json", name, "TotalFiles", fileSizeList[1]); // Modify the number of files of the save in the history
                _log.ModifyJsonFile("../../../state.json", name, "FilesCopied", iteration); // Modify the number of files copied of the save in the history
                _log.ModifyJsonFile("../../../state.json", name, "FilesRemaining", fileSizeList[1] - iteration); // Modify the number of files remaining of the save in the history
                _log.ModifyJsonFile("../../../state.json", name, "Status", "Active"); // Modify the status of the save in the history
            }
            else // If the save doesn't exist in the history
            {
                string toAdd = "{\"Date\": \"" + _log.GetDate() + "\"," + // Create the save in the history
                               "\"Name\": \"" + name + "\"," + // Create the save in the history
                               " \"SourcePath\": \"" + source + "\"," + // Create the save in the history
                               " \"TargetPath\": \"" + target + "\"," + // Create the save in the history
                               " \"Size\": " + fileSizeList[0] + ", " + // Create the save in the history
                               " \"TotalFiles\": " + fileSizeList[1] + ", " + // Create the save in the history
                               " \"FilesCopied\": " + iteration + ", " + // Create the save in the history
                               " \"FilesRemaining\": " + (fileSizeList[1] - iteration) + ", " + // Create the save in the history
                               " \"Status\": \"Active\"}"; // Create the save in the history
                _log.AddEntryToStateFile(toAdd); // Add the save to the history
            }

            if (fileSizeList[1] - iteration <= 0) // If the save is finished
            {
                _log.ModifyJsonFile("../../../state.json", name, "Status", "Completed"); // Modify the status of the save in the history
            }
        }
        
        if (@target != null) // If the target path is valid
        {
            if (fileName != null) // If the name of the file is valid
            {
                if (file != null) CopyFileIfFile(file, @target); // Copy the file
            }
        }

        string end = DateTime.UtcNow.ToString("o"); // Get the end of the save
        
        long differenceMs = CalculateDateDifference(debut, end); // Get the difference between the start and the end of the save
        
        //logJ(_name,source,target,fileSize differenceMS)
        
        string toAdd2Log = "{\"Date\": \"" + _log.GetDate() + "\"," + // Create the log
                           "\"Name\": \"" + name + "\"," + // Create the log
                           " \"SourcePath\": \"" + @source.Replace("\\", "\\\\") + "\"," + // Create the log
                           " \"TargetPath\": \"" + @target.Replace("\\", "\\\\") + "\"," + // Create the log
                           " \"FileName\": \"" + fileName + "\"," + // Create the log
                           " \"FileSize\": " + fileSizeList[0] + ", " + // Create the log
                           " \"Time (ms)\": " + differenceMs + "}"; // Create the log
        
        _log.AddEntryToLogFile(toAdd2Log); // Add the log to the history
    }

    
    public void SaveLaunch(string? source = null, string? target = null, string? name = null, int i = 0) // Function to save a save
    {
        if (source == null) // If the source path is not valid
        {
            source = _source; // Set the source path of the save
        }
        if (target == null) // If the target path is not valid
        {
            target = _target; // Set the target path of the save
        }
        if (@source != null) // If the source path is valid
        {
            string?[] files = Directory.GetFiles(@source); // Get the files of the save
            foreach (string? file in files) // For each file in the save
            {
                CopyFile(file, target, source,name, i); // Copy the file
                i++; // Increment the number of files copied
            }
        }

        if (@source != null) // If the source path is valid
        {
            string?[] directories = Directory.GetDirectories(@source); // Get the directories of the save
            foreach (string? directory in directories) // For each directory in the save
            {
                if (target != null) // If the target path is valid
                {
                    DirectoryInfo newDirectory = Directory.CreateDirectory(Path.Combine(target, Path.GetFileName(directory) ?? string.Empty)); // Create the directory
                    string? newDirectoryPath = newDirectory.FullName; // Get the path of the directory
                    SaveLaunch(directory, newDirectoryPath, name, i); // Save the directory
                }
            }
        }
    }
}