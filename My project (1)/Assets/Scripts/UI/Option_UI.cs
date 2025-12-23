using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option_UI : MonoBehaviour
{
    public Slider sensivitySlider;
    public Button quitBtn;
    public TMP_InputField sensivityInput;
    public CinemachineCameraController controller;
    GameObject PauseUI;

    private static Option_UI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        // 슬라이더 기본 설정
        sensivitySlider.minValue = 0.1f;
        sensivitySlider.maxValue = 1.0f;

        // 초기값 로드
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        // UI 초기화
        UpdateUI(savedSens);

        // 이벤트 리스너 등록
        sensivitySlider.onValueChanged.AddListener(OnSliderChanged); // 슬라이더 조절 시
        sensivityInput.onEndEdit.AddListener(OnInputChanged);       // 숫자 입력 완료 시
        quitBtn.onClick.AddListener(ClosePopUp);

        gameObject.SetActive(false);
    }

    private void OnSliderChanged(float value)
    {
        ApplySensitivity(value);
        sensivityInput.text = value.ToString("F1");
    }

    // 숫자를 직접 입력했을 때
    private void OnInputChanged(string text)
    {
        // 입력받은 문자열을 숫자로 변환 (실패 시 기본값 처리)
        if (float.TryParse(text, out float newValue))
        {
            // 설정 범위를 벗어나지 않게 고정 (0.1 ~ 5.0)
            newValue = Mathf.Clamp(newValue, sensivitySlider.minValue, sensivitySlider.maxValue);

            ApplySensitivity(newValue);
            // 슬라이더 위치도 숫자에 맞춰 이동
            sensivitySlider.value = newValue;
            sensivityInput.text = newValue.ToString("F1");
        }
    }

    //  공통 적용 로직
    private void ApplySensitivity(float value)
    {
        if (controller != null) controller.mouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }

    // UI 업데이트
    void UpdateUI(float value)
    {
        sensivitySlider.value = value;
        sensivityInput.text = value.ToString("F1");
        // 초기화 시에도 컨트롤러 존재 확인
        if (controller != null) controller.mouseSensitivity = value;
    }

    private void OnEnable()
    {
        UpdateReferences();
    }

    private void UpdateReferences()
    {
        GamePauseUI foundScript = FindAnyObjectByType<GamePauseUI>(FindObjectsInactive.Include);
        if (foundScript != null) PauseUI = foundScript.gameObject;

        controller = FindFirstObjectByType<CinemachineCameraController>();
    }

    void ClosePopUp()
    {
        gameObject.SetActive(false);

        // 핵심: PauseUI가 존재할 때만 다시 켜줌 (타이틀 씬에서는 무시됨)
        if (PauseUI != null)
        {
            PauseUI.SetActive(true);
        }
    }
}
