namespace EasySave_CLI.Views;

public class register_v
{
    //Attributes
    
    //Builder
    public register_v() {}
    
    //Methods
    //Chose the backup to modify
    public int SetBackup(int error = 0)
    {
        Console.Clear();
        int backup = 0;

        if (error == 1)
        {
            Console.WriteLine("The backup you entered is not valid, please try again.");
        }
        Console.WriteLine("Select the backup you want to Modify or Create (1-5)");
        //Véry the user input an int
        try
        {
            backup = Convert.ToInt32(Console.ReadLine());
        }
        catch (FormatException)
        {
            backup = SetBackup(1);
        }
        return backup;
    }

    //Set the source and target path of the backup
    public string? SetPath(int mode = 0)
    {
        Console.Clear();
        string? filePath;
        if (mode == 0)
        {
            Console.WriteLine("Enter the path of the file you want to save");
            filePath = Console.ReadLine();
            while (Directory.Exists(@filePath) == false && File.Exists(@filePath) == false)
            {
                Console.WriteLine("The path you entered is not valid, please try again.");
                filePath = Console.ReadLine();
            }
        }
        else
        {
            Console.WriteLine("Enter the path of where the file will be save");
            filePath = Console.ReadLine();
            while (Directory.Exists(@filePath) == false)
            {
                Console.WriteLine("The path you entered is not valid, please try again.");
                filePath = Console.ReadLine();
            }
        }
        return filePath;
    }
}