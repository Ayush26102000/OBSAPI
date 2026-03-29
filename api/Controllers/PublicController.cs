using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Domain.DTOS.LeadDTO;

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


        [HttpPost("lead/abandon")]
        public async Task<IActionResult> CaptureAbandon([FromBody] AbandonLeadDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Phone))
                return Ok(); // silently ignore

            var lead = await _context.Leads
                .FirstOrDefaultAsync(x => x.Phone == dto.Phone);

            if (lead != null)
            {
                lead.Status = "DROPPED";
                lead.StepDropped = dto.Step;
                lead.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        [HttpGet("lead/{clinicId}")]
        public async Task<IActionResult> GetLeads(Guid clinicId)
        {
            var leads = await _context.Leads
                .Where(x => x.ClinicId == clinicId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(leads);
        }

        [HttpPost("lead")]
        public async Task<IActionResult> CaptureLead([FromBody] CreateLeadDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Phone))
                return BadRequest("Phone is required");

            // 🔥 Check if already exists
            var existing = await _context.Leads
                .FirstOrDefaultAsync(x => x.Phone == dto.Phone && x.ClinicId == dto.ClinicId);

            if (existing != null)
            {
                // Update existing lead
                existing.Name = dto.Name;
                existing.Status = dto.Status ?? "INTERESTED";
                existing.Source = dto.Source;
                existing.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(existing);
            }

            // Create new lead
            var lead = new Lead
            {
                Id = Guid.NewGuid(),
                ClinicId = dto.ClinicId,
                Name = dto.Name,
                Phone = dto.Phone,
                Status = dto.Status ?? "INTERESTED",
                Source = dto.Source,
                CreatedAt = DateTime.UtcNow
            };

            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            return Ok(lead);
        }
    }
}
