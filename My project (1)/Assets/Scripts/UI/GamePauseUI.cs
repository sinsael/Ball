using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button continueBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button optionBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] GameObject optionUI;
    [SerializeField] string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        continueBtn.onClick.AddListener(OnClick_Continue);
        restartBtn.onClick.AddListener(OnClick_Restart);
        optionBtn.onClick.AddListener(OnClick_Option);
        quitBtn.onClick.AddListener(OnClick_Quit);
    }

    private void OnEnable()
    {
        Option_UI option_UI = FindFirstObjectByType<Option_UI>(FindObjectsInactive.Include);
        optionUI = option_UI.gameObject;
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
        optionUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void OnClick_Quit()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
