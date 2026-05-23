using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class SI_DATATests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            var data = new SI_DATA();

            Assert.NotNull(data);
            Assert.Null(data.SID_NAME);
            Assert.Null(data.SID_DATA);
        }

        [Fact]
        public void Properties_RoundTripValues()
        {
            var data = new SI_DATA
            {
                SID_DATA = "value",
                SID_NAME = "name"
            };

            Assert.Equal("value", data.SID_DATA);
            Assert.Equal("name", data.SID_NAME);
            Assert.NotEqual("other", data.SID_DATA);
        }
    }
}
