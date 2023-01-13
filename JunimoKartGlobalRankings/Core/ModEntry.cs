using StardewModdingAPI;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BirbShared;
using HarmonyLib;
using Amazon.CognitoIdentity;

namespace JunimoKartGlobalRankings
{
    public class ModEntry : Mod
    {
        internal static ModEntry Instance;

        internal static CognitoAWSCredentials Credentials;
        internal static AmazonDynamoDBClient DdbClient;
        internal static DynamoDBContext DdbContext;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Log.Init(this.Monitor);

            this.Helper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;
        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            new Harmony(this.ModManifest.UniqueID).PatchAll();

            #pragma warning disable CA2000
            Credentials = new CognitoAWSCredentials(
                    "us-west-2:2e234341-a166-4d68-94f8-f2e1ffb14e73", // Identity pool ID
                    Amazon.RegionEndpoint.USWest2 // Region
                );

            Log.Info("Using credentials: " + Credentials.GetIdentityId());

            DdbClient = new AmazonDynamoDBClient(Credentials, Amazon.RegionEndpoint.USWest2);
            DdbContext = new DynamoDBContext(DdbClient);
            #pragma warning restore CA2000
        }
    }
}
