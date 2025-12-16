using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_UI : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] Button achivementsBtn;
    [SerializeField] Button optionBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] string gameSceneName = "Selected";
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        startBtn.onClick.AddListener(OnClick_Start);
        achivementsBtn.onClick.AddListener(OnClick_Achivements);
        optionBtn.onClick.AddListener(OnClick_Option);
        quitBtn.onClick.AddListener(OnClick_Quit);
    }

    void OnClick_Start()
    {
        Debug.Log("Start Button Clicked");
        SceneManager.LoadScene(gameSceneName);
    }
    void OnClick_Achivements()
    {
        Debug.Log("Achievements Button Clicked");
        Comming_Soon_Popup.instance.OpenPopup();
    }

    void OnClick_Option()
    {
        Debug.Log("Option Button Clicked");
        Comming_Soon_Popup.instance.OpenPopup();
    }
    void OnClick_Quit()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
}
