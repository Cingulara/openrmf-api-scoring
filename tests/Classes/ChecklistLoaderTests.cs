using System.Xml;
using openrmf_scoring_api.Classes;
using Xunit;

namespace tests.Classes
{
    public class ChecklistLoaderTests
    {
        [Fact]
        public void LoadChecklist_ParsesChecklistSections_WhenXmlIsValid()
        {
            var checklist = ChecklistLoader.LoadChecklist(ValidChecklistXml());

            Assert.NotNull(checklist);
            Assert.Equal("role-a", checklist.ASSET.ROLE);
            Assert.Equal("host-loader", checklist.ASSET.HOST_NAME);
            Assert.Equal("title", checklist.STIGS.iSTIG.STIG_INFO.SI_DATA[0].SID_NAME);
            Assert.Equal(2, checklist.STIGS.iSTIG.VULN.Count);
            Assert.NotEqual("host-x", checklist.ASSET.HOST_NAME);
        }

        [Fact]
        public void LoadChecklist_Throws_WhenXmlIsMalformed()
        {
            Assert.Throws<XmlException>(() => ChecklistLoader.LoadChecklist("<CHECKLIST><ASSET></CHECKLIST>"));
        }

        private static string ValidChecklistXml()
        {
            return "<CHECKLIST>" +
                   "<ASSET>" +
                   "<ROLE>role-a</ROLE>" +
                   "<HOST_NAME>host-loader</HOST_NAME>" +
                   "</ASSET>" +
                   "<STIGS><iSTIG>" +
                   "<STIG_INFO>" +
                   "<SI_DATA><SID_NAME>title</SID_NAME><SID_DATA>Windows Server Security Technical Implementation Guide</SID_DATA></SI_DATA>" +
                   "<SI_DATA><SID_NAME>releaseinfo</SID_NAME><SID_DATA>Release: 7 Benchmark Date:2024-02-02</SID_DATA></SI_DATA>" +
                   "</STIG_INFO>" +
                   "<VULN>" +
                   "<STIG_DATA><VULN_ATTRIBUTE>Severity</VULN_ATTRIBUTE><ATTRIBUTE_DATA>high</ATTRIBUTE_DATA></STIG_DATA>" +
                   "<STATUS>open</STATUS>" +
                   "<COMMENTS>comment-1</COMMENTS>" +
                   "</VULN>" +
                   "<VULN>" +
                   "<STIG_DATA><VULN_ATTRIBUTE>Severity</VULN_ATTRIBUTE><ATTRIBUTE_DATA>medium</ATTRIBUTE_DATA></STIG_DATA>" +
                   "<STATUS>not_reviewed</STATUS>" +
                   "<COMMENTS>comment-2</COMMENTS>" +
                   "</VULN>" +
                   "</iSTIG></STIGS>" +
                   "</CHECKLIST>";
        }
    }
}
