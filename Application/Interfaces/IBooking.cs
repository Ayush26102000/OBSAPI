using Domain;
using Domain.DTOs;

namespace Application.Interfaces;

public interface IBooking
{
    Task<Guid> CreateBooking(CreateBookingDto dto);
    Task<List<Booking>> GetByClinic(Guid clinicId);
}