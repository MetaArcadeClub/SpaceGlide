using TMPro;
using UiViewController;
using UnityEngine.UI;

namespace Views.Settings
{
    public class SettingsViewRefs : BaseUiViewRefs
    {
        public TMP_InputField _PlayerNameInputField;
        public TextMeshProUGUI _PlayerNameText;
        public Slider TiltSensitivitySlider;
        public Button ResetSettingsButton;
        public Button SaveSettingsButton;
        public Button MuteButton;
    }
}