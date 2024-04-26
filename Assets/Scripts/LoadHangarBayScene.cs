using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadHangarBayScene : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadHangarBay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadHangarBay());
        }
    }

    IEnumerator LoadHangarBay()
    {
        // pause this coroutine for 1 seconds and then load upgrade scene
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Player Upgrades");
    }
}
