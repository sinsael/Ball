using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StageGameManager.instance.lastCheckPointPos = transform.position + Vector3.up;
        }
    }
}
