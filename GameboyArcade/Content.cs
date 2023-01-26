using Newtonsoft.Json;
using StardewModdingAPI;

namespace GameboyArcade
{
    class Content
    {
        public string Name;
        public string ID;
        public string FilePath;
        public string DGAID;
        public bool EnableEvents = false;
        public string SaveStyle = "LOCAL";
        public string LinkStyle = "NONE";
        public string SoundStyle = "NONE";

        [JsonIgnore]
        public IContentPack ContentPack;

        [JsonIgnore]
        public string UniqueID;
    }
}
