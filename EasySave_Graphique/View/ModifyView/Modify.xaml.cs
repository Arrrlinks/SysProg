using EasySave_Graphique.ViewModels;

namespace EasySave_Graphique.View.ModifyView;

public partial class Modify
{
    public Modify()
    {
        Modify_vm vm = new Modify_vm();
        DataContext = vm;
        InitializeComponent();
    }
}