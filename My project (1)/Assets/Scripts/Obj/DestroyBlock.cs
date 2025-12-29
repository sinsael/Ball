using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    [Header("설정")]
    [Tooltip("플레이어가 밟았을 때 부서질지 여부")]
    public bool breakByPlayer = true;
    [Tooltip("부서지기 전 대기 시간 (점프할 시간을 벌어줌)")]
    public float destroyDelay = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("help");
        if (breakByPlayer && collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 표면의 방향(Normal) 가져오기
            Vector3 hitNormal = collision.contacts[0].normal;

            Debug.Log($"[디버그] 'Player' 태그 감지됨! | 충돌 방향(Normal): {hitNormal}");

            // 2. 윗면인지 확인 (Y값이 0.7보다 큰지, 아니면 반대인지 확인 필요)
            if (hitNormal.y < -0.7f)
            {
                Debug.Log("--> 위에서 밟음! (조건 통과)");
                Invoke(nameof(Deactivate), destroyDelay);
            }
            else
            {
                Debug.Log("--> 옆이나 아래에서 닿음 (또는 방향이 반대일 수 있음)");
            }
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
