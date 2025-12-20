using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineCameraController : MonoBehaviour
{
    GameInput input;

    [Header("추적 대상")]
    public Transform playerBall;

    [Header("카메라 연결")]
    public CinemachineCamera CineCamera;
    private CinemachineThirdPersonFollow _thirdPersonFollow;

    [Header("감도 설정")]
    [Range(0, 5)]
    public float mouseSensitivity = 1.5f;
    [Tooltip("상단 회전 최대")]
    public float topClamp = 70.0f;
    [Tooltip("하단 회전 최대")]
    public float bottomClamp = -10.0f;

    [Header("줌 설정")]
    public float defaultFOV = 60f;
    private float _targetFOV;
    private float _zoomLerpSpeed;

    [Header("오프셋 설정")]
    public Vector3 defaultOffset;
    private Vector3 _targetOffset;
    private float _offsetLerpSpeed;

    private float _targetYaw;
    private float _targetPitch;


    private void Awake()
    {
        input = new GameInput();
        if (CineCamera != null)
        {
            defaultFOV = CineCamera.Lens.FieldOfView;
            _targetFOV = defaultFOV;

            _thirdPersonFollow = CineCamera.GetComponent<CinemachineThirdPersonFollow>();
            defaultOffset = _thirdPersonFollow.ShoulderOffset;
            _targetOffset = defaultOffset;
        }
    }

    private void OnEnable() => input.Camera.Enable();
    private void OnDisable() => input.Camera.Disable();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        _targetYaw = angles.y;
        _targetPitch = angles.x;
    }

    void LateUpdate()
    {
        if (StageGameManager.instance != null)
        {
            if (StageGameManager.instance.currentGameState == GameState.Paused ||
                StageGameManager.instance.currentGameState == GameState.GameClear)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
        }

        if (playerBall == null) return;

        // 피벗 이동
        transform.position = playerBall.position;
         
        // 마우스 회전
        Vector2 mouseDelta = input.Camera.Look.ReadValue<Vector2>();
        _targetYaw += mouseDelta.x * mouseSensitivity;
        _targetPitch -= mouseDelta.y * mouseSensitivity;
        _targetPitch = ClampAngle(_targetPitch, bottomClamp, topClamp);

        transform.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0.0f);

        // 3. 시네머신 줌
        if (CineCamera != null)
        {
            LensSettings lens = CineCamera.Lens;
            lens.FieldOfView = Mathf.Lerp(
                lens.FieldOfView,
                _targetFOV,
                Time.unscaledDeltaTime * _zoomLerpSpeed
            );
            CineCamera.Lens = lens;

            _thirdPersonFollow.ShoulderOffset = Vector3.Lerp(
                 _thirdPersonFollow.ShoulderOffset,
                 _targetOffset,
                 Time.unscaledDeltaTime * _offsetLerpSpeed
             );
        }
    }

    /// <summary>
    /// 줌인 기능 및, 오프셋변경
    /// </summary>
    /// <param name="fovAmount"> 줌 크기</param>
    /// <param name="zoomtime"> 줌까지 걸리는 시간</param>
    /// <param name="offsetX"> 오프셋 X 거리</param>
    /// <param name="offsettime"> 오프셋 바뀌는 시간</param>
    public void SetZoom(float fovAmount, float zoomtime, Vector3 offset, float offsettime)
    {
        _targetFOV = fovAmount;
        _targetOffset = offset;
        _zoomLerpSpeed = 1f / Mathf.Max(zoomtime, 0.01f);
        _offsetLerpSpeed = 1f / Mathf.Max(offsettime, 0.01f);
    }

    public void ResetZoom(float zoomtime, float offsettime)
    {
        _targetFOV = defaultFOV;
        _targetOffset = defaultOffset;
        _zoomLerpSpeed = 1f / Mathf.Max(zoomtime, 0.01f);
        _offsetLerpSpeed = 1f / Mathf.Max(offsettime);
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}