using System.Text.RegularExpressions;

namespace EasySave_CLI.Views;

public class launch_v
{
    public launch_v() {}

    public List<string> SetBackup()
    {
        List<string> backups = new List<string>();
        Console.WriteLine("chose the backups to execute \n syntax: \n 1-5 `\n 1; 4");
        string command = Console.ReadLine();
        
        //Verify the type of command the user entered
        //Set regex to check if there is a hyphen, Match for only one answer
        Match hyphen = Regex.Match(command, @"^(\d)-(\d)$");
        //Set regex to check if there is a hyphen, MatchCollection for several answers
        MatchCollection semicolon = Regex.Matches(command, @"\d+");
        
        //-
        if (hyphen.Success)
        {
            int firstBackup = int.Parse(hyphen.Groups[1].Value);
            int lastBackup = int.Parse(hyphen.Groups[2].Value);
            if (firstBackup >= 1 && firstBackup <= 5 &&
                lastBackup >= 1 && lastBackup <= 5 &&
                firstBackup < lastBackup)
            {
                for (int i = firstBackup; i <= lastBackup; i++)
                {
                    backups.Add("Save"+i);
                }
            }
        }
        //;
        else if (semicolon.Count > 0)
        {
            foreach (Match match in semicolon)
            {
                int backup = int.Parse(match.Value);
                if (backup >= 1 && backup <= 5)
                {
                    backups.Add("Save"+backup);
                }
            }
        }
        //1
        else if (int.TryParse(command, out int number) && number >= 1 && number <= 5)
        {
            backups.Add("Save"+number);
        }
        else
        {
            Console.WriteLine("The command you entered is not valid, please try again.");
            backups.AddRange(SetBackup());
        }
        return backups;
    }
}