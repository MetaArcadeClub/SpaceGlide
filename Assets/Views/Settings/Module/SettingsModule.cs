using UnityEngine;

namespace Views.Settings
{
    public class SettingsModule : MonoBehaviour, ISettings
    {
        #region serialized variables
        [SerializeField] private SettingsView _View;
        #endregion

        #region private variables
        private MainThreadDispatcher _dispatcher;
        private ISettings _settingsImplementation;
        private bool _isMuted = false; 
        #endregion

        #region public methods
        public void ShowView()
        {
            var savedData = LoadSavedData();
            var viewData = new SettingsViewData(this, savedData);
            _View.Show(viewData);
        }
        #endregion

        #region mono methods
        private void Awake()
        {
            _dispatcher = MainThreadDispatcher.Instance();
        }
        #endregion

        #region explicit implementations
        void ISettings.SaveName(string playerName)
        {
            SaveName(playerName);
        }

        void ISettings.SaveTiltSensitivity(float tiltSensitivityValue)
        {
            SaveTiltSensitivity(tiltSensitivityValue);
        }

        void ISettings.SaveAllSettings(SaveSettingsData saveData)
        {
            SaveName(saveData.PlayerName);
            SaveTiltSensitivity(saveData.TiltSensitivity);
            // hide
        }

        void ISettings.ResetSettings()
        {
            Debug.Log("ResetSettings is called");
            PlayerPrefs.DeleteKey("PlayerName");
            PlayerPrefs.DeleteKey("TiltSensitivity");
            PlayerPrefs.Save();
        }

        void ISettings.Mute()
        {
            _isMuted = !_isMuted;
            AudioListener.volume = _isMuted ? 0 : 1;
        }
        #endregion

        #region private methods
        private SavedSettingsData LoadSavedData()
        {
            var savedSettings = new SavedSettingsData();
            savedSettings.PlayerName = SettingsConstants.GetPlayerNameFromPrefs();
            savedSettings.TiltSensitivity = SettingsConstants.GetTiltSensitivityFromPrefs();
            return savedSettings;
        }

        private void SaveName(string playerName)
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save(); 
        }

        private void SaveTiltSensitivity(float tiltSensitivityValue)
        {
            PlayerPrefs.SetFloat("TiltSensitivity", tiltSensitivityValue);
            PlayerPrefs.Save();
        }
        #endregion
    }
}