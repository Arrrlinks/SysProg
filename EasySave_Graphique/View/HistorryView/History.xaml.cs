using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EasySave_Graphique.Models;

namespace EasySave_Graphique.View.HistorryView;

public partial class History : UserControl, INotifyPropertyChanged
{
    private ObservableCollection<history_m> _logs;

    public DateTime TodayDate { get; set; } = DateTime.Today;

    public ObservableCollection<history_m> Logs
    {
        get { return _logs; }
        set
        {
            _logs = value;
            OnPropertyChanged(nameof(Logs));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public History()
    {
        InitializeComponent();
        this.DataContext = this;
        log_m log = new log_m();
        Logs = log.GetHistoryFromLogFile(TodayDate.ToString("yyyyMMdd"));
    }

    private void Submit_Click(object sender, RoutedEventArgs e)
    {
        log_m log = new log_m();
        DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Today;
        Logs = log.GetHistoryFromLogFile(selectedDate.ToString("yyyyMMdd"));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}