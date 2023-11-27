// Purpose: Entry point for the program

using EasySave.ViewName;
using EasySave.ModelName;
using EasySave.ViewModelName;

namespace Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Variable Initialisation
            View view = new View();
            ViewModel viewModel = new ViewModel(view);
            //Ask the mode the user wants
            string mode = viewModel.GetMode();

            switch (mode)
            {
                case "COPY":
                    Console.WriteLine("copy");
                    break;

                case "HISTORY":
                    Console.WriteLine("HISTORY");
                    break;

                case "EXIT":
                    Environment.Exit(1);
                    break;
            }
        }
    }
}