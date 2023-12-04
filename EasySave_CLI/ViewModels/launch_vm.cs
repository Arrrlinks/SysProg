using EasySave_CLI.Views;
using Program.Models;

namespace EasySave_CLI.ViewModels;

public class launch_vm
{
    //Attributes
    private launch_v _view;
    private save_m _save;

    //Builders
    public launch_vm()
    {
        _view = new launch_v();
        _save = new save_m();
    }
    public launch_vm(launch_v view, save_m save)
    {
        _view = view;
        _save = save;
    }

    public void SaveBackup(string source, string target)
    {
        _save.CopyFile(source, target);
    }
    
    //Methods
    public void setBackup()
    {
        List<string> data=_view.SetBackup();
        foreach (string backup in data)
        {
            //récupère le chemin dans le log state en fonction du nom du backup
            string source = "";
            string target = "";
            SaveBackup(source, target);
            
        }
    }
}