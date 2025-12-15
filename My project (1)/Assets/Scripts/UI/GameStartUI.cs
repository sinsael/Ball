using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) return;

        if (Input.anyKeyDown)
            StageGameManager.instance.ChangeGameState(GameState.Playing);
    }
}
