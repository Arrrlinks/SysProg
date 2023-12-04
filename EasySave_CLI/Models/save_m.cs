namespace Program.Models;

public class save_m
{
    //Attributes
    public string _name { get; set; }
    public string _source { get; set; }
    public string _target { get; set; }
    public double _weight { get; set; }
    //Builders
    public save_m(){}
    public save_m(string name, string source, string target)
    {
        _name = name;
        _source = source;
        _target = target;
        _weight = 0;
    }
    //Methods
    
    public double GetFileSize(string source = null, double weight = 0)
    {
        //Get files weight
        FileInfo infoSize = new FileInfo(source);
        weight = infoSize.Length;
        
        //Get files in sub directories
        string[] files = Directory.GetFiles(@source);
        foreach (string file in files)
        {
            weight += GetFileSize(file, weight);
        }
        return weight;
    }

    public void CopyFile(string file, string target = null, string source = null)
    {
        if (source==null)
        {
            source = _source;
        }
        if (target==null)
        {
            target = _target;
        }
        //Get the debut of the save
        DateTime debut = DateTime.Now;
        
        //save the file
        
        string fileName = Path.GetFileName(file);
        string destFile = Path.Combine(@target, fileName);
        File.Copy(file, destFile, true);
        
        
        //Get the end of the save
        DateTime end = DateTime.Now;
        
        //Calcul the time in ms
        TimeSpan difference = end - debut;
        double differenceMS = difference.Microseconds;
        
        //Get the size of the file
        FileInfo infoSize = new FileInfo(file);
        long fileSize = infoSize.Length;
        //logJ(_name,source,target,fileSize differenceMS)
        //logS()Modifie le state
    }

    
    public void SaveLaunch(string source = null, string target = null)
    {
        //Set the source directory 
        if (source == null)
        {
            source = _source;
        }
        //Set the target directory
        if (target == null)
        {
            target = _target;
        }
        //Get and copy every files
        string[] files = Directory.GetFiles(@source);
        foreach (string file in files)
        {
            CopyFile(file, target, source);
        }
        //get and save every files from sub directories
        string[] directories = Directory.GetDirectories(@source);
        foreach (string directory in directories)
        {
            DirectoryInfo newDirectory = Directory.CreateDirectory(Path.Combine(target, Path.GetFileName(directory)));
            string newDirectoryPath = newDirectory.FullName;
            SaveLaunch(directory, newDirectoryPath);
        }
    }
}