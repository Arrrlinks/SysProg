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
            Model model = new Model();
            ViewModel viewModel = new ViewModel(view, model);
            
            viewModel.Run();
        }
    }
}