using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labrab2.Data;
using Labrab2.Data.Entities;
using Labrab2.Services.Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Labrab2.Services.Hotel;

public class HotelService : IHotelService
{
    public event Action<Resident>? OnResidentAdded;
    public event Action<Resident>? OnResidentChecked;
    public event Action<ApartmentReservation>? OnReservationCanceled;

    public async Task<ApartmentReservation> ReserveApartment(ReservationRequestModel model)
    {
        await using var context = new AppDbContext();

        var resident = await context.Residents
            .Where(x => x.Id == model.ResidentId)
            .FirstAsync();

        if (resident is null)
            throw new Exception($"Resident with Id {model.ResidentId} not found!");
        
        var reservation = new ApartmentReservation()
        {
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            ResidentId = model.ResidentId,
            Resident = resident,
            ApartmentId = model.ApartmentId,
        };

        resident.IsAwaitingSettlement = false;
        
        await context.ApartmentReservations.AddAsync(reservation);
        await context.SaveChangesAsync();

        OnResidentChecked?.Invoke(resident);
        
        return reservation;
    }

    public async Task CancelReservation(int reservationId)
    {
        await using var context = new AppDbContext();

        var reservation = await context.ApartmentReservations
            .FindAsync(reservationId);

        if (reservation == null)
            throw new Exception($"Невозможно отменить бронь. Бронь {reservationId} не существует");

        context.ApartmentReservations.Remove(reservation);
        await context.SaveChangesAsync();
        
        OnReservationCanceled?.Invoke(reservation);
    }

    public async Task<IEnumerable<Apartment>> GetAvailableApartments(DateOnly startDate, DateOnly endDate)
    {
        await using var context = new AppDbContext();

        var apartments = await context.Apartments.AsNoTracking()
            .Include(x => x.Reservations)
            .Where(x => x.Reservations.Count == 0 || 
                        !x.Reservations.Any(r => (r.StartDate <= endDate && r.StartDate >= startDate) ||
                                                 (r.EndDate <= endDate && r.EndDate >= startDate)))
            .ToListAsync();

        return apartments;
    }

    public async Task<Resident> CreateResident(CreateResidentRequestModel model)
    {
        await using var context = new AppDbContext();

        var resident = await context.Residents
            .Where(x => x.FullName == model.FullName && 
                        x.Passport == model.Passport)
            .FirstOrDefaultAsync();

        if (resident is not null)
        {
            resident.IsAwaitingSettlement = true;
            await context.SaveChangesAsync();
            
            OnResidentAdded?.Invoke(resident);
            
            return resident;
        }

        resident = new Resident
        {
            FullName = model.FullName,
            Passport = model.Passport,
            Phone = model.Phone,
            IsAwaitingSettlement = true
        };

        await context.Residents.AddAsync(resident);
        await context.SaveChangesAsync();
        
        OnResidentAdded?.Invoke(resident);
        
        return resident;
    }

    public async Task<IEnumerable<Resident>> GetResidents()
    {
        await using var context = new AppDbContext();

        var residents = await context.Residents.AsNoTracking()
            .Where(x => x.IsAwaitingSettlement)
            .ToListAsync();
        
        return residents;
    }

    public async Task<IEnumerable<ApartmentReservation>> GetReservations()
    {
        await using var context = new AppDbContext();

        var reservations = await context.ApartmentReservations.AsNoTracking()
            .Include(x => x.Resident)
            .ToListAsync();

        return reservations;
    }
}