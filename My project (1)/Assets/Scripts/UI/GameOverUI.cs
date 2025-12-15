using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    BallInput input;

    private void OnEnable()
    {
        input.UI.Enable();
    }

    private void OnDisable()
    {
        input.UI.Disable();
    }

    void Update()
    {
        if (StageGameManager.instance.currentGameState == GameState.Paused) return;
        if (input.UI.Pause.WasPressedThisFrame()) return;

        if (input.UI.Anykey.WasPressedThisFrame())
            Restart();

    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StageGameManager.instance.ChangeGameState(GameState.Start);
    }
}
