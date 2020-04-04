using System;
using Xunit;
using openrmf_scoring_api.Controllers;
using openrmf_scoring_api.Data;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace tests.Controllers
{

    public class HealthControllerTests
    {
        private readonly Mock<ILogger<HealthController>> _mockLogger;
        private readonly HealthController _healthController; 
        private readonly Mock<IScoreRepository> _mockscoreRepo;

        public HealthControllerTests() {
            _mockLogger = new Mock<ILogger<HealthController>>();
            _mockscoreRepo = new Mock<IScoreRepository>();
            _healthController = new HealthController(_mockscoreRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public void Test_HealthControllerIsValid()
        {
            Assert.True(_healthController != null);
        }

        [Fact]
        public void Test_HealthControllerGetIsValid()
        {
            var result = _healthController.Get();
            Assert.True(_healthController != null);
            //Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode); // returns a status code HTTP 200
        }
    }
}
