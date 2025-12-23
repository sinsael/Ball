using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("아이템 데이터")]
    [Tooltip("아이템의 효과를 정의하는 데이터 소스")]
    public ItemDataSO itemData;


    protected virtual void OnTriggerEnter(Collider other)
    {
        BallItemSystem ballSystem = other.GetComponent<BallItemSystem>();

        if(ballSystem != null)
        {
           ballSystem.PickUpItem(itemData);

           gameObject.SetActive(false);
        }
    }
}
