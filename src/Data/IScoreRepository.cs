using openrmf_scoring_api.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace openrmf_scoring_api.Data {
    public interface IScoreRepository
    {
        Task<IEnumerable<Score>> GetAllScores();
        Task<Score> GetScore(string id);

        // get the score by the artifact Id, not the Score Id
        Task<Score> GetScorebyArtifact(string artifactId);

        // get the score by the system as a whole
        Task<IEnumerable<Score>> GetScoresbySystem(string systemName);  

        // query after multiple parameters
        Task<IEnumerable<Score>> GetScore(string bodyText, DateTime updatedFrom, long headerSizeLimit);

    }
}