// See https://aka.ms/new-console-template for more information

using EasySave_CLI.ViewModels; // Namespace for the view models

namespace Program // Namespace for the program
{
    public class Program // Program
    {
        public static void Main(string[] args) // Main function
        {
            mode_vm viewModel = new mode_vm(); // Create a new view model for the mode
            viewModel.Run(); // Run the mode
        }
    }
}