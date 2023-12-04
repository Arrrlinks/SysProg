using EasySave_CLI.Views;

namespace EasySave_CLI.ViewModels;

public class mode_vm
{
    //Attributes
    private mode_v _view;
    private register_vm _register;
    private launch_vm _launch;
    
    //Builders
    public mode_vm()
    {
        _view = new mode_v();
        _register = new register_vm();
        _launch = new launch_vm();
    }
    
    public mode_vm(mode_v view, register_vm register, launch_vm launch)
    {
        _view = view;
        _register = register;
        _launch = launch;
    }
    
    //Methods
    //Get the mode the user want 
    public string SetMode()
    {
        string? mode = _view.SetMode(0).ToUpper();
        while (mode != "LAUNCH" && mode != "BACKUP" && mode != "EXIT")
        {
            mode = _view.SetMode(1).ToUpper();
        }
        return mode;
    }

    public void Run()
    {
        while (true)
        {
            //Ask the mode the user wants
            string mode = SetMode();
            
            //Execute the mode
            switch (mode)
            {
                case "LAUNCH":
                    _launch.setBackup();
                    break;
                case "BACKUP":
                    _register.RUN();
                    break;
                case "EXIT":
                    Environment.Exit(1);
                    break;
            }   
        }
    }
}