using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BallMainSystem>(out BallMainSystem ball))
        {
            ball.Die();
        }
    }
}
