using UnityEngine;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    private UIDocument uiDocument;

    // Start is called before the first frame update
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();

        uiDocument.rootVisualElement.Q<TextElement>("finalScore").text = PlayerStatsManager.Instance.points.ToString();
    }

}
