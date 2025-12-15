using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("컴포넌트")]
    Rigidbody rb;
    BallInput input;
    [Header("볼 상세설정")]
    [SerializeField] string GroundTag = "Ground"; // 땅감지 태그
    [Tooltip("움직임")]
    public float bounceForce = 5f; // 공 튕기는 힘
    public float moveSpeed = 5f; // 공 이동 속도
    [Tooltip("관성")]
    [SerializeField] float inertia = 0.1f; // 관성 감속
    Vector3 currentVelocityRef; // 관성 속도 참조

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
    }

    private void FixedUpdate()
    {
        movement();
    }

    // 공 이동
    private void movement()
    {
        Debug.Log(moveDir);
        // 이동 방향 벡터 생성
        Vector3 movement = new Vector3(moveDir.x, 0, moveDir.z) * moveSpeed;
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
}
