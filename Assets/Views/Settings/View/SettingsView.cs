using UiViewController;

namespace Views.Settings
{
    public class SettingsView : BaseUiViewController<SettingsViewRefs>
    {
        #region private variables
        private ISettings _settings;
        #endregion

        private void Awake()
        {
            #if UNITY_WEBGL
            _ViewRefs.TiltSensitivitySlider.gameObject.SetActive(false);
            #endif
        }
        
        #region base methods
        public override void Show(object viewData)
        {
            base.Show(viewData);
            var settingsViewData = (SettingsViewData) viewData;
            _settings = settingsViewData.Settings;
            var savedData = settingsViewData.SavedData;
            Initialize(savedData);
        }

        protected override void EnableButtons()
        {
            base.EnableButtons();
            _ViewRefs.ResetSettingsButton.onClick.AddListener(OnResetButtonClicked);
            _ViewRefs.SaveSettingsButton.onClick.AddListener(OnSaveButtonClicked);
            _ViewRefs.MuteButton.onClick.AddListener(_settings.Mute);
        }

        protected override void DisableButtons()
        {
            base.DisableButtons();
            _ViewRefs.ResetSettingsButton.onClick.RemoveListener(OnResetButtonClicked);
            _ViewRefs.SaveSettingsButton.onClick.RemoveListener(OnSaveButtonClicked);
            _ViewRefs.MuteButton.onClick.RemoveListener(_settings.Mute);
        }
        #endregion

        #region private methods
        private void Initialize(SavedSettingsData savedData)
        {
            _ViewRefs.TiltSensitivitySlider.value = savedData.TiltSensitivity;
            _ViewRefs._PlayerNameText.text = savedData.PlayerName;
            _ViewRefs._PlayerNameInputField.text = savedData.PlayerName;
        }
        
        private void OnResetButtonClicked()
        {
            _settings.ResetSettings();
            _ViewRefs._PlayerNameText.text = "NAME";
            _ViewRefs._PlayerNameInputField.text = "";
            _ViewRefs.TiltSensitivitySlider.value = 0.5f;
        }

        private void OnSaveButtonClicked()
        {
            var saveData = new SaveSettingsData();
            saveData.PlayerName = _ViewRefs._PlayerNameInputField.text;
            saveData.TiltSensitivity = _ViewRefs.TiltSensitivitySlider.value;
            _settings.SaveAllSettings(saveData);
            Hide();
        }
        #endregion
    }
}