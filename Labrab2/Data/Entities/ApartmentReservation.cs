using System;

namespace Labrab2.Data.Entities;

public class ApartmentReservation
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public Apartment Apartment { get; set; }
    public int ApartmentId { get; set; }

    public Resident Resident { get; set; }
    public int ResidentId { get; set; }
}
