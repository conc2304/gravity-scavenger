using UnityEngine;

public class HomeAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Play(AudioManager.Instance.mainMenuAudio, true);
    }
}
