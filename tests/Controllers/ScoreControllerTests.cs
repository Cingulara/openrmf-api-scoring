using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using openrmf_scoring_api.Controllers;
using openrmf_scoring_api.Data;
using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Controllers
{
    public class ScoreControllerTests
    {
        private readonly Mock<IScoreRepository> _mockScoreRepo;
        private readonly Mock<ILogger<ScoreController>> _mockLogger;

        public ScoreControllerTests()
        {
            _mockScoreRepo = new Mock<IScoreRepository>();
            _mockLogger = new Mock<ILogger<ScoreController>>();
        }

        [Fact]
        public async Task GetScore_ReturnsOk_WhenScoreExists()
        {
            var expected = new Score { hostName = "host-a", totalCat1Open = 1 };
            _mockScoreRepo.Setup(x => x.GetScore("score-id")).ReturnsAsync(expected);
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScore("score-id");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<Score>(okResult.Value);
            Assert.Equal("host-a", actual.hostName);
            Assert.NotEqual("host-b", actual.hostName);
            _mockScoreRepo.Verify(x => x.GetScore("score-id"), Times.Once);
        }

        [Fact]
        public async Task GetScore_ReturnsNotFound_WhenScoreMissing()
        {
            _mockScoreRepo.Setup(x => x.GetScore("missing")).ReturnsAsync((Score)null);
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScore("missing");

            Assert.IsType<NotFoundResult>(result);
            _mockScoreRepo.Verify(x => x.GetScore("missing"), Times.Once);
        }

        [Fact]
        public async Task GetScore_ReturnsBadRequest_WhenRepositoryThrows()
        {
            _mockScoreRepo.Setup(x => x.GetScore("boom")).ThrowsAsync(new Exception("failure"));
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScore("boom");

            Assert.IsType<BadRequestResult>(result);
            _mockScoreRepo.Verify(x => x.GetScore("boom"), Times.Once);
        }

        [Fact]
        public async Task GetScoreByArtifact_ReturnsOk_WhenScoreExists()
        {
            var expected = new Score { hostName = "host-artifact", totalCat2Open = 2 };
            _mockScoreRepo.Setup(x => x.GetScorebyArtifact("artifact-id")).ReturnsAsync(expected);
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScoreByArtifact("artifact-id");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<Score>(okResult.Value);
            Assert.Equal("host-artifact", actual.hostName);
            Assert.NotEqual(0, actual.totalCat2Open);
            _mockScoreRepo.Verify(x => x.GetScorebyArtifact("artifact-id"), Times.Once);
        }

        [Fact]
        public async Task GetScoreByArtifact_ReturnsNotFound_WhenScoreMissing()
        {
            _mockScoreRepo.Setup(x => x.GetScorebyArtifact("missing")).ReturnsAsync((Score)null);
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScoreByArtifact("missing");

            Assert.IsType<NotFoundResult>(result);
            _mockScoreRepo.Verify(x => x.GetScorebyArtifact("missing"), Times.Once);
        }

        [Fact]
        public async Task GetScoreBySystem_ReturnsNotFound_WhenSystemHasNoRecords()
        {
            _mockScoreRepo.Setup(x => x.GetScoresbySystem("sys-1")).ReturnsAsync((IEnumerable<Score>)null);
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScoreBySystem("sys-1");

            Assert.IsType<NotFoundResult>(result);
            _mockScoreRepo.Verify(x => x.GetScoresbySystem("sys-1"), Times.Once);
        }

        [Fact]
        public async Task GetScoreBySystem_ReturnsAggregatedScore_WhenRecordsExist()
        {
            var scores = new List<Score>
            {
                new Score
                {
                    totalCat1Open = 1,
                    totalCat1NotApplicable = 2,
                    totalCat1NotAFinding = 3,
                    totalCat1NotReviewed = 4,
                    totalCat2Open = 5,
                    totalCat2NotApplicable = 6,
                    totalCat2NotAFinding = 7,
                    totalCat2NotReviewed = 8,
                    totalCat3Open = 9,
                    totalCat3NotApplicable = 10,
                    totalCat3NotAFinding = 11,
                    totalCat3NotReviewed = 12
                },
                new Score
                {
                    totalCat1Open = 10,
                    totalCat1NotApplicable = 20,
                    totalCat1NotAFinding = 30,
                    totalCat1NotReviewed = 40,
                    totalCat2Open = 50,
                    totalCat2NotApplicable = 60,
                    totalCat2NotAFinding = 70,
                    totalCat2NotReviewed = 80,
                    totalCat3Open = 90,
                    totalCat3NotApplicable = 100,
                    totalCat3NotAFinding = 110,
                    totalCat3NotReviewed = 120
                }
            };

            _mockScoreRepo.Setup(x => x.GetScoresbySystem("sys-2")).ReturnsAsync(scores);
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScoreBySystem("sys-2");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var total = Assert.IsType<Score>(okResult.Value);
            Assert.Equal("sys-2", total.systemGroupId);
            Assert.Equal("all", total.hostName);
            Assert.Equal(11, total.totalCat1Open);
            Assert.Equal(132, total.totalCat3NotReviewed);
            Assert.NotEqual(0, total.totalOpen);
            _mockScoreRepo.Verify(x => x.GetScoresbySystem("sys-2"), Times.Once);
        }

        [Fact]
        public async Task GetScoreBySystem_ReturnsBadRequest_WhenRepositoryThrows()
        {
            _mockScoreRepo.Setup(x => x.GetScoresbySystem("sys-error")).ThrowsAsync(new Exception("failure"));
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = await controller.GetScoreBySystem("sys-error");

            Assert.IsType<BadRequestResult>(result);
            _mockScoreRepo.Verify(x => x.GetScoresbySystem("sys-error"), Times.Once);
        }

        [Fact]
        public void Score_ReturnsOk_WhenRawChecklistIsValid()
        {
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = controller.Score(ValidChecklistXml());

            var okResult = Assert.IsType<OkObjectResult>(result);
            var score = Assert.IsType<Score>(okResult.Value);
            Assert.Equal("host-score", score.hostName);
            Assert.NotEqual(0, score.totalOpen);
        }

        [Fact]
        public void Score_ReturnsBadRequest_WhenRawChecklistIsInvalidXml()
        {
            var controller = new ScoreController(_mockScoreRepo.Object, _mockLogger.Object);

            var result = controller.Score("<CHECKLIST><broken>");

            Assert.IsType<BadRequestResult>(result);
        }

        private static string ValidChecklistXml()
        {
            return "<CHECKLIST>" +
                   "<ASSET><HOST_NAME>host-score</HOST_NAME></ASSET>" +
                   "<STIGS><iSTIG>" +
                   "<STIG_INFO>" +
                   "<SI_DATA><SID_NAME>title</SID_NAME><SID_DATA>Windows Server Security Technical Implementation Guide</SID_DATA></SI_DATA>" +
                   "<SI_DATA><SID_NAME>releaseinfo</SID_NAME><SID_DATA>Release: 1 Benchmark Date:2024-01-01</SID_DATA></SI_DATA>" +
                   "</STIG_INFO>" +
                   "<VULN>" +
                   "<STIG_DATA><VULN_ATTRIBUTE>Severity</VULN_ATTRIBUTE><ATTRIBUTE_DATA>high</ATTRIBUTE_DATA></STIG_DATA>" +
                   "<STATUS>open</STATUS>" +
                   "</VULN>" +
                   "</iSTIG></STIGS>" +
                   "</CHECKLIST>";
        }
    }
}
