using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasySave.ViewName;


namespace EasySave.ViewModelName
{
    public class ViewModel
    {
        //Builders
        private View _view;

        public ViewModel() 
        {
            _view = new View();
        }
        public ViewModel(View view)
        {
            _view = view;
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



    }
}
