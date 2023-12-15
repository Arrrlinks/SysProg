using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using EasySave_Graphique.Models;
using Newtonsoft.Json; // Model for the history

namespace Program.Models; // Namespace for the models

public class save_m // Model for the saves
{
    public delegate void SaveCompletedEventHandler(object sender, EventArgs e);
    public event SaveCompletedEventHandler SaveCompleted;
    //Attributes
    public string _name { get; set; } // Name of the save
    public string? _source { get; set; } // Source path of the save
    public string? _target { get; set; } // Target path of the save
    public double _weight { get; set; } // Weight of the save
    public double _nbFiles { get; set; } // Number of files of the save
    private log_m _log; // Model for the history
    private state_m _state; // Model for the state
    private format_m _format;
    
    public event Action SaveUpdated;
    
    private Process _saveProcess; // Process for the save

    private string _key;
    public bool isPaused;
    //Builders
    public save_m() // Builder for the save
    {
        _saveProcess = new Process(); // Create a new process
        _log = new log_m(); // Create a new history model
        _state = new state_m(); // Create a new state model
        _format = new format_m(); // Create a new format model
        
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
    
    void CopyFileIfFile(string sourceFilePath, string destinationFolderPath, bool isPaused, bool isComplete = false ) // Function to copy a file
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
                        foreach (var item in extention)
                        {
                          /*
                            while (isPaused == true)
                            {
                                if (isPaused == false)
                                {
                                    break;
                                }
                            }
                            */
                            File.Copy(sourceFilePath, destinationFilePath, true); // Copy the file
                            if (isCrypted)
                            {
                                string Argument = $"\"{destinationFilePath.Replace(" ","?")}\" \"{_key}\"";
                                _saveProcess.StartInfo.Arguments = Argument;
                                _saveProcess.Start(); // Start the process
                                _saveProcess.WaitForExit();
                            }
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
                foreach (var item in extention)
                {/*
                    while (isPaused == true)
                    {
                        if (isPaused == false)
                        { */
                            File.Copy(sourceFilePath, destinationFilePath, true); // Copy the file
                            if (isCrypted)
                            {
                                string Argument = $"{destinationFilePath.Replace(" ","?")} {_key}";
                                _saveProcess.StartInfo.Arguments = Argument; 
                                _saveProcess.Start(); // Start the process
                            }/*
                            break;
                        }
                    }
                    */
                    if (File.Exists(sourceFilePath)) // If the file exists
                    {
                        string fileName = Path.GetFileName(sourceFilePath); // Get the name of the file
                        string destinationFilePath = Path.Combine(destinationFolderPath, fileName); // Get the destination file path

                        if (File.Exists(destinationFilePath)) // If the destination file path is valid
                        {
                            if (!FileCompare(sourceFilePath, destinationFilePath)) // If the file is different
                            {
                                File.Copy(sourceFilePath, destinationFilePath, true); // Copy the file
                                {
                                    string Argument = $"{destinationFilePath} {_key}";
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
    
    public void CopyFile(string? file, string? target = null, string? source = null, string? name = null, int iteration = 0, bool isFile = false, bool isComplete = false) // Function to copy a file
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
            UpdateSaveMenu();
        }
        
        if (@target != null && fileName != null && file != null) // If the target path, the name of the file and if the file is valid
        {
            CopyFileIfFile(file, @target, isPaused ,isComplete); // Copy the file
        }

        string end = DateTime.UtcNow.ToString("o"); // Get the end of the save
        
        long differenceMs = CalculateDateDifference(debut, end); // Get the difference between the start and the end of the save
        
        string toAdd2Log = "{\"Date\": \"" + _state.GetDate() + "\"," + // Create the log
                           "\"Name\": \"" + name + "\"," + // Create the log
                           " \"SourcePath\": \"" + @source.Replace("\\", "\\\\") + "\"," + // Create the log
                           " \"TargetPath\": \"" + @target.Replace("\\", "\\\\") + "\"," + // Create the log
                           " \"FileName\": \"" + fileName + "\"," + // Create the log
                           " \"FileSize\": " + fileSizeList[0].ToString(CultureInfo.InvariantCulture) + ", " + // Create the log
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
    
    public void SaveLaunch(string? source = null, string? target = null, string? name = null, int i = 0, bool isComplete = false) // Function to save a save
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
                CopyFile(file, target, source, name, i,false,  isComplete); // Copy the file
                if (name != null)
                    _state.ModifyJsonFile("../../../state.json", name, "isComplete",
                        isComplete); // Modify the status of the save in the history
                i++; // Increment the number of files copied
            }
            
            string?[] directories = Directory.GetDirectories(@source); // Get the directories of the save
            foreach (string? directory in directories) // For each directory in the save
            {
                if (target != null) // If the target path is valid
                {
                    DirectoryInfo newDirectory = Directory.CreateDirectory(Path.Combine(target, Path.GetFileName(directory) ?? string.Empty)); // Create the directory
                    string? newDirectoryPath = newDirectory.FullName; // Get the path of the directory
                    SaveLaunch(directory, newDirectoryPath, name, i, isComplete); // Save the directory
                }
            }
        }

    }
}
