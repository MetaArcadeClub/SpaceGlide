//
//  UsernameView.cs
//  
//  Dev: Scott Mitchell
//  Date: 09.05.24.
//

using UnityEngine;
using UiViewController;
using Views.Settings;

namespace Views.Username
{
    sealed public class UsernameView : BaseUiViewController<UsernameViewRefs>
    {
        #region private variables
        private ISettings _settings;
        #endregion

        
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
            _ViewRefs.SaveSettingsButton.onClick.AddListener(OnSaveButtonClicked);
        }

        protected override void DisableButtons()
        {
            base.DisableButtons();
            _ViewRefs.SaveSettingsButton.onClick.RemoveListener(OnSaveButtonClicked);
        }
        #endregion

        #region private methods
        private void Initialize(SavedSettingsData savedData)
        {
        }
        
        private void OnResetButtonClicked()
        {
            _settings.ResetSettings();
            _ViewRefs._PlayerNameInputField.text = "";
        }

        private void OnSaveButtonClicked()
        {
            var saveData = new SaveSettingsData();
            saveData.PlayerName = _ViewRefs._PlayerNameInputField.text;
            _settings.SaveAllSettings(saveData);
            Hide();
        }
        #endregion
    }
}