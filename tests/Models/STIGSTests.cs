using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Models
{
    public class STIGSTests
    {
        [Fact]
        public void Constructor_InitializesiSTIG()
        {
            var data = new STIGS();

            Assert.NotNull(data);
            Assert.NotNull(data.iSTIG);
        }

        [Fact]
        public void iSTIG_CanBeAssigned()
        {
            var custom = new iSTIG();
            var data = new STIGS { iSTIG = custom };

            Assert.Same(custom, data.iSTIG);
            Assert.NotNull(data.iSTIG.STIG_INFO);
        }
    }
}
