using System;
using System.Windows;
using Labrab2.ViewModels;
using Wpf.Ui.Controls.Window;

namespace Labrab2.Views;

public partial class ReservationsListWindow : FluentWindow
{
    public ReservationsListWindow()
    {
        DataContext = new ReservationsListWindowViewModel();
        
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        
        ((ReservationsListWindowViewModel)DataContext).Dispose();
    }
}