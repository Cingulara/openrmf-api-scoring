using System;
using Xunit;
using openrmf_scoring_api.Controllers;
using openrmf_scoring_api.Data;
using openrmf_scoring_api.Models;
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

    public class ScoreControllerTests
    {
        private readonly Mock<ILogger<ScoreController>> _mockLogger;
        private readonly ScoreController _controller; 

        public ScoreControllerTests() {
            _mockLogger = new Mock<ILogger<ScoreController>>();
            //_controller = new ScoreController(_mockLogger.Object);
        }

        [Fact]
        public void Test_ScoreControllerIsValid()
        {
            Assert.True(_controller != null);
        }

        // [Fact]
        // public void Test_HealthControllerGetIsValid()
        // {
        //     var result = _healthController.Get();
        //     Assert.True(_healthController != null);
        //     Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)result.Result).StatusCode); // returns a status code HTTP 200
        // }
    }
}
