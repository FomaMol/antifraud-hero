using VContainer.Unity;

namespace Services
{
    public class PersistingService : IInitializable
    {
        public string SavedLanguage { get; private set; } = "en";

        public void SetLanguage(string language)
        {
            SavedLanguage = language;
        }

        public void Initialize()
        {
        }
    }
}
