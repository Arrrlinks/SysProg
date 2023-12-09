using System;
using Windowsform = System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace EasySave_Graphique
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            Windowsform.FolderBrowserDialog openfile = new Windowsform.FolderBrowserDialog();
            Windowsform.DialogResult result = openfile.ShowDialog();
            if (result == Windowsform.DialogResult.OK)
            {
                string foldername = openfile.SelectedPath;
                //text.Text = foldername;
            }
            // Show the dialog and get the result
        }

        private void FileButton_OnClick(object sender, RoutedEventArgs e)
        {
            Windowsform.OpenFileDialog openfile = new Windowsform.OpenFileDialog();
            Windowsform.DialogResult result = openfile.ShowDialog();
            if (result == Windowsform.DialogResult.OK)
            {
                string filename = openfile.FileName;
                //text.Text = filename;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
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
    }
    
}

