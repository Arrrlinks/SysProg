using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.ModelName;
using EasySave.ViewName;


namespace EasySave.ViewModelName
{
    public class ViewModel
    {
        //Builders
        private View _view;
        private Model _model;

        public ViewModel() 
        {
            _view = new View();
            _model = new Model();
        }
        public ViewModel(View view, Model model)
        {
            _view = view;
            _model = model;
        }

        //Get the mode the user want 
        public string GetMode()
        {
            string? mode = _view.GetMode().ToUpper();
            while (mode != "COPY" && mode != "HISTORY" && mode != "EXIT")
            {
                Console.Clear();
                Console.WriteLine("You typed a wrong imput. Try again");
                mode = _view.GetMode().ToUpper();
            }
            return mode;
        }

        public void Run()
        {
            //Ask the mode the user wants
            string mode = GetMode();

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
