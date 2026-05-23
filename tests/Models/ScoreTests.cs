using System;
using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class ScoreTests
    {
        [Fact]
        public void Constructor_InitializesInstance()
        {
            var score = new Score();

            Assert.NotNull(score);
            Assert.Equal(0, score.totalOpen);
            Assert.Equal("Unknown--", score.title);
        }

        [Fact]
        public void Title_UsesTrimmedValues_AndUnknownHostFallback()
        {
            var score = new Score
            {
                hostName = " host-a ",
                stigType = " type-a ",
                stigRelease = " release-a "
            };

            Assert.Equal("host-a-type-a-release-a", score.title);
            Assert.NotEqual("Unknown-type-a-release-a", score.title);

            score.hostName = null;
            Assert.Equal("Unknown-type-a-release-a", score.title);
        }

        [Fact]
        public void Totals_AggregateCategoryValues()
        {
            var score = new Score
            {
                systemGroupId = "group-1",
                hostName = "host-1",
                stigRelease = "R10",
                stigType = "Google Chrome",
                created = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
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
            };

            Assert.Equal(15, score.totalOpen);
            Assert.Equal(18, score.totalNotApplicable);
            Assert.Equal(21, score.totalNotAFinding);
            Assert.Equal(24, score.totalNotReviewed);
            Assert.Equal(10, score.totalCat1);
            Assert.Equal(26, score.totalCat2);
            Assert.Equal(42, score.totalCat3);

            Assert.NotEqual(14, score.totalOpen);
            Assert.NotEqual(41, score.totalCat3);
        }
    }
}
