using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Ball>(out Ball ball);
        {
            ball.Die();
        }
    }
}
