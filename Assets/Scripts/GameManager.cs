using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama.Game;
using Nakama.Leaderboard;
using Views.Settings;


public class GameManager : MonoBehaviour
{
    #region public variables
    public Player player;
    public GameBounds bounds;
    public CometSpawner cometSpawner;
    public Text scoreText;
    public GameObject gameOver;
    public GameObject getReady;
    public SpriteRenderer playerSpriteRenderer;
    public Text countdownText;
    public GameObject resumeButton;
    public GameObject retryButton;
    public GameObject homeButton;  // Add this reference to the Home button
    public GameObject muteButton;

    public Image[] heartImages;  // Array of Image components for each heart
    public Sprite fullHeart;     // Sprite for the full heart
    public Sprite emptyHeart;    // Sprite for the empty heart
    public float startGravity = 0.5f;  // Initial gravity
    public float maxGravity = 10f;  // Maximum possible gravity
    public float gravityIncreaseRate = 0.01f;  // How fast the gravity increases. Adjust as needed.
    public float timeToIncreaseFallSpeed = 5f;  // Time in seconds before increasing fall speed. Adjusted to 15 seconds.
    public string playerName => SettingsConstants.GetPlayerNameFromPrefs();
    
    [Header("Nakama Classes")]
    public NakamaGameModule NakamaGame;
    #endregion

    #region private variables
    private RectTransform scoreTextRectTransform;
    private Vector2 originalScorePosition;
    private Vector2 originalScoreScale;
    long finalScore;

    private Coroutine scoreCoroutine = null; // Keep track of the score animation coroutine.

    private int currentDisplayScore = 0;  // Keeps track of the score currently being displayed.
    private int score;
    private float scoreIncrementInterval = 0.1f;
    private float scoreMultiplier = 5f;
    private float timeToIncreaseMultiplier = 15f;
 

    private bool isGameOver = false;
    private bool isMuted = false;

    #endregion

    #region public methods
    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;

        public ScoreEntry(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        if (!isGameOver)
        {
            if (player)
            {
                player.enabled = false;
            }
            retryButton.SetActive(false);
            resumeButton.SetActive(true);
            countdownText.gameObject.SetActive(false);
            if (homeButton) homeButton.SetActive(true); // Show the Home button when game is paused
            if (muteButton) muteButton.SetActive(true); // Show the Mute button when game is paused
            if (scoreTextRectTransform)
            {
                scoreTextRectTransform.anchoredPosition = Vector2.zero;
                scoreTextRectTransform.localScale = originalScoreScale * 2; // Double the scale
            }
        }
        SetHeartDisplayVisibility(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is running normally
        SceneManager.LoadScene("mainmenuscene", LoadSceneMode.Single); // This will reload and reinitialize the "mainmenuscene"
    }

    public async Task GameOver()
    {
        isGameOver = true;
        if (gameOver) gameOver.SetActive(true);
        if (retryButton) retryButton.SetActive(true);  // Show the Retry button
        if (resumeButton) resumeButton.SetActive(false); // Hide the Resume button
        if (homeButton) homeButton.SetActive(true); // Show the Home button when game is paused
        if (muteButton) muteButton.SetActive(true); // Show the Mute button when game is paused
        if (scoreText)
        {
            scoreText.rectTransform.anchoredPosition = Vector2.zero; // this assumes that the anchor is centered, otherwise, adjust as needed.
            scoreTextRectTransform.localScale = originalScoreScale * 2; // Double the scale
        }

        string playerName = this.playerName; // Get the player's name
        long finalScore = CalculateFinalScore();
        var playerData = new Dictionary<string, string> { { "playerName", playerName } };

        var scoreData = new LeaderboardSubmitScoreData {LeaderboardId = "weekly_top_200", Score = finalScore};
        NakamaGame.SubmitScore(scoreData);
        Pause();
        DestroyAllStars();
        SetHeartDisplayVisibility(false);
        Physics2D.gravity = new Vector2(0f, -startGravity);

        // Play the game over sound
        MusicController musicController = FindObjectOfType<MusicController>();
        if (musicController != null)
        {

            // Stop the gameplay music with fade
            musicController.StopGameplayMusicWithFade();
            // Play the game over sound
            musicController.PlayGameOverSound();
        }
        else
        {
            Debug.LogError("No MusicController found in the scene.");
        }
        if (homeButton) homeButton.SetActive(true); // Show the Home button when game is paused
    }
    
    public void Retry()
    {
        // Here you can either reload the current scene or reset everything like in the Play() function
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }
    
    public void Resume()
    {
        resumeButton.SetActive(false);
        StartCoroutine(ResumeAfterCountdown());
        Debug.Log("Game Resumed");
        SetHeartDisplayVisibility(true);

        if (homeButton) homeButton.SetActive(false);
        if (muteButton) muteButton.SetActive(false);
        if (scoreTextRectTransform)
        {
            scoreTextRectTransform.anchoredPosition = originalScorePosition; // Reset position
            scoreTextRectTransform.localScale = originalScoreScale; // Reset to original scale
        }
    }

    public void Play()
    {
        isGameOver = false;
        score = 0;
        currentDisplayScore = 0;

        if (homeButton) homeButton.SetActive(false);
        if (muteButton) muteButton.SetActive(false);

        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }

        scoreMultiplier = 10f;

        if (player)
        {
            player.RestartPlayer();
            player.enabled = false;  // Disable the player initially
            UpdateLivesDisplay(player.lives);
        }
        if (cometSpawner)
        {
            cometSpawner.gameBounds = bounds;
            cometSpawner.ResetSpawnRate();
        }
        if (playerSpriteRenderer)
        {
            playerSpriteRenderer.color = new Color(1f, 1f, 1f, 0f); // Set alpha to 0 (invisible)
        }

        if (resumeButton) resumeButton.SetActive(false);
        if (retryButton) retryButton.SetActive(false);  // Hide the retry button when starting
        if (getReady) getReady.SetActive(true);  // Display the 'Getting Ready' text
        if (gameOver) gameOver.SetActive(false);
        if (scoreText) scoreText.gameObject.SetActive(false);   // Hide the Score
        if (scoreText) scoreText.rectTransform.anchoredPosition = originalScorePosition;
        if (scoreTextRectTransform)
        {
            scoreTextRectTransform.anchoredPosition = originalScorePosition;
        }

        Time.timeScale = 1f; // Keep the game paused while the countdown is running
        StartCoroutine(StartCountdownAndPlay());
        DestroyAllComets();
        SetHeartDisplayVisibility(false);
    }

    public void DestroyAllStars()
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
        foreach (GameObject star in stars)
        {
            Destroy(star);
        }
    }

    public void IncreaseScore()
    {
        int newScore = score + (int)(1 * scoreMultiplier);
        StartScoreAnimation(newScore);
        score = newScore;  // Store the actual score.
    }

    public void IncreaseScoreBy(int amount)
    {
        int newScore = score + (int)(amount * scoreMultiplier);
        StartScoreAnimation(newScore);
        score = newScore;  // Store the actual score.
    }
    #endregion
    
    #region mono methods
    private void Awake()
    {
        StartCoroutine(CheckForPauseInput());
        Application.targetFrameRate = 120;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (getReady) getReady.SetActive(true);
        if (gameOver) gameOver.SetActive(false);
        if (scoreText) scoreText.gameObject.SetActive(false);  // Hide the Score                                                        
        if (resumeButton) resumeButton.SetActive(false);
        if (homeButton) homeButton.SetActive(false); // Ensure the Home button is hidden on start
        if (muteButton) muteButton.SetActive(false);
        if (playerSpriteRenderer)
        {
            playerSpriteRenderer.color = new Color(1f, 1f, 1f, 0f); // Set alpha to 0 (invisible)
        }
        if (player)
        {
            player.OnLivesChanged += UpdateLivesDisplay;
        }
        if (scoreText)
        {
            scoreTextRectTransform = scoreText.GetComponent<RectTransform>();
        }
        if (scoreTextRectTransform)
        {
            originalScorePosition = scoreTextRectTransform.anchoredPosition;
            originalScoreScale = scoreTextRectTransform.localScale;
        }

        // Start the countdown and subsequent game start here
        StartCoroutine(StartCountdownAndPlay());
        SetHeartDisplayVisibility(false);
        originalScorePosition = scoreText.rectTransform.anchoredPosition;

    }
    
    private void Start()
    {
        Physics2D.gravity = new Vector2(0f, -startGravity);

        if (player)
        {
            UpdateLivesDisplay(player.lives);
        }

        finalScore = CalculateFinalScore();
        StartCoroutine(IncreaseScoreMultiplier());
        StartCoroutine(IncrementScoreWithTime());
    }

    private void Update()
    {
        // Increase the gravity over time
        float newGravityY = Mathf.MoveTowards(Physics2D.gravity.y, -maxGravity, gravityIncreaseRate * Time.deltaTime);
        Physics2D.gravity = new Vector2(0f, newGravityY);
    }
    
    private void OnDestroy()
    {
        if (player)
        {
            player.OnLivesChanged -= UpdateLivesDisplay;
        }
    }
    #endregion

    #region private methods
    private long CalculateFinalScore()
    {
        // Your code to calculate or retrieve the final score
        // For example:
        return score;
    }

    private void DestroyAllComets()
    {
        Comet[] comets = FindObjectsOfType<Comet>();
        foreach (Comet comet in comets)
        {
            Destroy(comet.gameObject);
        }
    }

    private IEnumerator ResumeAfterCountdown()
    {
        yield return StartCountdown();
        Time.timeScale = 1f;

        if (player)
        {
            player.enabled = true;
        }
        retryButton.SetActive(false);
    }

    private void UpdateLivesDisplay(int remainingLives)
    {
        if (heartImages == null || heartImages.Length == 0)
        {
            Debug.LogError("Heart images are not set!");
            return;
        }

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < remainingLives)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }

    private void SetHeartDisplayVisibility(bool isVisible)
    {
        foreach (Image heart in heartImages)
        {
            heart.gameObject.SetActive(isVisible);
        }
    }
    
    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        getReady.SetActive(true); // Show the "Getting Ready" text

        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSecondsRealtime(1);  // Use WaitForRealtimeSeconds
            count--;
        }

        countdownText.gameObject.SetActive(false);
        getReady.SetActive(false); // Hide the "Getting Ready" text
    }

    private void StartScoreAnimation(int newScore)
    {
        // If there's an ongoing score animation, stop it.
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }

        // Start new animation.
        scoreCoroutine = StartCoroutine(AnimateScore(currentDisplayScore, newScore));
    }

    private IEnumerator AnimateScore(int fromScore, int toScore, float duration = 1f)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Calculate a linear progression towards the final score.
            int tempScore = (int)Mathf.Lerp(fromScore, toScore, elapsed / duration);

            // Ensure that tempScore doesn't decrease
            currentDisplayScore = Mathf.Max(currentDisplayScore, tempScore);
            if (scoreText) scoreText.text = currentDisplayScore.ToString();
            yield return null;
        }

        // Ensure the displayed score matches the exact score at the end of the animation.
        currentDisplayScore = toScore;
        if (scoreText) scoreText.text = currentDisplayScore.ToString();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText) scoreText.text = score.ToString();
    }

    private IEnumerator IncreaseScoreMultiplier()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToIncreaseMultiplier);
            scoreMultiplier += 5f;
        }
    }

    private IEnumerator IncrementScoreWithTime()
    {
        IncreaseScore();  // Increment the score immediately
        while (true)
        {
            yield return new WaitForSeconds(scoreIncrementInterval);
            IncreaseScore();
        }
    }

    private IEnumerator CheckForPauseInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                Pause();
            }
            yield return null;
        }
    }

    private IEnumerator StartCountdownAndPlay()
    {
        yield return StartCoroutine(StartCountdown()); // Run the countdown

        // Make the player visible and enable its controls
        if (playerSpriteRenderer) playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Set alpha to 1 (visible)
        if (player) player.enabled = true;

        // Display the score and lives
        if (scoreText) scoreText.gameObject.SetActive(true);
        UpdateLivesDisplay(player.lives);
        IncreaseScore();  // Start the score
        SetHeartDisplayVisibility(true);

        Time.timeScale = 1f; // Start the game after the countdown

        // Play the gameplay music
        MusicController musicController = FindObjectOfType<MusicController>();
        if (musicController)
        {
            if (!musicController.IsGameplayMusicPlaying())
            {
                musicController.StartGameplayMusicWithFadeIn();
            }
        }
    }
    #endregion
}