using System;
using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class ArtifactTests
    {
        [Fact]
        public void Constructor_InitializesRequiredDefaults()
        {
            var artifact = new Artifact();

            Assert.NotNull(artifact);
            Assert.NotEqual(Guid.Empty, artifact.id);
            Assert.NotNull(artifact.CHECKLIST);
        }

        [Fact]
        public void Title_ComposesFromTrimmedValues()
        {
            var artifact = new Artifact
            {
                created = DateTime.UtcNow,
                hostName = " hostA ",
                stigType = " Win11 ",
                stigRelease = " R1 ",
                version = "2",
                updatedOn = DateTime.UtcNow
            };

            Assert.Equal("hostA-Win11-V2-R1", artifact.title);
            Assert.NotEqual("hostA-Win10-V2-R1", artifact.title);
            Assert.True(artifact.updatedOn.HasValue);
        }

        [Fact]
        public void Title_Throws_WhenCriticalPartsMissing()
        {
            var artifact = new Artifact
            {
                hostName = null,
                stigType = "type",
                stigRelease = "R1",
                version = "1"
            };

            Assert.Throws<NullReferenceException>(() => _ = artifact.title);
        }
    }
}
