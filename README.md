# openstig-api-scoring
This is the openSTIG Scoring API for scoring a checklist. This piece of the openSTIG tool allows reading of the scoring database that is updated with
eventual consistency using the openstig-msg-score project that reads newer/updated XML and scores by category and status for each vulnerability in the 
appropriate STIG being saved. See the Documentation in https://github.com/Cingulara/openstig-docs for more information.


## API Calls
/api/score/{id} to pass in a GUID and score it, if in the database/memory

/api/score/ gives you the example full ASD checklist and scores a default empty checklist

/swagger/ gives you the API structure.

## Making your local Docker image
docker build --rm -t openstig-api-scoring:0.1 .

## creating the user
* ~/mongodb/bin/mongo 'mongodb://root:myp2ssw0rd@localhost'
* use admin
* db.createUser({ user: "openstigscore" , pwd: "openstig1234!", roles: ["readWriteAnyDatabase"]});
* use openstigscore
* db.createCollection("Scores");

## connecting to the database collection straight
~/mongodb/bin/mongo 'mongodb://openstigscore:openstig1234!@localhost/openstigscore?authSource=admin'

## Messaging Platform
Using NATS from Synadia to have a messaging backbone and eventual consistency. Currently publishing to these known items:
* openstig.save.new with payload (new Guid Id)
* openstig.save.update with payload (new Guid Id)
* openstig.upload.new with payload (new Guid Id)
* openstig.upload.update with payload (new Guid Id)

More will follow as this expands for auditing, logging, etc.

### How to run NATS
* docker run --rm --name nats-main -p 4222:4222 -p 6222:6222 -p 8222:8222 nats
* this is the default and lets you run a NATS server version 1.2.0 (as of 8/2018)
* just runs in memory and no streaming (that is separate)