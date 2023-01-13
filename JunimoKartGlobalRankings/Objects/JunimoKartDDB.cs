using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace JunimoKartGlobalRankings
{
    [DynamoDBTable(TABLE_NAME)]
    public class JunimoKartDDB
    {
        public const string TABLE_NAME = "StardewRankings";
        public const string HIGH_SCORE_INDEX_NAME = "HighScores";

        [DynamoDBHashKey]
        public string User { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBGlobalSecondaryIndexRangeKey(HIGH_SCORE_INDEX_NAME)]
        public int Score { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey(HIGH_SCORE_INDEX_NAME)]
        public string Game { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string Farm { get; set; }

        [DynamoDBProperty]
        public string Timestamp { get; set; }
    }
}
