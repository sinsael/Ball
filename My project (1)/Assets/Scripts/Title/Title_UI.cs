using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_UI : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] Button achivementsBtn;
    [SerializeField] Button optionBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] GameObject optionUI;
    [SerializeField] GameObject commingSoonUI;
    [SerializeField] string gameSceneName = "Selected";
    void Start()
    {
        Comming_Soon_Popup cspFoundScript = FindFirstObjectByType<Comming_Soon_Popup>(FindObjectsInactive.Include);
        commingSoonUI = cspFoundScript.gameObject;
        Option_UI ouFoundscript = FindFirstObjectByType<Option_UI>(FindObjectsInactive.Include);
        optionUI = ouFoundscript.gameObject;

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
        commingSoonUI.SetActive(true);
    }

    void OnClick_Option()
    {
        Debug.Log("Option Button Clicked");
        optionUI.SetActive(true);
    }
    void OnClick_Quit()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
}
