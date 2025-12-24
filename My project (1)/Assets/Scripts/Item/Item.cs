using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("아이템 데이터")]
    [Tooltip("아이템의 효과를 정의하는 데이터 소스")]
    public ItemDataSO itemData;


    protected virtual void OnTriggerEnter(Collider other)
    {
        BallItemSystem ballSystem = other.GetComponent<BallItemSystem>();

        if (ballSystem != null)
        {
            ballSystem.PickUpItem(itemData);

            // 아이템 데이터가 Item_ScoreSO 타입인지 확인합니다.
            if (itemData is Item_ScoreSO)
            {
                // 스코어 아이템은 아예 삭제합니다.
                Destroy(gameObject);
            }
            else
            {
                // 그 외(장비형 등)는 기존처럼 비활성화만 합니다.
                gameObject.SetActive(false);
            }
        }
    }
}
