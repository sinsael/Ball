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
    public CinemachineCamera CineCamera;

    [Header("설정")]
    [Range(0, 2)]
    [Tooltip("마우스 감도")]
    public float mouseSensitivity = 1.0f;

    [Tooltip("상단 회전 제한")]
    public float topClamp = 70.0f;

    [Tooltip("하단 회전 제한")]
    public float bottomClamp = -10.0f; // ★ 수정됨: 바닥 뚫림 방지 위해 -30 -> -10 추천

    [Header("줌 설정 (FOV 방식 추천)")]
    public float defaultFOV = 60f;
    private float _targetFOV;
    private float _zoomSpeed;

    // 내부 변수
    private float _targetYaw;
    private float _targetPitch;

    private void Awake()
    {
        input = new GameInput();
    }


    private void OnEnable() => input.Camera.Enable();
    private void OnDisable() => input.Camera.Disable();

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _targetYaw = angles.y;
        _targetPitch = angles.x;
    }

    void LateUpdate()
    {
        if (StageGameManager.instance.currentGameState == GameState.Paused ||
             StageGameManager.instance.currentGameState == GameState.GameClear) return;

        if (playerBall == null) return;

        // 1. 위치 따라가기 (피벗 이동)
        transform.position = playerBall.position;

        // 2. 마우스 회전 (이 부분이 있어야 마우스를 따라갑니다)
        Vector2 mouseDelta = input.Camera.Look.ReadValue<Vector2>();
        _targetYaw += mouseDelta.x * mouseSensitivity;
        _targetPitch -= mouseDelta.y * mouseSensitivity;
        _targetPitch = ClampAngle(_targetPitch, bottomClamp, topClamp);

        // 피벗 오브젝트를 회전시킴
        transform.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0.0f);

        // 3. 줌 로직 (슬로우 모션 영향을 받지 않도록 unscaledDeltaTime 사용)
        if (CineCamera != null)
        {
            CineCamera.Lens.FieldOfView = Mathf.Lerp(
                CineCamera.Lens.FieldOfView,
                _targetFOV,
                Time.unscaledDeltaTime * _zoomSpeed
            );
        }
    }

    // 줌 인/아웃
    public void SetZoom(float distance, float time)
    {
        _targetFOV = distance;
        _zoomSpeed = 1f / Mathf.Max(time, 0.01f); // 아이템이 정한 속도로 변경
    }

    // 원상복구 명령 (시간)
    public void ResetZoom(float time)
    {
        _targetFOV = defaultFOV;
        _zoomSpeed = 1f / Mathf.Max(time, 0.01f);
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}