
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using openstig_scoring_api.Classes;
using openstig_scoring_api.Models;
using System.IO;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace openstig_scoring_api.Controllers
{
    [Route("api/[controller]")]
    public class ScoreController : Controller
    {
	    private readonly IDistributedCache  _cache;
 
		// _distributedCache.GetString(cacheKey);
		// _distributedCache.SetString(cacheKey, existingTime);
        private readonly ILogger<ScoreController> _logger;
        const string exampleSTIG = "/examples/asd-example.ckl";

        public ScoreController(IDistributedCache cache, ILogger<ScoreController> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        // GET api/values
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByGuid(Guid id)
        {
            Score cklScore = new Score();
            string checklist = await _cache.GetStringAsync(id.ToString());
            if (!string.IsNullOrEmpty(checklist)) {
                _logger.LogInformation("/score/{id}: checklist is valid so putting into class to run queries.");
                Artifact asdSTIGChecklist = JsonConvert.DeserializeObject<Artifact>(checklist);
                if (asdSTIGChecklist.Checklist == null || asdSTIGChecklist.Checklist.Items == null){
                    // load the checklist
                    asdSTIGChecklist.Checklist = ChecklistLoader.LoadASDChecklist(Directory.GetCurrentDirectory() + 
                        "/wwwroot/data" + asdSTIGChecklist.filePath);
                        // save it to the cache for next time           
                    _logger.LogInformation("/score/{id}: Pulling in latest checklist file.");
                    _cache.SetString(asdSTIGChecklist.id.ToString(),JsonConvert.SerializeObject(asdSTIGChecklist));
                }
                if (asdSTIGChecklist != null && asdSTIGChecklist.Checklist.Items != null && 
                    asdSTIGChecklist.Checklist.Items.Length == 2 && asdSTIGChecklist.Checklist.Items[1] != null) {
                    _logger.LogInformation("/score/{id}: Scoring the checklist.");

                    // now see what score you can get
                    CHECKLISTSTIGS objSTIG = (CHECKLISTSTIGS)asdSTIGChecklist.Checklist.Items[1];
                    CHECKLISTSTIGSISTIG[] iSTIG = objSTIG.iSTIG;
                    if (iSTIG.Length == 1 && iSTIG[0] != null){
                        CHECKLISTSTIGSISTIG asdSTIG = (CHECKLISTSTIGSISTIG)iSTIG[0];
                        if (asdSTIG.VULN != null && asdSTIG.VULN.Length > 0){
                            CHECKLISTSTIGSISTIGVULN[] asdVulnerabilities = asdSTIG.VULN;
                            cklScore.NotReviewed = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_reviewed").Count();
                            cklScore.NotApplicable = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_applicable").Count();
                            cklScore.Open = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "open").Count();
                            cklScore.NotAFinding = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "notafinding").Count();
                        }
                    }
                }
            }
            return Json(cklScore);
        }
        
        // GET api/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const string exampleSTIG = "/example/asd-full-example.ckl";
            Score cklScore = new Score();

            string filename = Directory.GetCurrentDirectory() + exampleSTIG;
            string checklistXML = string.Empty;
            string returnedXML = string.Empty;
            
            if (System.IO.File.Exists(filename)) {
                _logger.LogInformation("/score/: using generic checklist to run queries.");
                CHECKLIST asdChecklist = new CHECKLIST();
                _logger.LogInformation("/example/: Example file active so returning an example ASD STIG.");

                // put that into a class and deserialize that
                asdChecklist = ChecklistLoader.LoadASDChecklist(filename);
                if (asdChecklist != null && asdChecklist.Items != null && 
                    asdChecklist.Items.Length == 2 && asdChecklist.Items[1] != null) {
                    _logger.LogInformation("/score/: Scoring the example checklist.");

                    // now see what score you can get
                    CHECKLISTSTIGS objSTIG = (CHECKLISTSTIGS)asdChecklist.Items[1];
                    CHECKLISTSTIGSISTIG[] iSTIG = objSTIG.iSTIG;
                    if (iSTIG.Length == 1 && iSTIG[0] != null){
                        CHECKLISTSTIGSISTIG asdSTIG = (CHECKLISTSTIGSISTIG)iSTIG[0];
                        if (asdSTIG.VULN != null && asdSTIG.VULN.Length > 0){
                            CHECKLISTSTIGSISTIGVULN[] asdVulnerabilities = asdSTIG.VULN;
                            cklScore.NotReviewed = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_reviewed").Count();
                            cklScore.NotApplicable = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_applicable").Count();
                            cklScore.Open = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "open").Count();
                            cklScore.NotAFinding = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "notafinding").Count();
                        }
                    }
                }
            }
            return Json(cklScore);
        }
    }
}
