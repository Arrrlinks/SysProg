namespace EasySave_CLI.Views;

public class mode_v
{
    public mode_v() {}
    
    //Get the mode the user want 
    public string? SetMode(int error)
    {
        Console.Clear();
        if (error == 1)
        {
            Console.WriteLine("The command you entered is not valid, please try again.");
        }
        Console.WriteLine("Select the mode \n - Launch \n - Backup \n - Exit");
        string? mode = Console.ReadLine();
        return mode;
    }
}