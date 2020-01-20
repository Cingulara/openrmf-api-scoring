// Copyright (c) Cingulara LLC 2019 and Tutela LLC 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// GET score by the checklist/artifact Id (Old call)
        /// </summary>
        /// <param name="id">The system ID for the checklists</param>
        /// <returns>
        /// HTTP Status showing it was generated and the score record showing the categories and status numbers.
        /// </returns>
        /// <response code="200">Returns the score generated for the checklist data passed in</response>
        /// <response code="400">If the item did not generate correctly, or if the CKL data was invalid</response>
        /// <response code="404">If the artifact ID was invalid</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> GetScore(string id)
        {
            try {
                _logger.LogInformation("Calling GetScore({0})", id);
                Score score = new Score();
                score = await _scoreRepo.GetScore(id);
                if (score == null) {                    
                    _logger.LogWarning("Calling GetScore({0}) returned an invalid Scoring record", id);
                    return NotFound();
                }
                _logger.LogInformation("Called GetScore({0}) successfully", id);
                return Ok(score);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "GetScore() Error Retrieving Score for id {0}", id);
                return BadRequest();
            }
        }

        /// <summary>
        /// GET score by the checklist/artifact Id
        /// </summary>
        /// <param name="id">The system ID for the checklists</param>
        /// <returns>
        /// HTTP Status showing it was generated and the score record showing the categories and status numbers.
        /// </returns>
        /// <response code="200">Returns the score generated for the checklist data passed in</response>
        /// <response code="400">If the item did not generate correctly, or if the CKL data was invalid</response>
        /// <response code="404">If the artifact ID was invalid</response>
        [HttpGet("artifact/{id}")]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> GetScoreByArtifact(string id)
        {
            try {
                _logger.LogInformation("Calling GetScoreByArtifact({0})", id);
                Score score = new Score();
                score = await _scoreRepo.GetScorebyArtifact(id);
                if (score == null) {    
                    _logger.LogWarning("Calling GetScoreByArtifact({0}) returned an invalid Scoring record", id);
                    return NotFound();
                }
                _logger.LogInformation("Called GetScoreByArtifact({0}) successfully", id);
                return Ok(score);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "GetScoreByArtifact() Error Retrieving Score for artifactId {0}", id);
                return BadRequest();
            }
        }

        /// <summary>
        /// GET scores by the system name as a whole. Create a single Score record for all checklists.
        /// </summary>
        /// <param name="systemGroupId">The system ID for the checklists</param>
        /// <returns>
        /// HTTP Status showing it was generated and the score record showing the categories and status numbers.
        /// </returns>
        /// <response code="200">Returns the score generated for the checklist data passed in</response>
        /// <response code="400">If the item did not generate correctly, or if the CKL data was invalid</response>
        /// <response code="404">If the system ID was invalid</response>
        [HttpGet("system/{systemGroupId}")]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public async Task<IActionResult> GetScoreBySystem(string systemGroupId)
        {
            try {
                _logger.LogInformation("Calling GetScoreBySystem({0})", systemGroupId);
                IEnumerable<Score> scores;
                scores = await _scoreRepo.GetScoresbySystem(systemGroupId);
                if (scores == null) {
                    _logger.LogWarning("Called GetScoreBySystem({0}) but it returned 0 scoring records", systemGroupId);
                    return NotFound();
                }
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
                _logger.LogInformation("Called GetScoreBySystem({0}) successfully", systemGroupId);
                return Ok(totalScore);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "GetScoreBySystem() Error Retrieving Scores for system {0}", systemGroupId);
                return BadRequest();
            }
        }

        /// <summary>
        /// POST Called from the OpenRMF UI (or external access) to generate the score of a checklist for the 
        /// category 1, 2, 3 items based on status. This is called from the Template page OR called from any 
        /// checklist listing where the score is currently empty for some reason.
        /// </summary>
        /// <param name="rawChecklist">The actual CKL file text to parse</param>
        /// <returns>
        /// HTTP Status showing it was generated and the score record showing the categories and status numbers.
        /// </returns>
        /// <response code="200">Returns the score generated for the checklist data passed in</response>
        /// <response code="400">If the item did not generate correctly, or if the CKL data was invalid</response>
        [HttpPost]
        [Authorize(Roles = "Administrator,Reader,Editor,Assessor")]
        public IActionResult Score (string rawChecklist){
            try {
                _logger.LogInformation("Calling Score() with a raw Checklist XML data");
                return Ok(ScoringEngine.ScoreChecklistString(rawChecklist));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Score() Error creating Score for XML string passed in");
                return BadRequest();
            }
        }
    }
}
