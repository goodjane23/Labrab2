using System.Collections.Generic;

namespace Labrab2.Data.Entities;

public class Apartment
{
    public int Id { get; set; }
    public int RoomCount { get; set; }
    public int PricePerDay { get; set; }

    public ICollection<ApartmentReservation> Reservations { get; set; }
}
