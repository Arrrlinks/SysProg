using EasySave_CLI.Views; // Namespace for the views

namespace EasySave_CLI.ViewModels; // Namespace for the view models

public class mode_vm // View model for the mode
{
    //Attributes
    private mode_v _view; // View for the mode
    private register_vm _register; // View model for the register
    private launch_vm _launch; // View model for the launch
    private history_vm _history; // View model for the history
    private state_vm _state = new state_vm(); // View model for the state
    private language_vm _lang = new language_vm(); // View model for the state
    private format_vm _format = new format_vm(); // View model for the state
    
    //Builders
    public mode_vm() // Builder for the mode
    {
        _view = new mode_v(); // Create a new view for the mode
        _register = new register_vm(); // Create a new view model for the register
        _launch = new launch_vm(); // Create a new view model for the launch
        _history = new history_vm(); // Create a new view model for the history
        _state = new state_vm(); // Create a new view model for the state
        _lang = new language_vm(); // Create a new view model for the state
        _format = new format_vm(); // Create a new view model for the state 
    }
    
    public mode_vm(mode_v view, register_vm register, launch_vm launch, history_vm history, state_vm state, language_vm lang, format_vm format) // Builder for the mode
    {
        _view = view; // Set the view for the mode
        _register = register; // Set the view model for the register
        _launch = launch; // Set the view model for the launch
        _history = history; // Set the view model for the history
        _state = state; // Set the view model for the state
        _lang = lang; // Set the view model for the state
        _format = format; // Create a new view model for the state
    }
    
    //Methods
    public string SetMode() // Function to set the mode
    {
        string? mode = _view.SetMode(0).ToUpper(); // Get the mode
        while (mode != "LAUNCH" && mode != "BACKUP" && mode != "EXIT" && mode != "HISTORY" && mode != "STATE" && mode != "LANGUAGE" && mode != "FORMAT" && mode != "1" && mode != "2" && mode != "3" && mode != "4" && mode != "5" && mode != "6" && mode != "7" ) // While the mode is not valid
        {
            mode = _view.SetMode(1).ToUpper(); // Get the mode
        }
        switch (mode)
        {
            case "1": // If the mode is "launch"
                mode = "LAUNCH"; // Set the mode to "launch"
                break; // Break the switch
            case "2": // If the mode is "backup"
                mode = "BACKUP"; // Set the mode to "backup"
                break; // Break the switch
            case "3": // If the mode is "state"
                mode = "STATE"; // Set the mode to "state"
                break; // Break the switch
            case "4": // If the mode is "history"
                mode = "HISTORY"; // Set the mode to "history"
                break; // Break the switch
            case "5": // If the mode is "language"
                mode = "LANGUAGE"; // Set the mode to "language"
                break; // Break the switch
            case "6": // If the mode is "format"
                mode = "FORMAT"; // Set the mode to "format"
                break; // Break the switch
            case "7":
                mode = "EXIT"; // Set the mode to "exit"
                break; // Break the switch
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
                case "STATE": // If the mode is "state"
                    _state.Run(); // Run the state
                    break; // Break the switch
                case "HISTORY": // If the mode is "history"
                    _history.Run(); // Run the history
                    break; // Break the switch
                case "LANGUAGE": // If the mode is "language"
                    _lang.Run();
                    break;
                case "FORMAT": // If the mode is "format"
                    _format.Run();
                    break;
                case "EXIT": // If the mode is "exit"
                    Environment.Exit(1); // Exit the program
                    break; // Break the switch
            }   
        }
    }
}