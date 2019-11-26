
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using openrmf_scoring_api.Classes;
using openrmf_scoring_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using openrmf_scoring_api.Data;

namespace openrmf_scoring_api.Controllers
{
    [Route("/")]
    public class ScoreController : Controller
    {
	    private readonly IScoreRepository _scoreRepo;
       private readonly ILogger<ScoreController> _logger;

        public ScoreController(IScoreRepository scoreRepo, ILogger<ScoreController> logger)
        {
            _logger = logger;
            _scoreRepo = scoreRepo;
        }

        // GET the listing with Ids of all the scores for the checklists
        [HttpGet]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> ListScores()
        {
            try {
                IEnumerable<Score> scores;
                scores = await _scoreRepo.GetAllScores();
                return Ok(scores);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error listing all scores. Check the database!");
                return BadRequest();
            }
        }

        // GET /value
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> GetScore(string id)
        {
            try {
                Score score = new Score();
                score = await _scoreRepo.GetScore(id);
                return Ok(score);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error Retrieving Score for id {0}", id);
                return NotFound();
            }
        }
        
        // GET /artifact/value
        [HttpGet("artifact/{id}")]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> GetScoreByArtifact(string id)
        {
            try {
                Score score = new Score();
                score = await _scoreRepo.GetScorebyArtifact(id);
                return Ok(score);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error Retrieving Score for artifactId {0}", id);
                return NotFound();
            }
        }

        // GET scores by the system name as a whole
        // create one return record for all the checklists in there
        [HttpGet("system/{systemGroupId}")]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> GetScoreBySystem(string systemGroupId)
        {
            try {
                IEnumerable<Score> scores;
                scores = await _scoreRepo.GetScoresbySystem(systemGroupId);
                // cycle through the list and return back only a single score
                Score totalScore = new Score();
                totalScore.systemGroupId = systemGroupId;
                totalScore.stigType = "all";
                totalScore.stigRelease = "all";
                totalScore.hostName = "all";
                // cycle through all, add each of the type to the previous value (starts with 0)
                foreach(Score s in scores) {
                    // make it add up the scores into the correct fields
                    totalScore.totalCat1Open += s.totalCat1Open;
                    totalScore.totalCat1NotApplicable += s.totalCat1NotApplicable;
                    totalScore.totalCat1NotAFinding += s.totalCat1NotAFinding;
                    totalScore.totalCat1NotReviewed += s.totalCat1NotReviewed;
                    totalScore.totalCat2Open += s.totalCat2Open;
                    totalScore.totalCat2NotApplicable += s.totalCat2NotApplicable;
                    totalScore.totalCat2NotAFinding += s.totalCat2NotAFinding;
                    totalScore.totalCat2NotReviewed += s.totalCat2NotReviewed;
                    totalScore.totalCat3Open += s.totalCat3Open;
                    totalScore.totalCat3NotApplicable += s.totalCat3NotApplicable;
                    totalScore.totalCat3NotAFinding += s.totalCat3NotAFinding;
                    totalScore.totalCat3NotReviewed += s.totalCat3NotReviewed;
                }
                // send back the summary scores of everything in the system
                return Ok(totalScore);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error Retrieving Scores for system {0}", systemGroupId);
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public IActionResult Score (string rawChecklist){
            try {
                return Ok(ScoringEngine.ScoreChecklistString(rawChecklist));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creating Score for XML string passed in");
                return BadRequest();
            }
        }
    }
}
