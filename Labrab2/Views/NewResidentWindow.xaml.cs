using System.Windows;
using Labrab2.ViewModels;
using Wpf.Ui.Controls.Window;

namespace Labrab2.Views;

public partial class NewResidentWindow : FluentWindow
{
    public NewResidentWindow()
    {
        DataContext = new NewResidentWindowViewModel();
        
        InitializeComponent();
    }
}