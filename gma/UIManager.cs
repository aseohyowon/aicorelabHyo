using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gamePlayPanel;
    public GameObject levelClearPanel;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        levelClearPanel.SetActive(false);
    }

    public void ShowGamePlay()
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        levelClearPanel.SetActive(false);
    }

    public void ShowLevelClear()
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        levelClearPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
