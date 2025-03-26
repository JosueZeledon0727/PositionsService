using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositionsService.Data;
using PositionsService.Models;
using Microsoft.AspNetCore.SignalR;
using PositionsService.Hubs;

namespace PositionsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHubContext<PositionHub> _hubContext;

        public PositionsController(DataContext context, IHubContext<PositionHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            try
            {
                var positions = await _context.Positions
                    .Include(p => p.Status)   
                    .Include(p => p.Department) 
                    .Include(p => p.Recruiter)  
                    .ToListAsync();  

                return Ok(positions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPosition(int id)
        {
            var position = await _context.Positions
                .Include(p => p.Status)
                .Include(p => p.Department)
                .Include(p => p.Recruiter)
                .FirstOrDefaultAsync(p => p.PositionID == id);

            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }


        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(PositionCreateDto position)
        {
            // Validations for Null position number and budget non negative
            if (string.IsNullOrWhiteSpace(position.PositionNumber))
            {
                return BadRequest("PositionNumber is required.");
            }
            if (position.Budget < 0)
            {
                return BadRequest("Budget must be non-negative.");
            }
            
            if (await _context.Positions.AnyAsync(p => p.PositionNumber == position.PositionNumber))
            {
                return Conflict("PositionNumber must be unique.");
            }

            try
            {
                var positionToInsert = new Position
                {
                    PositionNumber = position.PositionNumber,
                    Title = position.Title,
                    PositionStatusID = position.PositionStatusID,
                    DepartmentID = position.DepartmentID,
                    RecruiterID = position.RecruiterID,
                    Budget = position.Budget
                };

                // Adds new position
                _context.Positions.Add(positionToInsert);
                await _context.SaveChangesAsync();

                // Notify Clients through SignalR
                await _hubContext.Clients.All.SendAsync("PositionUpdated", positionToInsert.PositionNumber);

                return CreatedAtAction(nameof(GetPosition), new { id = positionToInsert.PositionID }, positionToInsert);
            } 
            catch (DbUpdateException ex)    // If there's an error with the DB
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message} ");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, PositionCreateDto position)
        {
            try
            {
                var existingPosition = await _context.Positions.FirstOrDefaultAsync(p => p.PositionID == id);

                if (existingPosition == null)
                {
                    return NotFound("Position not found.");
                }

                // Verify that position number is not null or empty
                if (string.IsNullOrWhiteSpace(position.PositionNumber))
                {
                    return BadRequest("PositionNumber is required.");
                }

                // Validation of non negative budget
                if (position.Budget < 0)
                {
                    return BadRequest("Budget must be non-negative.");
                }

                existingPosition.PositionNumber = position.PositionNumber;
                existingPosition.Title = position.Title;
                existingPosition.PositionStatusID = position.PositionStatusID;
                existingPosition.DepartmentID = position.DepartmentID;
                existingPosition.RecruiterID = position.RecruiterID;
                existingPosition.Budget = position.Budget;

                _context.Entry(existingPosition).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            try
            {
                var position = await _context.Positions.FindAsync(id);
                if (position == null)
                {
                    return NotFound();
                }

                _context.Positions.Remove(position);    // Delete from db
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
