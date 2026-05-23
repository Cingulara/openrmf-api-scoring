using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class SettingsTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            var settings = new Settings();

            Assert.NotNull(settings);
            Assert.Null(settings.ConnectionString);
            Assert.Null(settings.Database);
        }

        [Fact]
        public void Fields_RoundTripValues()
        {
            var settings = new Settings
            {
                ConnectionString = "mongodb://localhost",
                Database = "OpenRMF"
            };

            Assert.Equal("mongodb://localhost", settings.ConnectionString);
            Assert.Equal("OpenRMF", settings.Database);
            Assert.NotEqual("postgres://localhost", settings.ConnectionString);
        }
    }
}
