using UnityEngine;
using UnityEngine.UI;

public class Comming_Soon_Popup : MonoBehaviour
{
    public static Comming_Soon_Popup instance;
    [SerializeField] Button okBtn;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = true;
        okBtn.onClick.AddListener(OnClick_Ok);
        gameObject.SetActive(false);
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);
    }

    void OnClick_Ok()
    {
        gameObject.SetActive(false);
    }
}
