using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using openrmf_scoring_api.Controllers;
using openrmf_scoring_api.Data;
using Xunit;

namespace tests.Controllers
{

    public class HealthControllerTests
    {
        private readonly Mock<ILogger<HealthController>> _mockLogger;
        private readonly Mock<IScoreRepository> _mockScoreRepo;

        public HealthControllerTests()
        {
            _mockLogger = new Mock<ILogger<HealthController>>();
            _mockScoreRepo = new Mock<IScoreRepository>();
        }

        [Fact]
        public void Get_ReturnsOk_WhenRepositoryIsHealthy()
        {
            _mockScoreRepo.Setup(x => x.HealthStatus()).Returns(true);
            var controller = new HealthController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("ok", okResult.Value);
            Assert.NotEqual("database error", okResult.Value);
            _mockScoreRepo.Verify(x => x.HealthStatus(), Times.Once);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenRepositoryIsUnhealthy()
        {
            _mockScoreRepo.Setup(x => x.HealthStatus()).Returns(false);
            var controller = new HealthController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = controller.Get();

            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("database error", badResult.Value);
            Assert.NotEqual("ok", badResult.Value);
            _mockScoreRepo.Verify(x => x.HealthStatus(), Times.Once);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenRepositoryThrows()
        {
            _mockScoreRepo.Setup(x => x.HealthStatus()).Throws(new Exception("db failure"));
            var controller = new HealthController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = controller.Get();

            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Improper API configuration", badResult.Value);
            Assert.NotEqual("ok", badResult.Value);
            _mockScoreRepo.Verify(x => x.HealthStatus(), Times.Once);
        }
    }
}
