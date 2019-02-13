using openstig_scoring_api.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace openstig_scoring_api.Data {
    public interface IScoreRepository
    {
        Task<IEnumerable<Score>> GetAllScores();
        Task<Score> GetScore(string id);

        Task<Score> GetScorebyArtifact(string artifactId);

        // query after multiple parameters
        Task<IEnumerable<Score>> GetScore(string bodyText, DateTime updatedFrom, long headerSizeLimit);

    }
}