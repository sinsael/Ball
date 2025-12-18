using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Stage_Selected_Slot_Main : MonoBehaviour
{
    [SerializeField] GameObject slot;
    [SerializeField] GameObject exitPopup;
    [SerializeField] int slotCount;
    [SerializeField] Stage_Select_Popup sp;

    [SerializeField] string sceneNamePrefix = "Stage_";

    List<string> existingScenes = new List<string>(); // 빌드 설정에 있는 씬 이름 저장

    GameInput input;

    private void Awake()
    {
        input = new GameInput();
    }

    private void OnEnable()
    {
        input.Game.Enable();
    }

    private void OnDisable()
    {
        input.Game.Disable();
    }

    void Start()
    {
        Cursor.visible = true;
        CheckExistingScene();
        GenerateSlot();
    }

    private void Update()
    {
        if (input.Game.Pause.WasPressedThisFrame())
        {
            Debug.Log("Exit Popup Open");
            exitPopup.gameObject.SetActive(true);
        }
    }

    void CheckExistingScene()
    {
        int scenesCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < scenesCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            existingScenes.Add(sceneName);
        }
    }

    void GenerateSlot()
    {
        if(slot == null)
        {
            Debug.LogError("슬롯 장착 안됨");
            return;
        }

        for(int i = 0; i < slotCount; i++)
        {
            GameObject slots = Instantiate(slot, transform);

            Stage_Slot stage_Slot = slots.GetComponent<Stage_Slot>();
            if(stage_Slot != null)
            {
                int stageNum = i + 1; // 1부터 시작
                string targetSceneName = sceneNamePrefix + stageNum; // e.g., "Stage_1", "Stage_2", etc.
                bool isSceneExist = existingScenes.Contains(targetSceneName); // 씬 존재 여부 확인
                stage_Slot.SetSlotNumber(i + 1, sp, isSceneExist);

                if (i == 0)
                {
                    // 슬롯 프리팹 안에 있는 Button 컴포넌트를 찾음
                    UnityEngine.UI.Button btn = slots.GetComponentInChildren<UnityEngine.UI.Button>();
                    if (btn != null)
                    {
                        // 게임 시작하자마자 이 버튼이 선택된 상태가 됨 -> 키보드 한번만 눌러도 작동!
                        EventSystem.current.SetSelectedGameObject(btn.gameObject);
                    }
                }
            }
        }
    }
}
