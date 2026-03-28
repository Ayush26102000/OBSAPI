using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.DTOs;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBooking _service;

    public BookingController(IBooking service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
    {
        var id = await _service.CreateBooking(dto);
        return Ok(new { bookingId = id });
    }

    [HttpGet("{clinicId}")]
    public async Task<IActionResult> Get(Guid clinicId)
    {
        var data = await _service.GetByClinic(clinicId);
        return Ok(data);
    }


}