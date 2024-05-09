using UnityEngine;

namespace Views.Settings
{
    public static class SettingsConstants
    {
        public static string GetPlayerNameFromPrefs()
        {
            var playerName = "";
            if (PlayerPrefs.HasKey("PlayerName"))
                playerName = PlayerPrefs.GetString("PlayerName");

            return playerName;
        }
        
        public static float GetTiltSensitivityFromPrefs()
        {
            var sensitivity = 0.5f;
            if (PlayerPrefs.HasKey("TiltSensitivity"))
                sensitivity = PlayerPrefs.GetFloat("TiltSensitivity");

            return sensitivity;
        }
    }
}