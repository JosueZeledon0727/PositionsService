using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PositionsService.Controllers;
using PositionsService.Data;
using PositionsService.Models;
using Xunit;

namespace TestPositions
{
    public class PositionsTest1
    {
        private readonly DataContext _context;
        private readonly PositionsController _controller;

        public PositionsTest1()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestDatabase")  // Usamos una base de datos en memoria
            .Options;

            // Creamos el contexto con el "DbContextOptions" adecuado
            _context = new DataContext(options);

            // Ahora podemos crear el controlador con el contexto en memoria
            _controller = new PositionsController(_context);
        }

        [Fact]
        public async Task PostPosition_ReturnsBadRequest_WhenBudgetIsNegative()
        {
            // Negative budget position creation
            var newPosition = new PositionCreateDto
            {
                PositionNumber = "67890",
                Title = "Testing budget position",
                PositionStatusID = 1,
                DepartmentID = 1,
                RecruiterID = 1,
                Budget = -5000
            };

            // Act: Intentar crear una nueva posición
            var result = await _controller.PostPosition(newPosition);

            // Assert: Verificar que la respuesta sea un BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Budget must be non-negative.", badRequestResult.Value);
        }
    }
}