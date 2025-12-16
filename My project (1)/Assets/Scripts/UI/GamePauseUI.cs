using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button continueBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button optionBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] string mainMenuSceneName = "MainMenu";
    private void Start()
    {
        continueBtn.onClick.AddListener(OnClick_Continue);
        restartBtn.onClick.AddListener(OnClick_Restart);
        optionBtn.onClick.AddListener(OnClick_Option);
        quitBtn.onClick.AddListener(OnClick_Quit);
    }

    private void OnClick_Continue()
    {
        StageGameManager.instance.ChangeGameState(StageGameManager.instance.previousGameState);
    }

    private void OnClick_Restart()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnClick_Option()
    {
        Debug.Log("Option Clicked");
        Comming_Soon_Popup.instance.OpenPopup();
    }
    private void OnClick_Quit()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
