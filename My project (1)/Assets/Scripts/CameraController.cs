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

    [Header("설정")]
    [Range(0, 2)]
    [Tooltip("마우스 감도")]
    public float mouseSensitivity = 1.0f;

    [Tooltip("상단 회전 제한")]
    public float topClamp = 70.0f;

    [Tooltip("하단 회전 제한")]
    public float bottomClamp = -10.0f; // ★ 수정됨: 바닥 뚫림 방지 위해 -30 -> -10 추천

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
        // 시작할 때 현재 각도 저장
        Vector3 angles = transform.eulerAngles;
        _targetYaw = angles.y;
        _targetPitch = angles.x;
    }

    void LateUpdate()
    {
        if (StageGameManager.instance.currentGameState == GameState.Paused || StageGameManager.instance.currentGameState == GameState.GameClear) return;
        if (playerBall == null) return;
        if (Mouse.current == null) return;

        // ★ 1. 위치 동기화: 공의 위치를 그대로 따라갑니다.
        // 공이 구르든 말든 위치만 딱 붙어있습니다.
        transform.position = playerBall.position;

        // 2. 마우스 입력 계산
        Vector2 mouseDelta = input.Camera.Look.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            _targetYaw += mouseX;
            _targetPitch -= mouseY;
        }

        // 3. 각도 제한
        _targetPitch = ClampAngle(_targetPitch, bottomClamp, topClamp);

        // ★ 4. 회전 적용: 이 오브젝트(피벗) 자체를 회전시킵니다.
        // 공(playerBall)을 회전시키는 게 아니라 '나 자신'을 돌립니다.
        transform.rotation = Quaternion.Euler(_targetPitch, _targetYaw, 0.0f);
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}