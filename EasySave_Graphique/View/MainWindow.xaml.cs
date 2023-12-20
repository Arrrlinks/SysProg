using System.Windows;
using EasySave_Graphique.Models;

namespace EasySave_Graphique
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        state_m _state = new state_m();
        public MainWindow()
        {
            _state.AbortPausedTasksOnStartup();
            InitializeComponent();
        }
    }
}

