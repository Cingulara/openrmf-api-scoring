// Copyright (c) Cingulara LLC 2025 and Tutela LLC 2025. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
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
        Task<IEnumerable<Score>> GetScoresbySystem(string systemGroupId);  

        // query after multiple parameters
        Task<IEnumerable<Score>> GetScore(string bodyText, DateTime updatedFrom, long headerSizeLimit);

        bool HealthStatus();
    }
}