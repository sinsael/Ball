using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stage_Slot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI slotNumberText;
    [SerializeField] Button selected;

    [HideInInspector] public int number; // 슬롯 번호
    Stage_Select_Popup slotSelected; // 팝업 스크립트 참조

    void Start()
    {
        selected.onClick.AddListener(OnSlotClick);
    }

    public void OnSlotClick()
    {
        slotSelected.OpenPopup(number);
    }

    // 슬롯 번호 및 팝업 스크립트 설정
    public void SetSlotNumber(int myNum ,Stage_Select_Popup sp, bool isEnable)
    {
        number = myNum;
        slotSelected = sp;
        slotNumberText.text = number.ToString();

        selected.interactable = isEnable;
    }
}
