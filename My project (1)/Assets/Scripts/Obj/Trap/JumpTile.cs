using UnityEngine;
using DG.Tweening;

public class JumpTile : MonoBehaviour
{
    [Header("점프 설정")]
    [Tooltip("점프하는 힘 (20 이상 추천)")]
    public float jumpForce = 20f;

    [Header("연출 설정")]
    [Tooltip("밟았을 때 살짝 찌그러지는 연출 여부")]
    public bool useVisualEffect = true;

    // 원래 크기를 저장할 변수
    private Vector3 originalScale;

    // 중복 작동 방지용 쿨타임
    private bool isCoolTime = false;

    public void Start()
    {
        originalScale = transform.localScale;
    }

    // 바닥(Collision) 감지
    private void OnCollisionEnter(Collision collision)
    {
        TryJump(collision.collider);
    }

    private void TryJump(Collider other)
    {
        if (isCoolTime || !other.CompareTag("Player")) return;

        if (other.TryGetComponent<BallMainSystem>(out BallMainSystem ball))
        {
            isCoolTime = true;

            ball.ApllyExternalForce(transform.up, jumpForce);

            if (useVisualEffect)
            {
                transform.DOKill();
                transform.localScale = originalScale;

                transform.DOPunchScale(new Vector3(-0.1f, 0.2f, -0.1f), 0.3f, 10, 1)
                         .OnComplete(() => isCoolTime = false);
            }
            else
            {
                Invoke(nameof(ResetCoolTime), 0.1f);
            }
        }
    }

    void ResetCoolTime()
    {
        isCoolTime = false;
    }

    private void OnDisable()
    {
        transform.DOKill();
        isCoolTime = false;
        // 비활성화될 때도 원래 크기로 복구
        transform.localScale = originalScale;
    }
}