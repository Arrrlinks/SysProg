using EasySave_CLI.Views; // Namespace for the views

namespace EasySave_CLI.ViewModels; // Namespace for the view models

public class mode_vm // View model for the mode
{
    //Attributes
    private mode_v _view; // View for the mode
    private register_vm _register; // View model for the register
    private launch_vm _launch; // View model for the launch
    private history_vm _history; // View model for the history
    
    //Builders
    public mode_vm() // Builder for the mode
    {
        _view = new mode_v(); // Create a new view for the mode
        _register = new register_vm(); // Create a new view model for the register
        _launch = new launch_vm(); // Create a new view model for the launch
        _history = new history_vm(); // Create a new view model for the history
    }
    
    public mode_vm(mode_v view, register_vm register, launch_vm launch, history_vm history) // Builder for the mode
    {
        _view = view; // Set the view for the mode
        _register = register; // Set the view model for the register
        _launch = launch; // Set the view model for the launch
        _history = history; // Set the view model for the history
    }
    
    //Methods
    public string SetMode() // Function to set the mode
    {
        string? mode = _view.SetMode(0).ToUpper(); // Get the mode
        while (mode != "LAUNCH" && mode != "BACKUP" && mode != "EXIT" && mode != "HISTORY") // While the mode is not "launch", "backup", "exit" or "history"
        {
            mode = _view.SetMode(1).ToUpper(); // Get the mode
        }
        return mode; // Return the mode
    }

    public void Run() // Function to run the mode
    {
        while (true) // While the user doesn't want to exit
        {
            string mode = SetMode(); // Get the mode
            switch (mode) // Switch the mode
            {
                case "LAUNCH": // If the mode is "launch"
                    _launch.Run(); // Run the launch
                    break; // Break the switch
                case "BACKUP": // If the mode is "backup"
                    _register.Run(); // Run the register
                    break; // Break the switch
                case "EXIT": // If the mode is "exit"
                    Environment.Exit(1); // Exit the program
                    break; // Break the switch
                case "HISTORY": // If the mode is "history"
                    _history.Run(); // Run the history
                    break; // Break the switch
            }   
        }
    }
}