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
    public string SetBackup()
    {
        int backupNumber = _view.SetBackup(0);
        while (backupNumber < 1 || backupNumber > 5)
        {
            backupNumber = _view.SetBackup(1);
        }
        return "Save" + backupNumber;
    }
    
    public string? SetPath(int mode)
    {
        string? Path;
        if (mode == 0)
        {
            Path = _view.SetPath(0);
        }
        else
        {
            Path = _view.SetPath(1);
        }
        return Path;
    }
    
    public void SetSaveName(string name)
    {
        _save._name = name;
    }
    
    public void SetSaveSource(string source)
    {
        _save._source = source;
    }
    
    public void SetSaveTarget(string target)
    {
        _save._target = target;
    }
    
    public void RUN()
    {
        SetSaveName(SetBackup());
        
        SetSaveSource(SetPath(0));
        
        SetSaveTarget(SetPath(1));
        
    }
}