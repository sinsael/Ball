using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{

    void Update()
    {
        if (StageGameManager.instance.currentGameState == GameState.Paused) return;
        if (Input.GetKeyDown(KeyCode.Escape)) return;

        if (Input.anyKeyDown)
            Restart();

    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StageGameManager.instance.ChangeGameState(GameState.Start);
    }
}
