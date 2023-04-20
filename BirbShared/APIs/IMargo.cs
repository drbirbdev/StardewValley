namespace BirbShared.APIs
{
    public interface IMargo
    {
        void RegisterCustomSkillForPrestige(string id);

        IModConfig GetConfig();

        public interface IModConfig
        {
            bool EnableProfessions { get; }
        }
    }
}
