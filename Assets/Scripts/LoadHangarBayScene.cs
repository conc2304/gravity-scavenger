using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadHangarBayScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadHangarBay());
        }
    }

    IEnumerator LoadHangarBay()
    {
        // Pause 1 second and then load upgrade scene
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Player Upgrades");
    }
}
