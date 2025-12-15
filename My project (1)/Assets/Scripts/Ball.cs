using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("컴포넌트")]
    Rigidbody rb;
    BallInput input;
    [Header("카메라")]
    [Tooltip("CinemachineCameraController가 붙은 오브젝트")]
    public Transform cameraTransform;
    [Header("볼 상세설정")]
    [SerializeField] string GroundTag = "Ground"; // 땅감지 태그
    [Tooltip("움직임")]
    public float bounceForce = 5f; // 공 튕기는 힘
    public float moveSpeed = 5f; // 공 이동 속도
    [Tooltip("관성")]
    [SerializeField] float inertia = 0.1f; // 관성 감속
    Vector3 currentVelocityRef; // 관성 속도 참조

    bool cantMove => 
        StageGameManager.instance.currentGameState == GameState.GameOver ||
        StageGameManager.instance.currentGameState == GameState.Paused ||
        StageGameManager.instance.currentGameState == GameState.GameClear;

    Vector3 moveDir; // 이동 방향
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new BallInput();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(cantMove)
        {
            return;
        }

    }

    private void FixedUpdate()
    {
        if (cantMove)
        {
            return;
        }

        movement();
    }

    // 공 이동
    private void movement()
    {
        Debug.Log(moveDir);

        // 카메라 기준 이동 방향 변환
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // 카메라 수평 방향만 사용
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetMoveDir = (camForward * moveDir.z)+ (camRight * moveDir.x);

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

        rb.linearVelocity = new Vector3(
            newHorizontalVelocity.x,
            rb.linearVelocity.y,
            newHorizontalVelocity.z
        );
    }

    private void movementInput()
    {
        input.Ball.Move.performed += ctx => moveDir = ctx.ReadValue<Vector3>();
        input.Ball.Move.canceled += ctx => moveDir = Vector3.zero;
    }

    private void OnEnable()
    {
        input.Enable();
        movementInput();
    }

    private void OnDisable()
    {
        input.Disable();
        movementInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GroundTag))
        {
            ContactPoint contact = collision.contacts[0]; // 충돌 지점 정보

            if (contact.normal.y > 0.7f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // 수직 속도 초기화
            }
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }

    public void Die()
    {
        this.gameObject.SetActive( false );
        StageGameManager.instance.ChangeGameState(GameState.GameOver);
    }
}
