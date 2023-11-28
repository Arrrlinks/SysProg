using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.ViewName
{
    public class View
    {
        public View() { }
        //Get the mode the user want 
        public string? GetMode()
        {
            Console.WriteLine("Select the mode \n - Copy \n - History \n - Exit");
            string? Mode = Console.ReadLine();
            return Mode;
        }

        //Get the path of the file
        public string? GetFilePath()
        {
            Console.WriteLine("Select the mode \n - Copy \n -History");
            string? FilePath = Console.ReadLine();
            return FilePath;
        }

        //Get the file of the save foldier
        public string? GetSavePath()
        {
            Console.WriteLine("Select the mode \n - Copy \n -History");
            string? SavePath = Console.ReadLine();
            return SavePath;
        }
    }
}
