using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float fadeOutTime = 3.0f;  // Duration for fade out. Adjust as needed.

    [Header("Audio Sources")]
    [SerializeField] private AudioSource gameplayMusicSource;
    [SerializeField] private AudioSource gameOverSound;

    [Header("Fade Parameters")]
    [SerializeField] private float fadeDuration = 3.0f;
    [SerializeField] private float fadeInTime = 3.0f; // Duration for fade in. Adjust as needed.

    private void Start()
    {
        gameplayMusicSource.volume = 0f;  // Ensure the gameplay music starts silent
        StartGameplayMusicWithFadeIn();
    }

    public void StartGameplayMusicWithFadeIn()
    {
        if (gameplayMusicSource.isPlaying)
            return;  // If the music is already playing, exit the method without doing anything

        gameplayMusicSource.volume = 0;
        gameplayMusicSource.Play();
        StartCoroutine(FadeInMusic(gameplayMusicSource));
    }

    public void StopGameplayMusicWithFade()
    {
        if (gameplayMusicSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic(gameplayMusicSource));
        }
    }

    // New method for playing game over sound
    public void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            gameOverSound.Play();
        }
        else
        {
            Debug.LogError("GameOver sound not assigned in MusicController.");
        }
    }

    private IEnumerator FadeOutMusic(AudioSource source)
    {
        Debug.Log("Starting to fade out music...");

        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.unscaledDeltaTime / fadeOutTime;

            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset the volume for next play

        Debug.Log("Music faded out and stopped.");
    }

    private IEnumerator FadeInMusic(AudioSource source)
    {
        float targetVolume = 0.5f;  // Max fade in volume

        while (source.volume < targetVolume)
        {
            source.volume += targetVolume * Time.deltaTime / fadeInTime;

            yield return null;
        }

        source.volume = targetVolume;

        Debug.Log("Music faded in.");
    }

    public bool IsGameplayMusicPlaying()
    {
        return gameplayMusicSource.isPlaying;
    }


}
