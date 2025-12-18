using Unity.Cinemachine;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

public class CinemachineCameraController : MonoBehaviour
{
    GameInput input;

    [Header("추적 대상")]
    [Tooltip("따라다닐 공(Player) 오브젝트를 여기에 넣으세요")]
    public Transform playerBall; // ★ 수정됨: 추적할 공

    [Header("카메라 연결")]
    public Transform realCamera;

    [Header("설정")]
    [Range(0, 2)]
    [Tooltip("마우스 감도")]
    public float mouseSensitivity = 1.0f;

    [Tooltip("상단 회전 제한")]
    public float topClamp = 70.0f;

    [Tooltip("하단 회전 제한")]
    public float bottomClamp = -10.0f; // ★ 수정됨: 바닥 뚫림 방지 위해 -30 -> -10 추천

    [Header("줌 설정")]
    public float defaultDistance = 10.0f;

    private float currentSmoothTime;
    private float targetDistance;
    private Vector3 _currentZoomVelocity;

    // 내부 변수
    private float _targetYaw;
    private float _targetPitch;

    private void Awake()
    {
        input = new GameInput();
    }

    private void OnEnable()
    {
        input.Camera.Enable();
    }
    private void OnDisable()
    {
        input.Camera.Disable();
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _targetYaw = angles.y;
        _targetPitch = angles.x;

        targetDistance = defaultDistance;
        currentSmoothTime = 0.5f; // 초기값

        if (realCamera != null)
        {
            // 시작 시 현재 거리를 기본값으로
            float currentDist = Mathf.Abs(realCamera.localPosition.z);
            if (currentDist > 0.1f)
            {
                defaultDistance = currentDist;
                targetDistance = defaultDistance;
            }
        }
    }

    void LateUpdate()
    {
        if (StageGameManager.instance.currentGameState == GameState.Paused ||
             StageGameManager.instance.currentGameState == GameState.GameClear) return;

        if (playerBall == null) return;

        // 1. 위치 따라가기
        transform.position = playerBall.position;

        // 2. 마우스 회전
        if (input != null && Mouse.current != null)
        {
            Vector2 mouseDelta = input.Camera.Look.ReadValue<Vector2>();
            float mouseX = mouseDelta.x * mouseSensitivity;
            float mouseY = mouseDelta.y * mouseSensitivity;

            if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
            {
                _targetYaw += mouseX;
                _targetPitch -= mouseY;
            }
        }

        _targetPitch = ClampAngle(_targetPitch, bottomClamp, topClamp);
        transform.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0.0f);

        // 3. 줌 로직 (설정된 currentSmoothTime 사용)
        if (realCamera != null)
        {
            Vector3 targetLocalPos = new Vector3(0, 0, -targetDistance);

            realCamera.localPosition = Vector3.SmoothDamp(
                realCamera.localPosition,
                targetLocalPos,
                ref _currentZoomVelocity,
                currentSmoothTime // 상황에 따라 바뀐 시간 적용
            );
        }
    }

    // 줌 인/아웃
    public void SetZoom(float distance, float time)
    {
        targetDistance = distance;
        currentSmoothTime = time; // 아이템이 정한 속도로 변경
    }

    // 원상복구 명령 (시간)
    public void ResetZoom(float time)
    {
        targetDistance = defaultDistance;
        currentSmoothTime = time; // 아이템이 정한 속도로 복구
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}