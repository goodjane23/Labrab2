using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labrab2.Data.Entities;
using Labrab2.Services.Hotel;
using Labrab2.Services.Hotel.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Labrab2.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Resident> Residents { get; }
    public ObservableCollection<Apartment> AvailableApartments { get; }

    public string TotalPrice => GetTotalPrice();
    
    [ObservableProperty]
    private Resident? selectedResident;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))]
    private Apartment? selectedApartment;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))]
    private DateTime startDate = DateTime.UtcNow;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalPrice))]
    private DateTime endDate = DateTime.UtcNow;

    public CommunityToolkit.Mvvm.Input.IRelayCommand<Snackbar> GetApartmentsCommand { get; }
    public CommunityToolkit.Mvvm.Input.IRelayCommand<Snackbar> ReserveApartmentCommand { get; }
    
    private readonly IHotelService hotelService;
    
    public MainWindowViewModel()
    {
        hotelService = App.HotelService;

        Residents = new ObservableCollection<Resident>(hotelService.GetResidents().Result);
        AvailableApartments = new ObservableCollection<Apartment>();
        
        hotelService.OnResidentAdded += (resident) => Residents.Add(resident);
        hotelService.OnResidentChecked += OnResidentChecked;

        GetApartmentsCommand = new AsyncRelayCommand<Snackbar>(GetAvailableApartments);
        ReserveApartmentCommand = new AsyncRelayCommand<Snackbar>(ReserveApartment);
    }

    private async Task GetAvailableApartments(Snackbar snackbar)
    {
        if (EndDate < StartDate)
        {
            snackbar.Appearance = ControlAppearance.Danger;
            snackbar.Icon = SymbolRegular.CommentError24;
            snackbar.Timeout = 3000;

            await snackbar.ShowAsync("Ошибка поиска номеров", "Дата окончания не может быть раньше начальной даты!");
            
            return;
        }
        
        var start = DateOnly.FromDateTime(StartDate);
        var end = DateOnly.FromDateTime(EndDate);
        
        var apartments = await hotelService.GetAvailableApartments(start, end);
        
        AvailableApartments.Clear();
        
        foreach (var apartment in apartments)
            AvailableApartments.Add(apartment);

        if (AvailableApartments.Count > 0)
            SelectedApartment = AvailableApartments[0];
    }

    private async Task ReserveApartment(Snackbar snackbar)
    {
        if (SelectedApartment is null || SelectedResident is null)
            return;

        if (EndDate < StartDate)
        {
            snackbar.Appearance = ControlAppearance.Danger;
            snackbar.Icon = SymbolRegular.CommentError24;
            snackbar.Timeout = 3000;

            await snackbar.ShowAsync("Ошибка оформления", "Дата окончания не может быть раньше начальной даты!");
            
            return;
        }
        
        var start = DateOnly.FromDateTime(StartDate);
        var end = DateOnly.FromDateTime(EndDate);

        var reservationRequestModel = new ReservationRequestModel()
        {
            ResidentId = SelectedResident.Id,
            ApartmentId = SelectedApartment.Id,
            StartDate = start,
            EndDate = end,
        };
        
        var reservation = await hotelService.ReserveApartment(reservationRequestModel);
        var residentName = reservation.Resident.FullName;
        
        snackbar.Appearance = ControlAppearance.Success;
        snackbar.Icon = SymbolRegular.CalendarCheckmark24;
        snackbar.Timeout = 2000;

        await snackbar.ShowAsync("Бронь оформлена", $"Бронь для {residentName} успешно оформлена!");
    }

    private void OnResidentChecked(Resident checkedResident)
    {
        var resident = Residents.First(x => x.Id == checkedResident.Id);
        Residents.Remove(resident);
    }

    private string GetTotalPrice()
    {
        var daysCount = (EndDate - StartDate).Days;
        var apartmentPrice = SelectedApartment?.PricePerDay ?? 0;

        if (daysCount == 0) daysCount = 1;
        if (daysCount < 0) daysCount = 0;
        
        return $"${apartmentPrice * daysCount} за {daysCount} дн.";
    }
}