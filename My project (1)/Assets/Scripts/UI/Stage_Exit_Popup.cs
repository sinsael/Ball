using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Stage_Exit_Popup : MonoBehaviour
{
    [SerializeField] Button yesBtn;
    [SerializeField] Button noBtn;
    [SerializeField] string mainMenuSceneName = "Stage_Selected_Slot_Main";

    private void Start()
    {
        yesBtn.onClick.AddListener(OnClick_Yes);
        noBtn.onClick.AddListener(OnClick_No);
        gameObject.SetActive(false);
    }

    void OnClick_Yes()
    {
        Debug.Log("스테이지 종료 확인");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnClick_No()
    {
        Debug.Log("스테이지 종료 취소");
        gameObject.SetActive(false);
    }
}
