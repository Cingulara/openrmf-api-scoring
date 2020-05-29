// Copyright (c) Cingulara LLC 2019 and Tutela LLC 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
using openrmf_scoring_api.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace openrmf_scoring_api.Data {
    public class ScoreRepository : IScoreRepository
    {
        private readonly ScoreContext _context = null;

        public ScoreRepository(IOptions<Settings> settings)
        {
            _context = new ScoreContext(settings);
        }

        public async Task<IEnumerable<Score>> GetAllScores()
        {
            try
            {
                return await _context.Scores
                        .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after Id or InternalId (BSonId value)
        public async Task<Score> GetScore(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Scores.Find(Score => Score.InternalId == GetInternalId(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after artifactId
        public async Task<Score> GetScorebyArtifact(string artifactId)
        {
            try
            {
                return await _context.Scores.Find(Score => Score.artifactId == GetInternalId(artifactId)).FirstOrDefaultAsync();
             }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // get all scores by system name
        public async Task<IEnumerable<Score>> GetScoresbySystem(string systemGroupId)
        {
            try
            {
                var query = _context.Scores.Find(Score => Score.systemGroupId == systemGroupId);
                return await query.ToListAsync();
             }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after body text, updated time, and header image size
        //
        public async Task<IEnumerable<Score>> GetScore(string bodyText, DateTime updatedFrom, long headerSizeLimit)
        {
            try
            {
                var query = _context.Scores.Find(Score => Score.title.Contains(bodyText) &&
                                    Score.updatedOn >= updatedFrom);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        // check that the database is responding and it returns at least one collection name
        public bool HealthStatus(){
            var result = _context.Scores.Database.ListCollectionNamesAsync().GetAwaiter().GetResult().FirstOrDefault();
            if (!string.IsNullOrEmpty(result)) // we are good to go
                return true;
            return false;
        }
    }
}