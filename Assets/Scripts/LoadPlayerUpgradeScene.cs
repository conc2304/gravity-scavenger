using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayerUpgradeScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        // Pause 1 second and then load upgrade scene
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Player Upgrades");
    }
}
