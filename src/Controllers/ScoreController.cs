
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
