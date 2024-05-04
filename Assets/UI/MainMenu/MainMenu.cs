using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument mainMenuDocument;
    [SerializeField] private AudioSource confirmAudio; // UI Audio

    private Button startBtn;
    private Button homeBtn;
    private Button helpBtn;
    private Button quitBtn;

    private void OnEnable()
    {
        mainMenuDocument = GetComponent<UIDocument>();

        if (!mainMenuDocument)
        {
            Debug.Log("No Main Menu document found");
        }

        // Assign queried button elements
        startBtn = mainMenuDocument.rootVisualElement.Q("startBtn") as Button;
        helpBtn = mainMenuDocument.rootVisualElement.Q("helpBtn") as Button;
        quitBtn = mainMenuDocument.rootVisualElement.Q("quitBtn") as Button;
        homeBtn = mainMenuDocument.rootVisualElement.Q("homeBtn") as Button;

        // Register click callbacks if button exists
        startBtn?.RegisterCallback<ClickEvent>(OnStartClick);
        helpBtn?.RegisterCallback<ClickEvent>(OnHelpClick);
        quitBtn?.RegisterCallback<ClickEvent>(OnQuitClick);
        homeBtn?.RegisterCallback<ClickEvent>(OnHomeClick);
    }

    public void OnStartClick(ClickEvent evt)
    {
        Debug.Log("Start Clicked");
        StartCoroutine(LoadScene("Play"));
        // Make sure player stats are initialized when we start
        PlayerStatsManager.Instance.Reset();
    }

    public void OnHomeClick(ClickEvent evt)
    {
        Debug.Log("Home Clicked");
        StartCoroutine(LoadScene("Home"));
    }

    public void OnHelpClick(ClickEvent evt)
    {
        Debug.Log("Help Clicked");
        StartCoroutine(LoadScene("Help"));
    }

    public void OnQuitClick(ClickEvent evt)
    {
        Debug.Log("Quit Clicked");
        Application.Quit();
    }


    IEnumerator LoadScene(string scene)
    {
        confirmAudio.Play();

        // Pause 1 second and then load scene
        yield return new WaitForSeconds(confirmAudio.clip.length + 0.1f);
        SceneManager.LoadScene(scene);
    }

}
