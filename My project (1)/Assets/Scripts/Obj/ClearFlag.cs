using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.TryGetComponent<Ball>(out Ball ball);
        if (ball != null)
        {
            StageGameManager.instance.ChangeGameState(GameState.GameClear);
        }
    }
}
