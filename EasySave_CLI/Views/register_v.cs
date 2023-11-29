namespace EasySave_CLI.Views;

public class register_v
{
    //Attributes
    
    //Builder
    public register_v() {}
    
    //Methods
    public int SetBackup(int error)
    {
        Console.Clear();
        if (error == 1)
        {
            Console.WriteLine("The backup you entered is not valid, please try again.");
        }
        Console.WriteLine("Select the backup you want to Modify or Create (1-5)");
        int backup = 0;
        try
        {
            backup = Convert.ToInt32(Console.ReadLine());
        }
        catch (FormatException)
        {
            backup = 0;
        }
        return backup;
    }

    public string? SetPath(int error)
    {
        Console.Clear();
        if (error == 1)
        {
            Console.WriteLine("The path you entered is not valid, please try again.");
        }
        Console.WriteLine("Enter the path of the file you want to save");
        string? filePath = Console.ReadLine();
        return filePath;
    }
    
}