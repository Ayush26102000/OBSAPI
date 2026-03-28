using System;
using Application.Interfaces;
using Domain;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class BookingService : IBooking
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateBooking(CreateBookingDto dto)
    {
        var exists = await _context.Bookings.AnyAsync(x =>
            x.ClinicId == dto.ClinicId &&
            x.AppointmentDate == dto.AppointmentDate);

        if (exists)
            throw new Exception("Slot already booked");

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            ClinicId = dto.ClinicId,
            ServiceId = dto.ServiceId,
            PatientName = dto.PatientName,
            Phone = dto.Phone,
            AppointmentDate = dto.AppointmentDate,
            Status = "Pending"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return booking.Id;
    }

    public async Task<List<Booking>> GetByClinic(Guid clinicId)
    {
        var res = await _context.Bookings
            .Where(x => x.ClinicId == clinicId)
            .ToListAsync();
        return res;
    }
}