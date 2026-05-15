using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class iSTIGTests
    {
        [Fact]
        public void Constructor_InitializesNestedObjects()
        {
            var iStig = new iSTIG();

            Assert.NotNull(iStig);
            Assert.NotNull(iStig.STIG_INFO);
            Assert.NotNull(iStig.VULN);
            Assert.Empty(iStig.VULN);
        }

        [Fact]
        public void VULN_AcceptsEntries()
        {
            var iStig = new iSTIG();
            iStig.VULN.Add(new VULN { STATUS = "open" });

            Assert.Single(iStig.VULN);
            Assert.Equal("open", iStig.VULN[0].STATUS);
            Assert.NotEqual("not_reviewed", iStig.VULN[0].STATUS);
        }
    }
}
