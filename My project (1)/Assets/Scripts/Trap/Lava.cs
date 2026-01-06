using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BallMovement>(out BallMovement ball))
        {
            ball.Die();
        }
    }
}
