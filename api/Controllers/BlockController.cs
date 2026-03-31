using Domain;
using Domain.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/admin/block")]
    public class BlockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Block(BlockedSlot model)
        {
            _context.BlockedSlots.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("confirm/{id}")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            booking.Status = "Confirmed";
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("services/manage/{clinicId}")]
        public async Task<IActionResult> ManageServices(Guid clinicId, List<ServiceDto> services)
        {
            if (services == null)
                return BadRequest("Invalid data");

            var existingServices = await _context.Services
                .Where(s => s.ClinicId == clinicId)
                .ToListAsync();

            // 🧠 Track incoming IDs
            var incomingIds = services.Where(s => s.Id.HasValue)
                                      .Select(s => s.Id.Value)
                                      .ToList();

            // 🔄 ADD + UPDATE
            foreach (var dto in services)
            {
                if (dto.Id.HasValue)
                {
                    // ✏️ UPDATE
                    var existing = existingServices.FirstOrDefault(s => s.Id == dto.Id.Value);

                    if (existing != null)
                    {
                        existing.Name = dto.Name;
                        existing.DurationMinutes = dto.DurationMinutes;
                    }
                }
                else
                {
                    // ➕ ADD
                    var newService = new Service
                    {
                        Id = Guid.NewGuid(),
                        ClinicId = clinicId,
                        Name = dto.Name,
                        DurationMinutes = dto.DurationMinutes
                    };

                    _context.Services.Add(newService);
                }
            }

            // ❌ DELETE (anything not sent from frontend)
            var toDelete = existingServices
                .Where(s => !incomingIds.Contains(s.Id))
                .ToList();

            _context.Services.RemoveRange(toDelete);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Services updated successfully" });
        }

        [HttpGet("services/{clinicId}")]
        public async Task<IActionResult> GetServices(Guid clinicId)
        {
            var services = await _context.Services
                .Where(s => s.ClinicId == clinicId)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.DurationMinutes
                })
                .ToListAsync();

            return Ok(services);
        }
    }
}
