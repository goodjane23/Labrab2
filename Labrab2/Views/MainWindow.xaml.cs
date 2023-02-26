using System.Windows;
using Labrab2.ViewModels;
using Wpf.Ui.Controls.Window;

namespace Labrab2.Views;

public partial class MainWindow : FluentWindow 
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        
        InitializeComponent();
    }

    private void NewResidentButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new NewResidentWindow();
        window.ShowDialog();
    }

    private void ReservationsListButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new ReservationsListWindow();
        window.ShowDialog();
    }
}
