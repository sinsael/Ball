using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine; // 시네머신 네임스페이스 확인 필요

public class Option_UI : MonoBehaviour
{
    // ▼ [복구] 다른 스크립트에서 접근할 수 있도록 인스턴스 부활
    public static Option_UI instance;

    [Header("UI 컴포넌트")]
    public Slider sensivitySlider;
    public Button quitBtn;
    public TMP_InputField sensivityInput;

    [Header("연결")]
    public CinemachineCameraController controller;
    GameObject PauseUI;

    private void Awake()
    {
        // ▼ [복구] 싱글톤 초기화 로직
        if (instance == null)
        {
            instance = this;
            // [중요] DontDestroyOnLoad는 제거했습니다. (Canvas 자식 문제 해결)
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateReferences();
    }

    public void Start()
    {
        // 슬라이더 범위 설정
        sensivitySlider.minValue = 0.1f;
        sensivitySlider.maxValue = 5.0f;

        // 저장된 감도 불러오기 (없으면 기본값 1.0)
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        // UI에 값 적용
        UpdateUI(savedSens);

        // 이벤트 등록
        sensivitySlider.onValueChanged.AddListener(OnSliderChanged);
        sensivityInput.onEndEdit.AddListener(OnInputChanged);
        quitBtn.onClick.AddListener(ClosePopUp);
    }

    private void OnEnable()
    {
        UpdateReferences();
        // 켜질 때 현재 감도에 맞춰 UI 갱신
        float currentSens = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        UpdateUI(currentSens);
    }

    private void UpdateReferences()
    {
        // 씬이 바뀔 때마다 플레이어와 일시정지 UI를 다시 찾습니다.
        if (PauseUI == null)
        {
            GamePauseUI foundScript = FindFirstObjectByType<GamePauseUI>(FindObjectsInactive.Include);
            if (foundScript != null) PauseUI = foundScript.gameObject;
        }

        if (controller == null)
        {
            controller = FindFirstObjectByType<CinemachineCameraController>();
        }
    }

    // 슬라이더 조작 시
    private void OnSliderChanged(float value)
    {
        ApplySensitivity(value);
        sensivityInput.text = value.ToString("F3");
    }

    // 숫자 직접 입력 시
    private void OnInputChanged(string text)
    {
        if (float.TryParse(text, out float newValue))
        {
            // 입력값 범위 제한
            newValue = Mathf.Clamp(newValue, sensivitySlider.minValue, sensivitySlider.maxValue);

            ApplySensitivity(newValue);
            sensivitySlider.value = newValue;
            sensivityInput.text = newValue.ToString("F3");
        }
    }

    // 감도 적용 및 저장 (핵심)
    private void ApplySensitivity(float value)
    {
        // 1. 현재 컨트롤러에 적용
        if (controller != null) controller.mouseSensitivity = value;

        // 2. 다음번을 위해 저장
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }

    // UI 값만 변경 (이벤트 발생 안 시킴)
    void UpdateUI(float value)
    {
        // 리스너가 반응하지 않도록 잠시 끄거나, 그냥 값만 대입
        sensivitySlider.SetValueWithoutNotify(value);
        sensivityInput.text = value.ToString("F1");

        if (controller != null) controller.mouseSensitivity = value;
    }

    void ClosePopUp()
    {
        gameObject.SetActive(false);

        if (PauseUI != null)
        {
            PauseUI.SetActive(true);
        }
    }
}