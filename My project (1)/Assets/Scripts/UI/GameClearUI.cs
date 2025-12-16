using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] Button nextBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] string mainMenuSceneName = "MainMenu";


    private void Start()
    {
        nextBtn.onClick.AddListener(OnClick_Next);
        restartBtn.onClick.AddListener(OnClick_Restart);
        quitBtn.onClick.AddListener(OnClick_Quit);
    }

    void OnClick_Next()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        Debug.Log("Next Level Clicked");
        if (currentIndex + 1 >= totalScenes)
        {
            Debug.Log("´ÙÀ½ ¾À ¾øÀ½");
            Comming_Soon_Popup.instance.OpenPopup();
        }
        else
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
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
