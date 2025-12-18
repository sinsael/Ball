using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<BallMovement>(out BallMovement ball);
        {
            ball.Die();
        }
    }
}
