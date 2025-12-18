using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    GameInput input;

    private void Awake()
    {
        input = new GameInput();
    }

    private void OnEnable()
    {
        input.Game.Enable();
    }

    private void OnDisable()
    {
        input.Game.Disable();
    }

    void Update()
    {
        if (StageGameManager.instance.currentGameState == GameState.Paused) return;
        if (input.Game.Pause.WasPressedThisFrame()) return;

        if (input.Game.Anykey.WasPressedThisFrame())
            Restart();

    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StageGameManager.instance.ChangeGameState(GameState.Start);
    }
}
