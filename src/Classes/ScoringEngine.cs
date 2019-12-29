// Copyright (c) Cingulara LLC 2019 and Tutela LLC 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
using System;
using openrmf_scoring_api.Models;
using System.Linq;

namespace openrmf_scoring_api.Classes
{
    public static class ScoringEngine 
    {
        public static Score ScoreChecklistString(string rawChecklist) {
          var score = ScoreChecklist(ChecklistLoader.LoadChecklist(rawChecklist));
          return score;
        }
        public static Score ScoreChecklist (CHECKLIST xml)
        {
            try {
                Score score = new Score();
                // CAT 1
                score.totalCat1NotReviewed = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "not_reviewed" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "high").FirstOrDefault() != null).Count();
                score.totalCat1NotApplicable = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "not_applicable" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "high").FirstOrDefault() != null).Count();
                score.totalCat1Open = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "open" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "high").FirstOrDefault() != null).Count();
                score.totalCat1NotAFinding = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "notafinding" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "high").FirstOrDefault() != null).Count();
                // CAT 2
                score.totalCat2NotReviewed = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "not_reviewed" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "medium").FirstOrDefault() != null).Count();
                score.totalCat2NotApplicable = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "not_applicable" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "medium").FirstOrDefault() != null).Count();
                score.totalCat2Open = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "open" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "medium").FirstOrDefault() != null).Count();
                score.totalCat2NotAFinding = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "notafinding" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "medium").FirstOrDefault() != null).Count();
                // CAT 3
                score.totalCat3NotReviewed = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "not_reviewed" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "low").FirstOrDefault() != null).Count();
                score.totalCat3NotApplicable = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "not_applicable" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "low").FirstOrDefault() != null).Count();
                score.totalCat3Open = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "open" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "low").FirstOrDefault() != null).Count();
                score.totalCat3NotAFinding = xml.STIGS.iSTIG.VULN.Where(x => x.STATUS.ToLower() == "notafinding" && 
                        x.STIG_DATA.Where(y => y.VULN_ATTRIBUTE == "Severity" && 
                                               y.ATTRIBUTE_DATA == "low").FirstOrDefault() != null).Count();

                // get the title and release which is a list of children of child nodes buried deeper :face-palm-emoji:
                score.stigRelease = xml.STIGS.iSTIG.STIG_INFO.SI_DATA.Where(x => x.SID_NAME.ToLower() == "releaseinfo").FirstOrDefault().SID_DATA;
                score.stigType = xml.STIGS.iSTIG.STIG_INFO.SI_DATA.Where(x => x.SID_NAME.ToLower() == "title").FirstOrDefault().SID_DATA;

                // shorten the names a bit
                if (score != null && !string.IsNullOrEmpty(score.stigType)){
                    score.stigType = score.stigType.Replace("Security Technical Implementation Guide", "STIG");
                    score.stigType = score.stigType.Replace("Windows", "WIN");
                    score.stigType = score.stigType.Replace("Application Security and Development", "ASD");
                    score.stigType = score.stigType.Replace("Microsoft Internet Explorer", "MSIE");
                    score.stigType = score.stigType.Replace("Red Hat Enterprise Linux", "REL");
                    score.stigType = score.stigType.Replace("MS SQL Server", "MSSQL");
                    score.stigType = score.stigType.Replace("Server", "SVR");
                    score.stigType = score.stigType.Replace("Workstation", "WRK");
                }
                if (score != null && !string.IsNullOrEmpty(score.stigRelease)) {
                    score.stigRelease = score.stigRelease.Replace("Release: ", "R"); // i.e. R11, R2 for the release number
                    score.stigRelease = score.stigRelease.Replace("Benchmark Date:","dated");
                }
                return score;
            }
            catch (Exception ex) {
                Console.WriteLine("Oops! The Scoring Engine had a major problem..." + ex.Message);
                return new Score();
            }
        }
    }
}