using Microsoft.EntityFrameworkCore;
using Domain;
using System.Collections.Generic;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Availability> Availability { get; set; }
    public DbSet<BlockedSlot> BlockedSlots { get; set; }

    public DbSet<Lead> Leads { get; set; }

}