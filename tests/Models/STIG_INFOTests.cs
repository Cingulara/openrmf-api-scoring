using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class STIG_INFOTests
    {
        [Fact]
        public void Constructor_InitializesSIDataList()
        {
            var data = new STIG_INFO();

            Assert.NotNull(data);
            Assert.NotNull(data.SI_DATA);
            Assert.Empty(data.SI_DATA);
        }

        [Fact]
        public void SI_DATA_AcceptsEntries()
        {
            var data = new STIG_INFO();
            data.SI_DATA.Add(new SI_DATA { SID_NAME = "title", SID_DATA = "Windows STIG" });

            Assert.Single(data.SI_DATA);
            Assert.Equal("title", data.SI_DATA[0].SID_NAME);
            Assert.NotEqual("releaseinfo", data.SI_DATA[0].SID_NAME);
        }
    }
}
