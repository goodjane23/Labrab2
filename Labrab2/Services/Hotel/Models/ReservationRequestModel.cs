using System;

namespace Labrab2.Services.Hotel.Models;

public class ReservationRequestModel
{
    public int ResidentId { get; set; }
    public int ApartmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}