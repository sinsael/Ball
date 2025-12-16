using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage_Select_Popup : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] Button cancleBtn;

    [SerializeField] string gameSceneName = "Stage_";
    int selectedStageNumber;



    private void Start()
    {
        startBtn.onClick.AddListener(OnClick_Start);
        cancleBtn.onClick.AddListener(OnClick_Cancle);

        gameObject.SetActive(false);
    }

    public void OpenPopup(int stageNumber)
    {
        selectedStageNumber = stageNumber;

        gameObject.SetActive(true);
        Debug.Log($"{stageNumber} 선택");
        if (startBtn != null)
        {
            EventSystem.current.SetSelectedGameObject(startBtn.gameObject);
        }
    }

    void OnClick_Start()
    {
        Debug.Log($"{selectedStageNumber} 스테이지 시작");
        SceneManager.LoadScene(gameSceneName + selectedStageNumber.ToString());
    }

    void OnClick_Cancle()
    {
        Debug.Log("취소");
        selectedStageNumber = 0;
        gameObject.SetActive(false);
    }
}
