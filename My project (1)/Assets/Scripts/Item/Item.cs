using UnityEngine;

public class Item : MonoBehaviour
{
    public int point;
    [SerializeField] protected string playerTag = "Player";

    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            Debug.Log($"Item Collected! +{point} points");
            Destroy(gameObject);
        }
    }
}
