using EasySave_CLI.Views;

namespace EasySave_CLI.ViewModels;

public class mode_vm
{
    //Attributes
    private mode_v _view;
    private register_vm _register;
    
    //Builders
    public mode_vm()
    {
        _view = new mode_v();
        _register = new register_vm();
    }
    
    public mode_vm(mode_v view, register_vm register)
    {
        _view = view;
        _register = register;
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
                    _register.RUN();
                    break;
                case "BACKUP":
                    Console.WriteLine("HISTORY");
                    break;
                case "EXIT":
                    Environment.Exit(1);
                    break;
            }   
        }
    }
}