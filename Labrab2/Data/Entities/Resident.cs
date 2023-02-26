using System.Collections.Generic;

namespace Labrab2.Data.Entities;

public class Resident
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Passport { get; set; }
    public string Phone { get; set; }
    public bool IsAwaitingSettlement { get; set; }

    public ICollection<ApartmentReservation> Reservations { get; set; }
}