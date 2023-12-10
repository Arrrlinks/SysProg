using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace EasySave_Graphique
{
    public partial class PathChoose : Page
    {
        // Define the PathSelected event
        public event EventHandler<PathSelectedEventArgs> PathSelected;

        public PathChoose()
        {
            InitializeComponent();
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a file dialog
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                // Raise the PathSelected event with the selected file path
                PathSelected?.Invoke(this, new PathSelectedEventArgs(fileDialog.FileName));

                // Close the window
                Window.GetWindow(this).Close();
            }
        }

        private void DirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a directory dialog
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Raise the PathSelected event with the selected directory path
                PathSelected?.Invoke(this, new PathSelectedEventArgs(folderDialog.SelectedPath));

                // Close the window
                Window.GetWindow(this).Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Window.GetWindow(this).Close();
        }
    }

    // Define the PathSelectedEventArgs class
    public class PathSelectedEventArgs : EventArgs
    {
        public string SelectedPath { get; }

        public PathSelectedEventArgs(string selectedPath)
        {
            SelectedPath = selectedPath;
        }
    }
}