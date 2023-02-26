using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labrab2.Services.Hotel;
using Labrab2.Services.Hotel.Models;
using System.Threading.Tasks;

namespace Labrab2.ViewModels;

public partial class NewResidentWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = "Дмитрий Петрович";
    
    [ObservableProperty]
    private string passport = "228-420";
    
    [ObservableProperty]
    private string phone = "+7 (228) 420 14-88";

    public IRelayCommand CreateResidentCommand { get; set; } 

    private readonly IHotelService hotelService;
    
    public NewResidentWindowViewModel()
    {
        hotelService = App.HotelService;

        CreateResidentCommand = new AsyncRelayCommand(CreateResident);
    }

    private async Task CreateResident()
    {
        var resident = new CreateResidentRequestModel()
        {
            FullName = Name,
            Passport = Passport,
            Phone = Phone,
        };

        await hotelService.CreateResident(resident);
    }
}