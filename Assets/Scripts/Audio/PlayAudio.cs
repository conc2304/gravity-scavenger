using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Play(AudioManager.Instance.gamePlayAudio, true);
    }
}
