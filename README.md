![.NET Core Build and Test](https://github.com/Cingulara/openrmf-api-scoring/workflows/.NET%20Core%20Build%20and%20Test/badge.svg)

# OpenRMF-api-scoring
This is the OpenRMF Scoring API for scoring a checklist. This piece of the OpenRMF tool allows reading of the scoring database that is updated with
eventual consistency using the openrmf-msg-score project that reads newer/updated XML and scores by category and status for each vulnerability in the 
appropriate STIG being saved. See the Documentation in https://github.com/Cingulara/openstig-docs for more information.


## API Calls
* /{id} - pass in an internal ID and score it, if in the database
* /artifact/{id} - pass in a GUID and score it, if in the database
* /system/{id} - pass in to get the score across all checklists in a given system
*/ - POST the raw XML string to get back a score on that data dynamically
* /swagger/ gives you the API structure.

## Making your local Docker image
docker build --rm -t openrmf-api-scoring:0.13 .

## creating the user
* ~/mongodb/bin/mongo 'mongodb://root:myp2ssw0rd@localhost'
* use admin
* db.createUser({ user: "openrmfscore" , pwd: "openrmf1234!", roles: ["readWriteAnyDatabase"]});
* use openstigscore
* db.createCollection("Scores");
* db.Scores.createIndex({ artifactId: 1 })
* db.Scores.createIndex({ systemGroupId: 1 })
* db.Scores.createIndex({ hostName: 1 })
* db.Scores.createIndex({ stigType: 1 })

## connecting to the database collection straight
~/mongodb/bin/mongo 'mongodb://openrmfscore:openrmf1234!@localhost/openrmfscore?authSource=admin'

## Messaging Platform
Using NATS from Synadia to have a messaging backbone and eventual consistency. This Score API reads data from the Score database filled by listening to messages.  Currently publishing to these known items:
* openstig.save.new with payload (new Guid Id)
* openstig.save.update with payload (new Guid Id)
* openstig.upload.new with payload (new Guid Id)
* openstig.upload.update with payload (new Guid Id)

More will follow as this expands for auditing, logging, etc.

### How to run NATS
* docker run --rm --name nats-main -p 4222:4222 -p 6222:6222 -p 8222:8222 nats
* this is the default and lets you run a NATS server version 1.2.0 (as of 8/2018)
* just runs in memory and no streaming (that is separate)