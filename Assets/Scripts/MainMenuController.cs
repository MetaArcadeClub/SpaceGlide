using UnityEngine;
using UnityEngine.UI;
using Social.Leaderboard;
using UnityEngine.SceneManagement;
using Views.Settings;
using System.Collections;
using Views.Username;

public class MainMenuController : MonoBehaviour
{
    #region public variables
    public GameObject playButton;
    public GameObject solanaWalletButton;
    public GameObject walletDisconnectButton;
    public GameObject txtPublicKey;
    public GameObject mainCamera;
    public GameObject txtBalance;
    public GameObject bg;          // Reference to the Background
    public GameObject shadow1;     // Reference to Shadow-1
    public GameObject shadow3;     // Reference to Shadow-3
    public GameObject shadow4;     // Reference to Shadow-4
    public Image logoImage;        // Reference to the logo
    public GameObject starBg;  // Reference to the 'image' Background
    public Button leaderboardButton;
    public Button exitButton;
    
    [Header("Settings")]
    public Button settingsButton;
    #endregion

    #region serialized variables
    [Header("Modules")] 
    [SerializeField] private SettingsModule _SettingsModule;
    [SerializeField] private UsernameView _UsernameView;
    [SerializeField] private SocialLeaderboardModule _LeaderboardModule;
    #endregion

    #region private variables
    #endregion

    #region public methods
    public void Play()
    {
        SceneManager.LoadScene("something space", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    #endregion
    
    #region mono methods
    private void Awake()
    {
        if (playButton) playButton.SetActive(true);
        // if (shadow1) shadow1.SetActive(true);  
        // if (shadow3) shadow3.SetActive(true);   
        // if (shadow4) shadow4.SetActive(true);   
        if (mainCamera) mainCamera.SetActive(true);
        if (txtPublicKey) txtPublicKey.SetActive(true);
        if (txtBalance) txtBalance.SetActive(true);
        if (bg) bg.SetActive(true);         
        if (logoImage) logoImage.gameObject.SetActive(true);
        if (starBg) starBg.SetActive(true); 
        if (starBg) starBg.SetActive(true);
        if (solanaWalletButton) solanaWalletButton.SetActive(true);
        if (walletDisconnectButton) walletDisconnectButton.SetActive(true);
    }

    private IEnumerator Start()
    {
        var name = SettingsConstants.GetPlayerNameFromPrefs();
        if(string.IsNullOrEmpty(name))
        {
            _UsernameView.Show(new SettingsViewData(_SettingsModule, new SavedSettingsData()));
            yield return new WaitUntil(() => !_UsernameView.gameObject.activeSelf);
        }

        Debug.Log("username setup complete");
    }

    private void OnEnable()
    {
        EnableButtons();
    }

    private void OnDisable()
    {
        DisableButtons();
    }
    #endregion

    #region private methods

    private void EnableButtons()
    {
        if (settingsButton) settingsButton.onClick.AddListener(OpenSettings);
        if (exitButton) exitButton.onClick.AddListener(ExitGame);
        if (leaderboardButton) leaderboardButton.onClick.AddListener(OpenLeaderboard);
    }

    private void DisableButtons()
    {
        if (settingsButton) settingsButton.onClick.RemoveListener(OpenSettings);
        if (exitButton) exitButton.onClick.RemoveListener(ExitGame);
        if (leaderboardButton) leaderboardButton.onClick.RemoveListener(OpenLeaderboard);
    }

    private void OpenSettings()
    {
        _SettingsModule.ShowView();
        Debug.Log("Open settings.");
    }
    
    private void OpenLeaderboard()
    {
        _LeaderboardModule.ShowLeaderboard();
    }
    #endregion
}
