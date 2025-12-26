using UnityEngine;
using DG.Tweening;

public class MoveTile : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 Move;
    public string playerTag = "Player";
    public float moveTime = 0.5f;
    public float delayTimer = 0.2f;

    Vector3 defaultVector;
    Vector3 targetVector;

    bool isActive = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 중요: 물리 충돌 떨림 방지

        defaultVector = transform.position;
        targetVector = defaultVector + Move;
    }

    private void OnTriggerEnter(Collider collision)
    {
        TryActivateTrap(collision);
    }

    private void OnTriggerStay(Collider collision)
    {
        TryActivateTrap(collision);
    }

    private void TryActivateTrap(Collider collision)
    {
        // 1. 작동 중이면 즉시 차단
        if (isActive || !collision.CompareTag(playerTag)) return;

        // 2. [가장 중요] 동작 시작 전, 즉시 잠금! (이제 중복 실행 안 됨)
        isActive = true;

        Sequence sequence = DOTween.Sequence();

        // 부드러운 움직임 추가 (OutQuad)
        sequence.Append(rb.DOMove(targetVector, moveTime).SetEase(Ease.OutQuad));
        sequence.AppendInterval(delayTimer);
        sequence.Append(rb.DOMove(defaultVector, moveTime).SetEase(Ease.OutQuad));

        // 3. 모든 동작이 끝나면 잠금 해제
        sequence.OnComplete(() => {
            isActive = false;
        });
    }

    // [안전장치 추가] 
    // 게임 재시작이나 씬 이동 등으로 오브젝트가 꺼질 때 트윈을 정리해줍니다.
    private void OnDisable()
    {
        transform.DOKill(); // 현재 진행 중인 모든 트윈 강제 종료
        isActive = false;   // 상태 초기화
    }
}