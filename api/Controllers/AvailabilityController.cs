using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/admin/availability")]
    public class AvailabilityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvailabilityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SetAvailability(Availability model)
        {
            _context.Availability.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{clinicId}")]
        public async Task<IActionResult> Get(Guid clinicId)
        {
            var data = await _context.Availability
                .Where(x => x.ClinicId == clinicId)
                .ToListAsync();

            return Ok(data);
        }
    }
}
