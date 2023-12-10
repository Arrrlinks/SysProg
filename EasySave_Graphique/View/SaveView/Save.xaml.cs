using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySave_Graphique.View.SaveView;

public partial class Save : UserControl
{
    public Save()
    {
        InitializeComponent();
    }
    
    private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is CheckBox)
        {
            // Laisser la sélection se produire
        }
        else
        {
            // Annuler la sélection en empêchant la propagation de l'événement
            e.Handled = true;
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
    }
    
}