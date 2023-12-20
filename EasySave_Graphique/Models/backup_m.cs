using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

public class backup_m : INotifyPropertyChanged
{
    private bool _selected;
    private string _name;
    private string _source;
    private string _target;
    private string _date;
    private string _size;
    private string _filesNB;
    private string? _filesRemaining;
    private string _state;
    private string _isPaused;

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged("Name");
        }
    }

    public string Source
    {
        get { return _source; }
        set
        {
            _source = value;
            OnPropertyChanged("Source");
            FilesNB = CalculateFilesNB(_source);
            Size = CalculateSize(_source);
        }
    }

    public string Target
    {
        get { return _target; }
        set
        {
            _target = value;
            OnPropertyChanged("Target");
        }
    }

    public string Date
    {
        get { return _date; }
        set
        {
            _date = value;
            OnPropertyChanged("Date");
        }
    }

    public string Size
    {
        get { return _size; }
        set
        {
            _size = value;
            OnPropertyChanged("Size");
        }
    }

    public string FilesNB
    {
        get { return _filesNB; }
        set
        {
            _filesNB = value;
            OnPropertyChanged("FilesNB");
        }
    }

    public string State
    {
        get { return _state; }
        set
        {
            _state = value;
            OnPropertyChanged("State");
        }
    }

    public bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            OnPropertyChanged("Selected");
        }
    }

    public bool IsPaused { get; set; } = false;

    public string? FilesRemaining
    {
        get { return _filesRemaining; }
        set
        {
            _filesRemaining = value;
            OnPropertyChanged("FilesRemaining");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string CalculateFilesNB(string source)
    {
        try
        {
            if (System.IO.Directory.Exists(source))
            {
                var files = System.IO.Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
                return files.Length.ToString();
            }
            else
            {
                return "Directory does not exist";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "Error";
        }
    }

    private string CalculateSize(string source)
    {
        try
        {
            if (System.IO.Directory.Exists(source))
            {
                var files = System.IO.Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories);
                long totalSize = files.Sum(file => new System.IO.FileInfo(file).Length);
                double sizeInMB = totalSize / (1024.0 * 1024.0);
                return sizeInMB.ToString("F2");
            }
            else
            {
                return "Directory does not exist";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "Error";
        }
    }
}