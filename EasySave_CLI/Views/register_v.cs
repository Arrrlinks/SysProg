namespace EasySave_CLI.Views; // Namespace for the views

public class register_v // View for the register
{
    //Attributes
    
    //Builder
    public register_v() {} // Builder for the register
    
    //Methods
    //Chose the backup to modify
    public int SetBackup(int error = 0) // Function to set the name of the save
    {
        Console.Clear(); // Clear the console
        int backup = 0; // Create a string for the name of the save

        if (error == 1) // If the user entered a wrong command
        {
            Console.WriteLine("The backup you entered is not valid, please try again."); // Display an error message
        }
        Console.WriteLine("Select the backup you want to Modify or Create (1-5)"); // Display the syntax
        try // Try to get the backup
        {
            backup = Convert.ToInt32(Console.ReadLine()); // Get the backup
        }
        catch (FormatException) // If the backup is not a number
        {
            backup = SetBackup(1); // Get the backup
        }
        return backup; // Return the backup
    }

    public string? SetPath(int mode = 0) // Function to set the path of the save
    {
        Console.Clear(); // Clear the console
        string? filePath; // Create a string for the path of the save
        if (mode == 0) // If the mode is "source"
        {
            Console.WriteLine("Enter the path of the file you want to save"); // Display the syntax
            filePath = Console.ReadLine(); // Get the path of the save
            while (Directory.Exists(@filePath) == false && File.Exists(@filePath) == false) // While the path is not valid
            {
                Console.WriteLine("The path you entered is not valid, please try again."); // Display an error message
                filePath = Console.ReadLine(); // Get the path of the save
            }
        }
        else // If the mode is "target"
        {
            Console.WriteLine("Enter the path of where the file will be save"); // Display the syntax
            filePath = Console.ReadLine(); // Get the path of the save
            while (Directory.Exists(@filePath) == false) // While the path is not valid
            {
                Console.WriteLine("The path you entered is not valid, please try again."); // Display an error message
                filePath = Console.ReadLine(); // Get the path of the save
            }
        }
        return filePath; // Return the path of the save
    }
}