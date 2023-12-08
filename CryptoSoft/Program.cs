// See https://aka.ms/new-console-template for more information

using System.Text;

namespace CryptoSoft
{
    class Program
    {
        static ViewModel _viewModel;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                _viewModel = new ViewModel();
            }
            //if not the good number of arguments, quit
            else if (args.Length != 2)
            {
                throw new ArgumentNullException("Not enough arguments");
            }
            else
            {
                _viewModel = new ViewModel(args[0], args[1]);
            }
            _viewModel.Run();
        }
    }
}