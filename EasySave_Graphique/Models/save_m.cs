using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySave_Graphique.Models;

public class save_m // Model for the saves
{
    public delegate void SaveCompletedEventHandler(object sender, EventArgs e);
    public event SaveCompletedEventHandler SaveCompleted;
    //Attributes
    private Dictionary<string, ManualResetEvent> _pauseEvents = new Dictionary<string, ManualResetEvent>();
    public string _name { get; set; } // Name of the save
    public string? _source { get; set; } // Source path of the save
    public string? _target { get; set; } // Target path of the save
    public double _weight { get; set; } // Weight of the save
    public double _nbFiles { get; set; } // Number of files of the save
    private log_m _log; // Model for the history
    private state_m _state; // Model for the state
    private format_m _format; // Model for the format
    private Settings_m _settings; // Model for the settings
    private bool _stopRequested = false;
    public event Action SaveUpdated;
    private Process _saveProcess; // Process for the save
    private string _key;
    
    public void PauseSelectedSave(backup_m backup)
    {
        _state.ModifyJsonFile("../../../state.json", backup.Name, "IsPaused", true);
        if (_pauseEvents.TryGetValue(backup.Name, out var pauseEvent))
        {
            pauseEvent.Reset();
        }
    }
    
    //Builders
    public save_m() // Builder for the save
    {
        _saveProcess = new Process(); // Create a new process
        _log = new log_m(); // Create a new history model
        _state = new state_m(); // Create a new state model
        _format = new format_m(); // Create a new format model
        _settings = new Settings_m(); // Create a new settings model
        
        _saveProcess.StartInfo.FileName = @".\Cryptosoft.exe"; // Set the name of the process
        _saveProcess.StartInfo.UseShellExecute = false; // Set the use of the shell to false
        _key = "azerty"; // Set the key of the save

    }
    public save_m(string name, string? source, string? target) // Builder for the save
    {
        _name = name; // Set the name of the save
        _source = source; // Set the source path of the save
        _target = target; // Set the target path of the save
        _weight = 0; // Set the weight of the save
    }
    //Methods
    
    public void ResumeSelectedSave(backup_m backup)
    {
        _state.ModifyJsonFile("../../../state.json", backup.Name, "IsPaused", false);
        if (_pauseEvents.TryGetValue(backup.Name, out var pauseEvent))
        {
            pauseEvent.Set();
        }
    }
    
    public double[] GetFileSize(string? source) // Function to get the size of a file
    {
        var size = CalculateSize(source);
        var nbFiles = CalculateFilesNB(source);
        double[] result = {nbFiles,size};
        return result;
    }
    
    public List<string> ConvertStringToList(string input)
    {
        List<string> result = JsonConvert.DeserializeObject<List<string>>(input);
        return result;
    }
    
    void CopyFileIfFile(string sourceFilePath, string destinationFolderPath, bool isComplete = false) // Function to copy a file
    {
        try // Try to copy the file
        {
            var extention = ConvertStringToList(_format.RetrieveValueFromConfigFile("Extensions", "Extensions"));
            bool isCrypted = false;
            foreach (var item in extention)
            {
                if(Path.GetExtension(sourceFilePath) == $".{item}")
                    isCrypted = true;
            }
            if (isComplete) // If the save is complete
            {
                if (File.Exists(sourceFilePath)) // If the file exists
                {
                    string fileName = Path.GetFileName(sourceFilePath); // Get the name of the file
                    if (Directory.Exists(destinationFolderPath)) // If the destination folder path is valid
                    {
                        string destinationFilePath = Path.Combine(destinationFolderPath, fileName); // Get the destination file path
                        File.Copy(sourceFilePath, destinationFilePath, true); // Copy the file
                        if (isCrypted)
                        {
                            
                            string Argument = $"{destinationFilePath.Replace(" ","?")} {_key}";
                            _saveProcess.StartInfo.Arguments = Argument; 
                            _saveProcess.Start(); // Start the process
                        }
                    }
                    else // If the destination folder path is not valid
                    {
                        throw new ArgumentException("[CopyFileIfFile] The destination folder path is not valid."); // Throw an exception
                    }
                }
            }
            else // If the save is not complete
            {
                if (File.Exists(sourceFilePath)) // If the file exists
                {
                    string fileName = Path.GetFileName(sourceFilePath); // Get the name of the file
                    string destinationFilePath = Path.Combine(destinationFolderPath, fileName); // Get the destination file path

                    if (File.Exists(destinationFilePath)) // If the destination file path is valid
                    {
                        if (!FileCompare(sourceFilePath, destinationFilePath)) // If the file is different
                        {
                            File.Copy(sourceFilePath, destinationFilePath, true); // Copy the file
                            if (isCrypted)
                            {
                                string Argument = $"{destinationFilePath.Replace(" ","?")} {_key}";
                                _saveProcess.StartInfo.Arguments = Argument; 
                                _saveProcess.Start(); // Start the process
                            }
                        }
                    }
                    else // If the destination file path is not valid
                    {
                        File.Copy(sourceFilePath, destinationFilePath); // Copy the file
                        if (isCrypted)
                        {
                            string Argument = $"{destinationFilePath.Replace(" ","?")} {_key}";
                            _saveProcess.StartInfo.Arguments = Argument; 
                            _saveProcess.Start(); // Start the process
                        }
                    }
                }
            }
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"An error occured: {ex.Message}"); // Display an error message
        }
    }
    
    private bool FileCompare(string file1, string file2) // Function to compare two files
    {
        int file1Byte; // Create an int for the first file
        int file2Byte; // Create an int for the second file
        if (file1 == file2) // If the files are the same
        {
            return true; // Return true
        }
        var fs1 = new FileStream(file1, FileMode.Open); // Open the first file
        var fs2 = new FileStream(file2, FileMode.Open); // Open the second file
        if (fs1.Length != fs2.Length) // If the files are not the same
        {
            fs1.Close(); // Close the first file
            fs2.Close(); // Close the second file
            return false; // Return false
        }
        do // Do while the files are the same
        {
            file1Byte = fs1.ReadByte(); // Get the first file
            file2Byte = fs2.ReadByte(); // Get the second file
        }
        while ((file1Byte == file2Byte) && (file1Byte != -1)); // While the files are the same
        fs1.Close(); // Close the first file
        fs2.Close(); // Close the second file
        return ((file1Byte - file2Byte) == 0); // Return the difference between the two files
    }
    
    static long CalculateDateDifference(string isoDate1, string isoDate2) // Function to calculate the difference between two dates
    {
        try // Try to calculate the difference between two dates
        {
            DateTime date1 = DateTime.Parse(isoDate1, null, System.Globalization.DateTimeStyles.RoundtripKind); // Get the first date
            DateTime date2 = DateTime.Parse(isoDate2, null, System.Globalization.DateTimeStyles.RoundtripKind); // Get the second date

            long differenceInMilliseconds = (long)(date2 - date1).TotalMilliseconds; // Get the difference between the two dates

            return differenceInMilliseconds; // Return the difference between the two dates
        }
        catch (Exception ex) // If an error occured
        {
            Console.WriteLine($"Error calculating date difference: {ex.Message}"); // Display an error message
            return -1; // Return -1
        }
    }
    
    public void CopyFile(string? file, string? target = null, string? source = null, string? name = null, int iteration = 0, bool isComplete = false) // Function to copy a file
    {
        string debut = DateTime.UtcNow.ToString("o"); // Get the start of the save
        
        string? fileName = Path.GetFileName(file); // Get the name of the file

        double[] fileSizeList = GetFileSize(source); // Get the size of the file
        
        if (name != null) // If the name of the save is valid
        {
            if (_state.RetrieveValueFromStateFile(name, "Name") != null && target != null && source != null) // If the save exists in the history
            {
                    if (target != null && source != null)
                        _state.ModifyStateFile(name, source, target, fileSizeList, "Active", iteration); // Modify the save in the history
            }
            else // If the save doesn't exist in the history
            {
                string toAdd = "{\"Date\": \"" + _state.GetDate() + "\"," + // Create the save in the history
                               "\"Name\": \"" + name + "\"," + // Create the save in the history
                               "\"Source\": \"" + source + "\"," + // Create the save in the history
                               "\"Target\": \"" + target + "\"," + // Create the save in the history
                               "\"Size\": " + fileSizeList[0] + ", " + // Create the save in the history
                               "\"FilesNB\": " + fileSizeList[1] + ", " + // Create the save in the history
                               "\"FilesRemaining\": " + (iteration+1) + ", " + // Create the save in the history
                               "\"isComplete\": \"" + isComplete + "\", " + // Create the save in the history
                               "\"State\": \"Active\"}"; // Create the save in the history
                _state.AddEntryToStateFile(toAdd); // Add the save to the history
            }

            if (fileSizeList[0] - (iteration+1) <= 0) // If the save is finished
            {
                _state.ModifyJsonFile("../../../state.json", name, "State", "Completed"); // Modify the status of the save in the history
            }

            if (IsBusinessSoftwareRunning())
            {
                _state.ModifyJsonFile("../../../state.json", name, "State", "Paused");
            }
            UpdateSaveMenu();
        }
        
        if (@target != null && fileName != null && file != null) // If the target path, the name of the file and if the file is valid
        {
            string configJson = File.ReadAllText("../../../config.json");
            JArray config = JArray.Parse(configJson); // Get the config file
            string saveMode = config.Children<JObject>()
                .FirstOrDefault(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == "SaveMode")?["SaveMode"].ToString(); // Get the save mode
            isComplete = saveMode == "complete"; // Set the status of the save
            CopyFileIfFile(file, @target, isComplete); // Copy the file
        }

        string end = DateTime.UtcNow.ToString("o"); // Get the end of the save
        
        long differenceMs = CalculateDateDifference(debut, end); // Get the difference between the start and the end of the save
        
        string toAdd2Log = "{\"Date\": \"" + _state.GetDate() + "\"," + // Create the log
                           "\"Name\": \"" + name + "\"," + // Create the log
                           " \"SourcePath\": \"" + @source.Replace("\\", "\\\\") + $"\\\\{fileName}\"," + // Create the log
                           " \"TargetPath\": \"" + @target.Replace("\\", "\\\\") + "\"," + // Create the log
                           " \"FileName\": \"" + fileName + "\"," + // Create the log
                           " \"FileSize\": " + fileSizeList[1].ToString(CultureInfo.InvariantCulture) + ", " + // Create the log
                           " \"TimeMs\": " + differenceMs + "}"; // Create the log
        
        _log.AddEntryToLogFile(toAdd2Log); // Add the log to the history
    }
    
    private int CalculateFilesNB(string source)
    {
        try
        {
            if (Directory.Exists(source))
            {
                var files = Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
                return files.Length;
            }
            else
            {
                return -1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return -1;
        }
    }

    private int CalculateSize(string source)
    {
        try
        {
            if (Directory.Exists(source))
            {
                var files = Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
                long totalSize = files.Sum(file => new System.IO.FileInfo(file).Length);
                double sizeInMB = totalSize / (1024.0 * 1024.0);
                return (int)sizeInMB;
            }
            else
            {
                return -1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return -1;
        }
    }
    
    private async void UpdateSaveMenu()
    {
        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            SaveUpdated?.Invoke();
        }));
    }
    
    public bool IsBusinessSoftwareRunning()
    {
        foreach (var process in Process.GetProcesses())
        {
            if (process.ProcessName == "CalculatorApp")
            {
                return true;
            }
        }
        return false;
    }
    
    public void SaveLaunch(string? source = null, string? target = null, string? name = null, backup_m? backup = null, int i = 0, bool isComplete = false) // Function to launch a save
    {
        if (!IsBusinessSoftwareRunning())
        {
            string configJson = File.ReadAllText("../../../config.json");
            JArray configArray = JArray.Parse(configJson);
            double sizeLimit = 0;

            foreach (var config in configArray)
            {
                if (config["Name"]?.ToString() == "SizeLimit")
                {
                    sizeLimit = config["SizeLimit"].Value<double>();
                    break;
                }
            }

            var backups = _state.GetBackupsFromStateFile();
            
            double totalSize = 0;
            int count = 0;
            foreach (var item in backups)
            {
                if (item.State == "Active")
                {
                    totalSize += GetFileSize(item.Source)[1];
                    count++;
                }
            }

            Console.WriteLine(sizeLimit);
            Console.WriteLine(totalSize);
            
            if (count > 0)
            {
                totalSize += GetFileSize(source)[1];
            }

            if (totalSize > sizeLimit)
            {
                MessageBox.Show("The saves are too big to be launched. Please reduce the size of the saves or increase the SizeLimit in the config.json file.");
                return;
            }
            
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

                // Create a new ManualResetEvent for this task and add it to the dictionary
                _pauseEvents[name] = new ManualResetEvent(true);

                foreach (string? file in files) // For each file in the save
                {
                    if (_state.RetrieveValueFromStateFile(name, "IsPaused"))
                    {
                        _state.ModifyJsonFile("../../../state.json", name, "IsPaused", false);
                    }

                    if (IsBusinessSoftwareRunning())
                    {
                        PauseSelectedSave(backup);
                        _state.ModifyJsonFile("../../../state.json", name, "State", "Paused");
                        _state.ModifyJsonFile("../../../state.json", name, "IsPaused", true);
                        UpdateSaveMenu();
                        MessageBox.Show(
                            $"{language.Resources.TheBusinessSoftwareIsRunningAllRunningSavesHaveBeenPaused}",
                            "EasySave", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    CopyFile(file, target, source, name, i, isComplete); // Copy the file
                    if (name != null)
                        _state.ModifyJsonFile("../../../state.json", name, "isComplete",
                            isComplete); // Modify the status of the save in the history
                    i++; // Increment the number of files copied

                    // Wait using the correct ManualResetEvent
                    _pauseEvents[name].WaitOne();
                }

                string?[] directories = Directory.GetDirectories(@source); // Get the directories of the save
                foreach (string? directory in directories) // For each directory in the save
                {
                    if (target != null) // If the target path is valid
                    {
                        DirectoryInfo newDirectory =
                            Directory.CreateDirectory(Path.Combine(target,
                                Path.GetFileName(directory) ?? string.Empty)); // Create the directory
                        string? newDirectoryPath = newDirectory.FullName; // Get the path of the directory
                        SaveLaunch(directory, newDirectoryPath, name, backup, i, isComplete); // Save the directory
                    }
                }
            }
        }
        else
        {
            MessageBox.Show(
                $"{language.Resources.YouCantLaunchASaveWhileTheBusinessSoftwareIsRunning}",
                "EasySave", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
