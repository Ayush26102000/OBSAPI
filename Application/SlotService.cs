using Microsoft.EntityFrameworkCore;
using Domain;

public class SlotService
{
    private readonly AppDbContext _context;

    public SlotService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DateTime>> GetAvailableSlots(Guid clinicId, DateTime date)
    {
        var day = (int)date.DayOfWeek;

        var availability = await _context.Availability
            .FirstOrDefaultAsync(x => x.ClinicId == clinicId && x.DayOfWeek == day);

        if (availability == null)
            return new List<DateTime>();

        var existingBookings = await _context.Bookings
            .Where(x => x.ClinicId == clinicId && x.AppointmentDate.Date == date.Date)
            .Select(x => x.AppointmentDate)
            .ToListAsync();

        var blocked = await _context.BlockedSlots
            .Where(x => x.ClinicId == clinicId && x.BlockedDate.Date == date.Date)
            .ToListAsync();

        var slots = new List<DateTime>();

        var start = date.Date.Add(availability.StartTime);
        var end = date.Date.Add(availability.EndTime);

        while (start < end)
        {
            var isBooked = existingBookings.Contains(start);

            var isBlocked = blocked.Any(b =>
                start.TimeOfDay >= b.StartTime &&
                start.TimeOfDay < b.EndTime);

            if (!isBooked && !isBlocked)
                slots.Add(start);

            start = start.AddMinutes(30); // slot duration
        }

        return slots;
    }
}