using System;
using System.Windows.Forms;
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

namespace EasySave_Graphique // Namespace of the project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window // Class of the main window
    {
        public MainWindow() // Constructor of the main window
        {
            InitializeComponent(); // Initialize the main window
        }
        private void PathChooseMenu(object sender, RoutedEventArgs e) // Method to open the path choose window
        {
            PathChoose pathChoosePage = new PathChoose(); // Create a new path choose page
            pathChoosePage.PathSelected += PathChoosePage_PathSelected; // Add an event handler to the path choose page
            Window newWindow = new Window // Create a new window
            {
                Title = "Choose File Type", // Set the title of the window
                Content = pathChoosePage, // Set the content of the window
                SizeToContent = SizeToContent.WidthAndHeight // Set the size of the window
            };
            newWindow.Show(); // Show the window
        }

        private void PathChoosePage_PathSelected(object sender, PathSelectedEventArgs e) // Method to handle the path selected event
        {
            TextBox.Text = e.SelectedPath; // Set the text of the text box to the selected path
        }
        
        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsPage = new Settings();
            Window newWindow = new Window
            {
                Title = "Settings",
                Content = settingsPage,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };
            newWindow.Show();
        }
    }
}