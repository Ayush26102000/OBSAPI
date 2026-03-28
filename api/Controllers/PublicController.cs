using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        private readonly SlotService _slotService;
        private readonly AppDbContext _context;

        public PublicController(SlotService slotService, AppDbContext context)
        {
            _slotService = slotService;
            _context = context;
        }

        // GET services
        [HttpGet("services/{clinicId}")]
        public async Task<IActionResult> GetServices(Guid clinicId)
        {
            var services = await _context.Services
                .Where(x => x.ClinicId == clinicId)
                .ToListAsync();

            return Ok(services);
        }

        // GET available slots
        [HttpGet("slots")]
        public async Task<IActionResult> GetSlots(Guid clinicId, DateTime date)
        {
            var slots = await _slotService.GetAvailableSlots(clinicId, date);
            return Ok(slots);
        }
    }
}
