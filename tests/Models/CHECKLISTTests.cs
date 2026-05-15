using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class CHECKLISTTests
    {
        [Fact]
        public void Constructor_InitializesNestedObjects()
        {
            var checklist = new CHECKLIST();

            Assert.NotNull(checklist);
            Assert.NotNull(checklist.ASSET);
            Assert.NotNull(checklist.STIGS);
        }

        [Fact]
        public void Properties_CanBeReassigned()
        {
            var checklist = new CHECKLIST
            {
                ASSET = new ASSET { HOST_NAME = "host-one" },
                STIGS = new STIGS()
            };

            Assert.Equal("host-one", checklist.ASSET.HOST_NAME);
            Assert.NotEqual("host-two", checklist.ASSET.HOST_NAME);
        }
    }
}
