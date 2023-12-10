using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySave_Graphique.ViewModels;

//This class is used to implement the INotifyPropertyChanged interface
//This interface is used to update the UI when a property is changed
public class Base_vm : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    //This method is used to update the UI when a property is changed
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}