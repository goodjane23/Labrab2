using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labrab2.Data.Entities;
using Labrab2.Services.Hotel;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Labrab2.ViewModels;

public partial class ReservationsListWindowViewModel : ObservableObject, IDisposable
{
    public ObservableCollection<ApartmentReservation> Reservations { get; }

    [ObservableProperty]
    private ApartmentReservation? selectedReservation;
    
    public IRelayCommand<Snackbar> CancelReservationCommand { get; }    

    private readonly IHotelService hotelService;
    
    public ReservationsListWindowViewModel()
    {
        hotelService = App.HotelService;

        hotelService.OnReservationCanceled += OnReservationCanceled;
        
        Reservations = new ObservableCollection<ApartmentReservation>(hotelService.GetReservations().Result);

        CancelReservationCommand = new AsyncRelayCommand<Snackbar>(CancelReservation);
    }

    private async Task CancelReservation(Snackbar snackbar)
    {
        if (SelectedReservation is null)
            return;
        
        var residentName = SelectedReservation.Resident.FullName;
        
        await hotelService.CancelReservation(SelectedReservation.Id);

        snackbar.Message = $"Бронь для {residentName} успешно отменена!";
        await snackbar.ShowAsync();
    }

    private void OnReservationCanceled(ApartmentReservation canceledReservation)
    {
        var reservation = Reservations.FirstOrDefault(x => x.Id == canceledReservation.Id);
        
        if (reservation is null)
            return;
        
        Reservations.Remove(reservation);
    }

    public void Dispose()
    {
        hotelService.OnReservationCanceled -= OnReservationCanceled;
    }
}