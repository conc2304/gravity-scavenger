using UnityEngine;

public class PlayerUpgradeAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Play(AudioManager.Instance.playerUpgradeAudio, true);
    }
}
