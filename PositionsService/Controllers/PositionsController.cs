using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositionsService.Data;
using PositionsService.Models;

namespace PositionsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly DataContext _context;
        public PositionsController(DataContext context)
        {
            _context = context;
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        //{
        //    // Obtener todas las cuentas
        //    var cuentas = await _cuentaRepository.GetAllAsync();
        //    return Ok(cuentas);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            //// Obtener todas las cuentas
            //var positions = new List<Position> { new Position { 
            //    PositionID = 1,
            //    PositionNumber = "1111",
            //    Title = "Software Engineer",
            //    Budget = 15000
            //}};
            //return Ok(positions);
            try
            {
                // Consultar todas las posiciones, incluyendo las entidades relacionadas (Status, Department, Recruiter)
                var positions = await _context.Positions
                    .Include(p => p.Status)   
                    .Include(p => p.Department) 
                    .Include(p => p.Recruiter)  
                    .ToListAsync();  

                return Ok(positions); // Retorna un código 200 (OK) con las posiciones
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
        public async Task<IActionResult> PutPosition(int id, Position position)
        {
            try
            {
                if (id != position.PositionID)
                {
                    return BadRequest("Position ID mismatch.");
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

                _context.Entry(position).State = EntityState.Modified;

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
