namespace EasySave_CLI.Views; // Namespace for the views

public class mode_v // View for the mode
{
    public mode_v() {} // Builder for the mode
    
    public string? SetMode(int error) // Function to set the mode
    {
        if (error == 1) // If the user entered a wrong command
        {
            Console.WriteLine("The command you entered is not valid, please try again."); // Display an error message
        }
        Console.WriteLine("Select the mode \n - launch \n - backup \n - history \n - exit"); // Display the modes
        string? mode = Console.ReadLine(); // Get the mode
        return mode; // Return the mode
    }
}