using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<BallMovement>(out BallMovement ball);
        if (ball != null)
        {
            StageGameManager.instance.ChangeGameState(GameState.GameClear);
        }
    }
}
