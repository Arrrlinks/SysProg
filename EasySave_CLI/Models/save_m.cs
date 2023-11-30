using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace Program.Models;

public class save_m
{
    //Attributes
    public string _name { get; set; }
    public string _source { get; set; }
    public string _target { get; set; }
    
    //Builders
    public save_m(){}
    
    public save_m(string name, string source, string target)
    {
        _name = name;
        _source = source;
        _target = target;
    }
    
    //Methods

    public void CopyFile(string file)
    {
        string fileName = Path.GetFileName(file);
        string destFile = Path.Combine(@_target, fileName);
        File.Copy(file, destFile, true);
        //modif log journalier
    }
    
    public List<string> GetFiles(string source)
    {
        //Récupérer les fichier et les mettres dans une collections
        string[] directories = Directory.GetDirectories(@source);
        string[] doc = Directory.GetFiles(@source);
        //ajoutes les fichiers dans files
        List<string> files = new List<string>();
        files.AddRange(doc);
        files.AddRange(directories);

        return files;
    }

    public void SaveLaunch()
    { 
        List<string> files = GetFiles(@_source);
        foreach (string file in files)
        {
            Console.WriteLine(file);
        }
        /*
        try
        {
            string[] files = Directory.GetFiles(@_source);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                CopyFile(file);
            }
        }
        catch (Exception)
        {
            string file = @_source;
            CopyFile(file);
        }
        Console.ReadKey(); */
    }
}