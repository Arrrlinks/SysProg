using EasySave_CLI.Views;

namespace EasySave_CLI.ViewModels;

public class mode_vm
{
    //Attributes
    private mode_v _view;
    
    //Builders
    public mode_vm()
    {
        _view = new mode_v();
    }
    
    public mode_vm(mode_v view)
    {
        _view = view;
    }
    
    //Methods
    //Get the mode the user want 
    public string GetMode()
    {
        string? mode = _view.GetMode().ToUpper();
        while (mode != "COPY" && mode != "HISTORY" && mode != "EXIT")
        {
            Console.Clear();
            Console.WriteLine("You typed a wrong imput. Try again");
            mode = _view.GetMode().ToUpper();
        }
        return mode;
    }

    public void Run()
    {
        //Ask the mode the user wants
        string mode = GetMode();

        switch (mode)
        {
            case "COPY":
                Console.WriteLine("copy");
                break;
            case "HISTORY":
                Console.WriteLine("HISTORY");
                break;
            case "EXIT":
                Environment.Exit(1);
                break;
        }
    }
}