using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class BallMovement : MonoBehaviour
{
    [Header("컴포넌트")]
    Rigidbody rb;
    GameInput input;
    MeshRenderer meshRenderer;
    BallItemSystem itemSystem;

    [Header("카메라")]
    [Tooltip("방향 참조용")]
    public Transform cameraTransform;
    [Tooltip("카메라 줌 인/아웃 용")]
    public CinemachineCameraController cameraContorller;

    [Header("볼 상세설정")]
    public LayerMask groundLayer;

    [Tooltip("움직임")]
    public float bounceForce = 5f; // 공 튕기는 힘
    [Tooltip("벽이나 천장에 닿았을 때 튕기는 힘")]
    public float wallBounceForce = 5f;
    public float moveSpeed = 5f; // 공 이동 속도
    Vector3 MoveDirection;

    [Tooltip("관성")]
    [SerializeField] float inertia = 0.1f; // 관성 감속
    Vector3 currentVelocityRef; // 관성 속도 참조

    bool cantMove =>

        StageGameManager.instance.currentGameState == GameState.GameOver ||

        StageGameManager.instance.currentGameState == GameState.Paused ||

        StageGameManager.instance.currentGameState == GameState.GameClear;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        input = new GameInput();
        itemSystem = GetComponent<BallItemSystem>();

        if (cameraContorller == null)
        {
            cameraContorller = FindFirstObjectByType<CinemachineCameraController>();
        }
    }

    private void OnEnable()
    {
        input.Ball.Enable();
        input.Ball.Move.performed += ctx => MoveDirection = ctx.ReadValue<Vector3>();
        input.Ball.Move.canceled += ctx => MoveDirection = Vector3.zero;
    }
    private void OnDisable()
    {
        input.Ball.Disable();
        input.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        if (cantMove) return;
    }

    void FixedUpdate()
    {
        if (cantMove) return;

        Vector3 dir = MoveDirection;

        movement(dir);
    }

    // 공 이동
    void movement(Vector3 moveinput)
    {
        Debug.Log(moveinput);

        // 이동 방향 벡터 정규화
        Vector3 clampedMoveDir = Vector3.ClampMagnitude(moveinput, 1f);

        // 카메라 기준 이동 방향 변환
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // 카메라 수평 방향만 사용
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 최종 이동 방향 계산
        Vector3 targetMoveDir = (camForward * clampedMoveDir.z) + (camRight * clampedMoveDir.x);

        // 이동 방향 벡터 생성

        Vector3 movement = targetMoveDir * moveSpeed;

        // 현재 수평 속도 벡터 생성

        Vector3 newVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // 부드러운 관성 이동
        Vector3 newHorizontalVelocity = Vector3.SmoothDamp(
            newVelocity,
            movement,
            ref currentVelocityRef,
            inertia

        );

        // 최종 속도 적용 (수평 + 수직)

        rb.linearVelocity = new Vector3(
    newHorizontalVelocity.x,
    rb.linearVelocity.y, // 이 값을 강제로 0으로 만들지 않는 것이 핵심!
    newHorizontalVelocity.z
        );
    }

    // [추가됨] 벽이나 천장(옆면/아랫면)에 '쾅' 하고 부딪혔을 때 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 땅 레이어인지 확인
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            ContactPoint contact = collision.contacts[0];

            // 법선 벡터(Normal)의 y값이 0.7 이하라면 (즉, 윗면이 아니라면)
            if (contact.normal.y <= 0.7f)
            {
                // 벽이 밀어내는 방향(Normal)으로 힘을 '팍' 줍니다.
                // ForceMode.Impulse는 순간적인 힘을 가할 때 적합합니다.
                rb.AddForce(contact.normal * wallBounceForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 충돌한 물체의 레이어가 groundLayer에 포함되어 있는지 비트 연산으로 확인
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            ContactPoint contact = collision.contacts[0];

            if (contact.normal.y > 0.7f)
            {
                Vector3 currentVel = rb.linearVelocity;

                // 점프 로직 (기존 유지)
                if (currentVel.y < bounceForce)
                {
                    rb.linearVelocity = new Vector3(currentVel.x, bounceForce, currentVel.z);
                }
            }
        }
    }

    // 다른 방식의 속도 추가
    public void ApllyExternalForce(Vector3 direction, float force)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    // 리스폰
    public void Respawn(Vector3 targetPos)
    {
        // 움직임 초기화
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentVelocityRef = Vector3.zero;
        itemSystem.currentItemData = null;

        // 리스폰 구간
        rb.position = targetPos;
        transform.position = targetPos;

        rb.interpolation = RigidbodyInterpolation.None;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // 렌더러 활성화
        meshRenderer.enabled = true;
        rb.isKinematic = false;
    }

    // 죽음
    public void Die()
    {
        // 움직임 초기화
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        currentVelocityRef = Vector3.zero;

        // 게임 상태 변화
        StageGameManager.instance.ChangeGameState(GameState.GameOver);

        // 컴포넌트 비활성화
        meshRenderer.enabled = false;
    }
}