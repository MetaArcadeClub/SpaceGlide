namespace Views.Settings
{
    public interface ISettings
    {
        public void SaveName(string playerName);
        public void SaveTiltSensitivity(float tiltSensitivityValue);
        public void SaveAllSettings(SaveSettingsData saveData);
        public void ResetSettings();
        public void Mute();
    }
}