using System.Windows.Controls;
using EasySave_Graphique.ViewModels;

namespace EasySave_Graphique.View.ModifyView;

public partial class Modify : UserControl
{
    public Modify()
    {
        Modify_vm vm = new Modify_vm();
        DataContext = vm;
        InitializeComponent();
    }
}