using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class VULNTests
    {
        [Fact]
        public void Constructor_InitializesStigDataList()
        {
            var vuln = new VULN();

            Assert.NotNull(vuln);
            Assert.NotNull(vuln.STIG_DATA);
            Assert.Empty(vuln.STIG_DATA);
        }

        [Fact]
        public void Properties_RoundTripValues()
        {
            var vuln = new VULN
            {
                STATUS = "open",
                FINDING_DETAILS = "details",
                COMMENTS = "comment",
                SEVERITY_OVERRIDE = "high",
                SEVERITY_JUSTIFICATION = "approved"
            };

            vuln.STIG_DATA.Add(new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "medium" });

            Assert.Equal("open", vuln.STATUS);
            Assert.Equal("details", vuln.FINDING_DETAILS);
            Assert.Single(vuln.STIG_DATA);
            Assert.NotEqual("closed", vuln.STATUS);
        }
    }
}
