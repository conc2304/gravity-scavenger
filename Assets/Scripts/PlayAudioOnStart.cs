using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
