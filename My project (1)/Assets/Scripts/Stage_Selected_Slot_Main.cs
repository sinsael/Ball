using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage_Selected_Slot_Main : MonoBehaviour
{
    [SerializeField] GameObject slot;
    [SerializeField] int slotCount;
    [SerializeField] Stage_Select_Popup sp;

    [SerializeField] string sceneNamePrefix = "Stage_";

    List<string> existingScenes = new List<string>(); // ∫ÙµÂ º≥¡§ø° ¿÷¥¬ æ¿ ¿Ã∏ß ¿˙¿Â

    void Start()
    {
        CheckExistingScene();
        GenerateSlot();
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
            Debug.LogError("ΩΩ∑‘ ¿Â¬¯ æ»µ ");
            return;
        }

        for(int i = 0; i < slotCount; i++)
        {
            GameObject slots = Instantiate(slot, transform);

            Stage_Slot stage_Slot = slots.GetComponent<Stage_Slot>();
            if(stage_Slot != null)
            {
                int stageNum = i + 1; // 1∫Œ≈Õ Ω√¿€
                string targetSceneName = sceneNamePrefix + stageNum; // e.g., "Stage_1", "Stage_2", etc.
                bool isSceneExist = existingScenes.Contains(targetSceneName); // æ¿ ¡∏¿Á ø©∫Œ »Æ¿Œ
                stage_Slot.SetSlotNumber(i + 1, sp, isSceneExist);
            }
        }
    }
}
