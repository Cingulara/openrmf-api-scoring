using openrmf_scoring_api.Classes;
using openrmf_scoring_api.Models;
using Xunit;

namespace tests.Classes
{
    public class ScoringEngineTests
    {
        [Fact]
        public void ScoreChecklist_ComputesCountsAndMetadata_FromChecklistObject()
        {
            var checklist = new CHECKLIST
            {
                ASSET = new ASSET
                {
                    HOST_NAME = "host-engine",
                    HOST_FQDN = "host-engine.domain.local"
                },
                STIGS = new STIGS
                {
                    iSTIG = new iSTIG
                    {
                        STIG_INFO = new STIG_INFO
                        {
                            SI_DATA =
                            [
                                new SI_DATA
                                {
                                    SID_NAME = "title",
                                    SID_DATA = "Windows Server Security Technical Implementation Guide"
                                },
                                new SI_DATA
                                {
                                    SID_NAME = "releaseinfo",
                                    SID_DATA = "Release: 2 Benchmark Date:2024-01-01"
                                }
                            ]
                        }
                    }
                }
            };

            checklist.STIGS.iSTIG.VULN.Add(new VULN
            {
                STATUS = "open",
                STIG_DATA = [new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "high" }]
            });
            checklist.STIGS.iSTIG.VULN.Add(new VULN
            {
                STATUS = "not_reviewed",
                STIG_DATA = [new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "medium" }]
            });
            checklist.STIGS.iSTIG.VULN.Add(new VULN
            {
                STATUS = "not_applicable",
                SEVERITY_OVERRIDE = "low",
                STIG_DATA = [new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "high" }]
            });
            checklist.STIGS.iSTIG.VULN.Add(new VULN
            {
                STATUS = "notafinding",
                SEVERITY_OVERRIDE = "high",
                STIG_DATA = [new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "low" }]
            });

            var score = ScoringEngine.ScoreChecklist(checklist);

            Assert.Equal("host-engine", score.hostName);
            Assert.Equal(1, score.totalCat1Open);
            Assert.Equal(1, score.totalCat2NotReviewed);
            Assert.Equal(1, score.totalCat3NotApplicable);
            Assert.Equal(1, score.totalCat1NotAFinding);
            Assert.Equal(4, score.totalOpen + score.totalNotApplicable + score.totalNotAFinding + score.totalNotReviewed);
            Assert.Contains("WIN", score.stigType);
            Assert.Contains("SVR", score.stigType);
            Assert.Contains("STIG", score.stigType);
            Assert.StartsWith("R2", score.stigRelease);
            Assert.DoesNotContain("Release: ", score.stigRelease);
        }

        [Fact]
        public void ScoreChecklist_UsesHostFqdn_WhenHostNameIsMissing()
        {
            var checklist = new CHECKLIST
            {
                ASSET = new ASSET { HOST_FQDN = "fqdn-only.local" },
                STIGS = new STIGS
                {
                    iSTIG = new iSTIG
                    {
                        STIG_INFO = new STIG_INFO
                        {
                            SI_DATA =
                            [
                                new SI_DATA { SID_NAME = "title", SID_DATA = "Windows Security Technical Implementation Guide" },
                                new SI_DATA { SID_NAME = "releaseinfo", SID_DATA = "Release: 1 Benchmark Date:2024-03-03" }
                            ]
                        },
                        VULN =
                        [
                            new VULN
                            {
                                STATUS = "open",
                                STIG_DATA = [new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "high" }]
                            }
                        ]
                    }
                }
            };

            var score = ScoringEngine.ScoreChecklist(checklist);

            Assert.Equal("fqdn-only.local", score.hostName);
            Assert.NotEqual("Unknown", score.hostName);
        }

        [Fact]
        public void ScoreChecklist_ReturnsDefaultScore_WhenMetadataIsMissing()
        {
            var checklist = new CHECKLIST
            {
                ASSET = new ASSET { HOST_NAME = "host-a" },
                STIGS = new STIGS
                {
                    iSTIG = new iSTIG
                    {
                        STIG_INFO = new STIG_INFO(),
                        VULN =
                        [
                            new VULN
                            {
                                STATUS = "open",
                                STIG_DATA = [new STIG_DATA { VULN_ATTRIBUTE = "Severity", ATTRIBUTE_DATA = "high" }]
                            }
                        ]
                    }
                }
            };

            var score = ScoringEngine.ScoreChecklist(checklist);

            Assert.NotNull(score);
            Assert.Equal(0, score.totalOpen);
            Assert.Equal("Unknown--", score.title);
            Assert.NotEqual("host-a", score.hostName);
        }
    }
}
