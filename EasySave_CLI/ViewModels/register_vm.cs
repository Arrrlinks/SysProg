using EasySave_CLI.Views;
using Program.Models;

namespace EasySave_CLI.ViewModels;

public class register_vm
{
    //Attributes
    private register_v _view;
    private save_m _save;
    
    //Builders
    public register_vm()
    {
        _view = new register_v();
        _save = new save_m();
    }
    public register_vm(register_v view, save_m save)
    {
        _view = view;
        _save = save;
    }
    
    //Methods
    public string GetBackup()
    {
        int backupNumber = _view.GetBackup(0);
        while (backupNumber < 1 || backupNumber > 5)
        {
            backupNumber = _view.GetBackup(1);
        }
        return "Save" + backupNumber;
    }
    
    public string? GetPath()
    {
        string? Path = _view.GetPath(0);
        while (Directory.Exists(@Path) == false && File.Exists(@Path) == false)
        {
            Path = _view.GetPath(1);
        }
        return Path;
    }
    
    

    public void RUN()
    {
        _save._name = GetBackup();
        
        _save._source = GetPath();
        
        _save._target = GetPath();
        
    }
}