using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] Button nextBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string nextLevelSceneName = "NextLevel";


    private void Start()
    {
        nextBtn.onClick.AddListener(OnClick_Next);
        restartBtn.onClick.AddListener(OnClick_Restart);
        quitBtn.onClick.AddListener(OnClick_Quit);
    }

    void OnClick_Next()
    {
        Debug.Log("Next Level Clicked");
        SceneManager.LoadScene(nextLevelSceneName);
    }

    void OnClick_Restart()
    {
        Debug.Log("Restart Level Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnClick_Quit()
    {
        Debug.Log("Quit to Main Menu Clicked");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
