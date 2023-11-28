// See https://aka.ms/new-console-template for more information

using EasySave_CLI.ViewModels;
using EasySave_CLI.Views;

namespace Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Create the view and the view model
            mode_v view = new mode_v();
            mode_vm viewModel = new mode_vm(view);
            
            //Run the program
            viewModel.Run();
        }
    }
}