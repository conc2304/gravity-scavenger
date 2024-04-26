using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A singleton class to access the audio sources to control them from anywhere
public class AudioManager : MonoBehaviour
{
    // Make a private variable so others can't update or change the instance
    private static AudioManager _instance;

    // Expose the audio sources to be used 
    public static AudioSource[] AudioSources;

    // Public getter to access the private controlled instance
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                print("No AudioManager instantiated");
            }
            if (_instance.GetComponents<AudioSource>() == null)
            {
                print("No AudioSources In Component");
            }

            return _instance;
        }
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

    public static void Play(AudioSource audioSource)
    {
        audioSource.Play();
    }

    // Set thee audio sources and instance on awake
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        AudioSources = _instance.GetComponents<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
