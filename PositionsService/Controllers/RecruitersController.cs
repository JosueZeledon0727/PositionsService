using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositionsService.Data;
using PositionsService.Models;

namespace PositionsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitersController : ControllerBase
    {
        private readonly DataContext _context;

        public RecruitersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/recruiters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recruiter>>> GetRecruiters()
        {
            var recruiters = await _context.Recruiters.ToListAsync();
            return Ok(recruiters);
        }
    }
}
