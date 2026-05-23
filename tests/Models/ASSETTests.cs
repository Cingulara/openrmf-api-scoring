using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class ASSETTests
    {
        [Fact]
        public void Constructor_InitializesWithNullProperties()
        {
            var asset = new ASSET();

            Assert.NotNull(asset);
            Assert.Null(asset.ROLE);
            Assert.Null(asset.HOST_NAME);
        }

        [Fact]
        public void Properties_RoundTripAssignedValues()
        {
            var asset = new ASSET
            {
                ROLE = "server",
                ASSET_TYPE = "vm",
                HOST_NAME = "alpha",
                HOST_IP = "10.0.0.1",
                HOST_MAC = "00-11-22-33-44-55",
                HOST_FQDN = "alpha.example.local",
                TECH_AREA = "network",
                TARGET_KEY = "target-key",
                WEB_OR_DATABASE = "database",
                WEB_DB_SITE = "site-a",
                WEB_DB_INSTANCE = "instance-a"
            };

            Assert.Equal("server", asset.ROLE);
            Assert.Equal("alpha", asset.HOST_NAME);
            Assert.NotEqual("beta", asset.HOST_NAME);
        }
    }
}
