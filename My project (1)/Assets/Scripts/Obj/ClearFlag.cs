using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<BallMainSystem>(out BallMainSystem ball);
        if (ball != null)
        {
            StageGameManager.instance.ChangeGameState(GameState.GameClear);
        }
    }
}
