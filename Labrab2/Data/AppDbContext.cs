using System;
using System.Collections.Generic;
using Labrab2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Labrab2.Data;

public class AppDbContext : DbContext
{
    public DbSet<Resident> Residents { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<ApartmentReservation> ApartmentReservations { get; set; }
    
    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Data\\storage.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Apartment>()
            .HasData(GetInitialApartments());

        modelBuilder.Entity<ApartmentReservation>()
            .HasData(GetInitialReservations());

        modelBuilder.Entity<Resident>()
            .HasData(GetInitialResidents());
    }

    private IEnumerable<Apartment> GetInitialApartments()
    {
        var apartments = new List<Apartment>()
        {
            new Apartment()
            {
                Id = 1,
                RoomCount = 1,
                PricePerDay = 150
            },
            new Apartment()
            {
                Id = 2,
                RoomCount = 2,
                PricePerDay = 250
            },
            new Apartment()
            {
                Id = 3,
                RoomCount = 1,
                PricePerDay = 200
            },
        };

        return apartments;
    }

    private IEnumerable<ApartmentReservation> GetInitialReservations()
    {
        var reservations = new List<ApartmentReservation>()
        {
            new ApartmentReservation()
            {
                Id = 1,
                ApartmentId = 1,
                ResidentId = 1,
                StartDate = new DateOnly(2023, 2, 1),
                EndDate = new DateOnly(2023, 2, 5)
            },
            new ApartmentReservation()
            {
                Id = 2,
                ApartmentId = 2,
                ResidentId = 2,
                StartDate = new DateOnly(2023, 2, 10),
                EndDate = new DateOnly(2023, 2, 15)
            },
        };

        return reservations;
    }

    private IEnumerable<Resident> GetInitialResidents()
    {
        var residents = new List<Resident>()
        {
            new Resident()
            {
                Id = 1,
                FullName = "Виктор Иванович",
                Passport = "123-456",
                Phone = "+1 (123) 456 78-90",
                IsAwaitingSettlement = true
            },
            new Resident()
            {
                Id = 2,
                FullName = "Иван Викторович",
                Passport = "123-456",
                Phone = "+1 (123) 456 78-90",
                IsAwaitingSettlement = true
            },
        };

        return residents;
    }
}