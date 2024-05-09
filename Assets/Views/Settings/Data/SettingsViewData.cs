using UiViewController.Data;

namespace Views.Settings
{
    public class SettingsViewData : BaseUiViewData
    {
        public ISettings Settings;
        public SavedSettingsData SavedData;

        public SettingsViewData(ISettings settings, SavedSettingsData savedData)
        {
            Settings = settings;
            SavedData = savedData;
        }
    }
}