namespace EasySave_CLI.Views; // Namespace for the views

public class mode_v // View for the mode
{
    public mode_v() {} // Builder for the mode
    
    public string? SetMode(int error) // Function to set the mode
    {
        if (error == 1) // If the user entered a wrong command
        {
            Console.WriteLine("The command you entered is not valid, please try again."); // Display an error message
            Console.WriteLine("Press any key to continue..."); // Display a message
            Console.ReadKey(); // Wait for the user to press a key
        }
        Console.Clear(); // Clear the console
        Console.WriteLine("Select the mode \n 1 - launch \n 2 - backup \n 3 - state \n 4 - history \n 5 - exit"); // Display the modes
        string? mode = Console.ReadLine(); // Get the mode
        return mode; // Return the mode
    }
}