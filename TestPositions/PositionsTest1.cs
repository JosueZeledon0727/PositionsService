using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PositionsService.Controllers;
using PositionsService.Data;
using PositionsService.Models;
using PositionsService.Services;
using PositionsService.Hubs;
using Microsoft.AspNetCore.SignalR;
using Xunit;

namespace TestPositions
{
    public class PositionsTest1
    {
        private readonly DataContext _context;
        private readonly PositionsController _controller;
        private readonly Mock<RabbitMqService> _mockRabbitMqService;
        private readonly Mock<IHubContext<PositionHub>> _mockHubContext;

        public PositionsTest1()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestDatabase")  // Using db on memory
            .Options;

            // Creating context with the "DbContextOptions"
            _context = new DataContext(options);

            _mockHubContext = new Mock<IHubContext<PositionHub>>();

            _mockRabbitMqService = new Mock<RabbitMqService>(_mockHubContext.Object);

            // Creating controller with memory context
            _controller = new PositionsController(_context, _mockHubContext.Object, _mockRabbitMqService.Object);


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

            // Act: Trying to create a new position
            var result = await _controller.PostPosition(newPosition);

            // Assert: Verify the bad request response
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Budget must be non-negative.", badRequestResult.Value);
        }

    }
}