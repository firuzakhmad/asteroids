using UnityEngine;
using UnityEngine.UIElements;

public class HUB : MonoBehaviour
{
    private UIDocument m_uiDocument;

    [SerializeField]
    private ScoreManager m_scoreManager;

    [SerializeField]
    private VisualElement m_gameOverScreen;


    private void OnEnable()
    {
        m_uiDocument = gameObject.GetComponent<UIDocument>();
        VisualElement root =  m_uiDocument.rootVisualElement;

        VisualElement topBar = root.Q<VisualElement>("TopBar");
        topBar.dataSource = m_scoreManager;

        m_gameOverScreen = root.Q<VisualElement>("GameOver");

        VisualElement retryButton = m_gameOverScreen.Q<VisualElement>("RetryButton");
        Clickable retryClickable = new Clickable(() =>
        {
            HandleRetryEvent();
        });
        retryButton.AddManipulator(retryClickable);

        VisualElement quitButton = m_gameOverScreen.Q<VisualElement>("QuitButton");
        Clickable quiteClickable = new Clickable(() =>
        {
            HandleQuiteEvent();
        });
        quitButton.AddManipulator(quiteClickable);

        GameEvents.Instance.onGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        m_gameOverScreen.RemoveFromClassList("hidden");
    }

    private void HandleRetryEvent()
    {
        GameEvents.Instance.OnRetry();
        m_gameOverScreen.AddToClassList("hidden");

    }

    private void HandleQuiteEvent()
    {
        Application.Quit();

#if UNITY_EDITOR
    UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

}
