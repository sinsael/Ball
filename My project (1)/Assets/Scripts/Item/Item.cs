using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("아이템 데이터")]
    [Tooltip("아이템의 효과를 정의하는 데이터 소스")]
    public ItemDataSO itemData;


    protected virtual void OnTriggerEnter(Collider other)
    {
        BallMovement ball = other.GetComponent<BallMovement>();

        if(ball != null)
        {
           // bool isPickedUp = ball.PickUpItem(itemData);

            //// 줍기 성공했다면 이 오브젝트는 삭제 (데이터는 플레이어에게 넘어감)
            //if (isPickedUp)
            //{
            //    Destroy(gameObject);
            //}
        }
    }
}
