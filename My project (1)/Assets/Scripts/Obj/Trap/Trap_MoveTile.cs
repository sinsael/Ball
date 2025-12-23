using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Trap_MoveTile : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 Move;
    public string playerTag;
    public float moveTime;
    public float delayTimer;
    Vector3 defaultVector;
    Vector3 targetVector;

    bool isActive = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultVector = transform.position;

        targetVector = defaultVector + Move;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // 이미 작동 중이거나 플레이어가 아니면 무시
        if (isActive || !collision.CompareTag(playerTag)) return;

        Sequence sequence = DOTween.Sequence();

        // 목표 위치로 이동
        sequence.Append(rb.DOMove(targetVector, moveTime)).AppendCallback(() =>
        {
            isActive = true;
        });

        // 그 위치에서 잠시 대기
        sequence.AppendInterval(delayTimer);

        // 원래 위치로 복귀
        sequence.Append(rb.DOMove(defaultVector, moveTime));

        // 모든 동작이 끝나면 다시 실행 가능하게 상태 변경
        sequence.OnComplete(() => {
            isActive = false;
        });
    }
}
