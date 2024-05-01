using UnityEngine;

public class GameOverAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Play(AudioManager.Instance.gameOverAudio, true);
    }
}
