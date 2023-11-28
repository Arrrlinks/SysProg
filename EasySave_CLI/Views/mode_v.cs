namespace EasySave_CLI.Views;

public class mode_v
{
    public mode_v() {}
    
    //Get the mode the user want 
    public string? GetMode()
    {
        Console.WriteLine("Select the mode \n - Copy \n - History \n - Exit");
        string? mode = Console.ReadLine();
        return mode;
    }
}