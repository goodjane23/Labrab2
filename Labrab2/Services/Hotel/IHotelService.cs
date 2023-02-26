using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Labrab2.Data.Entities;
using Labrab2.Services.Hotel.Models;

namespace Labrab2.Services.Hotel;

public interface IHotelService
{
    public event Action<Resident>? OnResidentAdded; 
    public event Action<Resident>? OnResidentChecked;
    public event Action<ApartmentReservation>? OnReservationCanceled;

    public Task<ApartmentReservation> ReserveApartment(ReservationRequestModel model);
    public Task CancelReservation(int reservationId);
    public Task<IEnumerable<Apartment>> GetAvailableApartments(DateOnly startDate, DateOnly endDate);
    public Task<Resident> CreateResident(CreateResidentRequestModel model);
    public Task<IEnumerable<Resident>> GetResidents();
    public Task<IEnumerable<ApartmentReservation>> GetReservations();
}