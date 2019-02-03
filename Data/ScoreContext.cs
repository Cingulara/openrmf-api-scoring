using MongoDB.Driver;
using openstig_scoring_api.Models;
using Microsoft.Extensions.Options;

namespace openstig_scoring_api.Data
{
    public class ScoreContext
    {
        private readonly IMongoDatabase _database = null;

        public ScoreContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Score> Scores
        {
            get
            {
                return _database.GetCollection<Score>("Scores");
            }
        }
    }
}