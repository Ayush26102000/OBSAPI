using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
