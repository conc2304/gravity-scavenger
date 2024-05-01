using UnityEngine;

// A singleton class to access the audio sources to control them from anywhere
public class AudioManager : MonoBehaviour
{
    // Make a private variable so others can't update or change the instance
    public static AudioManager Instance;

    // Expose the audio sources to be used 
    public AudioSource mainMenuAudio;
    public AudioSource gameOverAudio;
    public AudioSource gamePlayAudio;
    public AudioSource playerUpgradeAudio;
    private static AudioSource[] audioSources;


    // Set the audio sources and instance on awake
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSources = new AudioSource[] { mainMenuAudio, gameOverAudio, gamePlayAudio, playerUpgradeAudio };
    }



    // Various public methods to call on the audio sources
    public static void Mute(AudioSource audioSource)
    {
        audioSource.mute = true;
    }

    public static void unMute(AudioSource audioSource)
    {
        audioSource.mute = false;
    }

    public static void Pause(AudioSource audioSource)
    {
        audioSource.Pause();
    }

    public static void Play(AudioSource audioSource, bool stopOthers)
    {

        // Stop other audio if flag is set
        if (stopOthers)
        {
            foreach (var source in audioSources)
            {
                if (audioSource != source)
                {
                    source.Stop();
                }
            }
        }

        audioSource.Play();
    }
}

