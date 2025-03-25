using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositionsService.Data;
using PositionsService.Models;

namespace PositionsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionStatusesController : ControllerBase
    {
        private readonly DataContext _context;

        public PositionStatusesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/positionstatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionStatus>>> GetPositionStatuses()
        {
            var positionStatuses = await _context.PositionStatuses.ToListAsync();
            return Ok(positionStatuses);
        }
    }
}
