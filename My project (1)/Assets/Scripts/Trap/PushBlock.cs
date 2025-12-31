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
        rb.isKinematic = true;

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
        // 1. �۵� ���̸� ��� ����
        if (isActive || !collision.CompareTag(playerTag)) return;
        isActive = true;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(rb.DOMove(targetVector, moveTime).SetEase(Ease.OutQuad));
        sequence.AppendInterval(delayTimer);
        sequence.Append(rb.DOMove(defaultVector, moveTime).SetEase(Ease.OutQuad));

        sequence.OnComplete(() => {
            isActive = false;
        });
    }

    private void OnDisable()
    {
        transform.DOKill();
        isActive = false;
    }
}