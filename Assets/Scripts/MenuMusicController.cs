using System.Collections;
using UnityEngine;

public class MenuMusicController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource menuMusicSource;

    [Header("Fade Parameters")]
    [SerializeField] private float fadeInTime = 3.0f; // Duration for fade in. Adjust as needed.
    [SerializeField] private float fadeOutTime = 3.0f;  // Duration for fade out. Adjust as needed.

    private void Start()
    {
        // Assuming menu music starts playing by default (Play on Awake checked)
        menuMusicSource.volume = 1f;  // Ensure the menu music starts with full volume
    }

    public void StopMenuMusicWithFade()
    {
        StartCoroutine(FadeOutMusic(menuMusicSource));
    }

    private IEnumerator FadeOutMusic(AudioSource source)
    {
        Debug.Log("Starting to fade out menu music...");

        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.unscaledDeltaTime / fadeOutTime;

            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset the volume for next play

        Debug.Log("Menu music faded out and stopped.");
    }
}
