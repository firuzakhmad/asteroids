using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private void OnEnable()
    {
        m_uiDocument = gameObject.GetComponent<UIDocument>();
        VisualElement root =  m_uiDocument.rootVisualElement;

        VisualElement quitButton = root.Q<VisualElement>("QuitButton");
        Clickable quiteClickable = new Clickable(() =>
        {
            HandleQuiteEvent();
        });
        quitButton.AddManipulator(quiteClickable);

        VisualElement playButton = root.Q<VisualElement>("PlayButton");
        Clickable playClickable = new Clickable(() =>
        {
            Debug.Log("Play clicked");
            HandlePlayEvent();
        });
        playButton.AddManipulator(playClickable);

    }

    private void HandleQuiteEvent()
    {
        Application.Quit();

#if UNITY_EDITOR
    UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

    private void HandlePlayEvent()
    {
        SceneManager.LoadScene("GameScene");
    }
}
