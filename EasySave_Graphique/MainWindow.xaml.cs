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
                if (!(text2.Text == ""))
                {
                    text2.Text = "";
                }
                string foldername = openfile.SelectedPath;
                text1.Text = foldername;
            }
            // Show the dialog and get the result
        }

        private void FileButton_OnClick(object sender, RoutedEventArgs e)
        {
            Windowsform.OpenFileDialog openfile = new Windowsform.OpenFileDialog();
            Windowsform.DialogResult result = openfile.ShowDialog();
            if (result == Windowsform.DialogResult.OK)
            {
                if (!(text1.Text == ""))
                {
                    text1.Text = "";
                }
                string filename = openfile.FileName;
                text2.Text = filename;
            }
        }
    }
}