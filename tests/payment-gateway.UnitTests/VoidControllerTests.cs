using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using payment_gateway.Controllers;
using payment_gateway.Models;
using payment_gateway.Services;
using Xunit;

namespace payment_gateway.Coverlet.Collector
{
    public class VoidControllerTests
    {
        [Fact]
        public async Task Controller_ShouldReturnBadRequest_WhenIdNotFound()
        {
            // Arrange
            Payment payment = null;
            var mockLogger = new Mock<ILogger<VoidController>>();
            var mockService = new Mock<IPaymentService>();

            mockService.Setup(x => x.GetPaymentById(It.IsAny<string>())).ReturnsAsync(payment);

            var controller = new VoidController(mockLogger.Object, mockService.Object);

            // Act
            var actionResult = await controller.Void(It.IsAny<string>());           

            // Assert    
            var result = actionResult.Result as NotFoundResult;
            result.StatusCode.Should().Be(404);
        }
    }
}
