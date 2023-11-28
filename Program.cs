// Purpose: Entry point for the program

using EasySave.ViewsName;
using EasySave.ModelsName;
using EasySave.ViewModelsName;

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