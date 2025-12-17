using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemDataSO itemData;
    [SerializeField] protected string playerTag = "Player";

    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            itemData.Effect();
        }
    }
}
