// Copyright (c) Cingulara LLC 2019 and Tutela LLC 2019. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 license. See LICENSE file in the project root for full license information.
using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace openrmf_scoring_api.Models
{
    /// <summary>
    /// This is the class that shows the score of the STIG for all categories
    /// </summary>

    public class Score
    {

        public Score () {        }

        #region members
        [BsonId]
        // standard BSonId generated by MongoDb
        public ObjectId InternalId { get; set; }
        public string InternalIdString { get { return InternalId.ToString();}}
        
        public ObjectId artifactId { get; set;}

        public string systemGroupId { get; set; }
        public string hostName { get; set;}
        public string stigType { get; set; }
        public string stigRelease { get; set; }
        public string title { get {
            string validHostname = !string.IsNullOrEmpty(hostName)? hostName.Trim() : "Unknown";
            string validStigType = "";
            if (!string.IsNullOrWhiteSpace(stigType)) validStigType = stigType.Trim();
            string validStigRelease = "";
            if (!string.IsNullOrWhiteSpace(stigRelease)) validStigRelease = stigRelease.Trim();

            return validHostname + "-" + validStigType + "-" + validStigRelease;
        }}
        
        [BsonDateTimeOptions]
        public DateTime created { get; set; }
        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime? updatedOn { get; set; }
        public Guid createdBy { get; set; }
        public Guid? updatedBy { get; set; }


        public int totalCat1Open { get; set; }
        public int totalCat1NotApplicable { get; set; }
        public int totalCat1NotAFinding { get; set; }
        public int totalCat1NotReviewed { get; set; }
        public int totalCat2Open { get; set; }
        public int totalCat2NotApplicable{ get; set; }
        public int totalCat2NotAFinding { get; set; }
        public int totalCat2NotReviewed { get; set; }
        public int totalCat3Open { get; set; }
        public int totalCat3NotApplicable { get; set; }
        public int totalCat3NotAFinding { get; set; }
        public int totalCat3NotReviewed { get; set; }

        public int totalOpen { get { return totalCat1Open + totalCat2Open + totalCat3Open;} }
        public int totalNotApplicable { get { return totalCat1NotApplicable + totalCat2NotApplicable + totalCat3NotApplicable;} }
        public int totalNotAFinding { get { return totalCat1NotAFinding + totalCat2NotAFinding + totalCat3NotAFinding;} }
        public int totalNotReviewed { get { return totalCat1NotReviewed + totalCat2NotReviewed + totalCat3NotReviewed;} }

        public int totalCat1 { get { return totalCat1NotAFinding + totalCat1NotApplicable + totalCat1NotReviewed + totalCat1Open;} }
        public int totalCat2 { get { return totalCat2NotAFinding + totalCat2NotApplicable + totalCat2NotReviewed + totalCat2Open;} }
        public int totalCat3 { get { return totalCat3NotAFinding + totalCat3NotApplicable + totalCat3NotReviewed + totalCat3Open;} }
        #endregion

    }
}