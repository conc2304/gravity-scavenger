using UnityEngine;

public class HomeAudio : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance && AudioManager.Instance.mainMenuAudio)
        {
            if (AudioManager.Instance.mainMenuAudio.isPlaying) return;
            AudioManager.Play(AudioManager.Instance.mainMenuAudio, true);
        }
    }
}
