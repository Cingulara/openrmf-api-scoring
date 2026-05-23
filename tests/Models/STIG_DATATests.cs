using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class STIG_DATATests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            var data = new STIG_DATA();

            Assert.NotNull(data);
            Assert.Null(data.VULN_ATTRIBUTE);
            Assert.Null(data.ATTRIBUTE_DATA);
        }

        [Fact]
        public void Properties_RoundTripValues()
        {
            var data = new STIG_DATA
            {
                VULN_ATTRIBUTE = "Severity",
                ATTRIBUTE_DATA = "high"
            };

            Assert.Equal("Severity", data.VULN_ATTRIBUTE);
            Assert.Equal("high", data.ATTRIBUTE_DATA);
            Assert.NotEqual("low", data.ATTRIBUTE_DATA);
        }
    }
}
