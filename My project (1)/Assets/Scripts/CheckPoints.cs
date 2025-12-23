using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StageGameManager.instance.lastCheckPointPos = transform.position;
        }
    }
}
