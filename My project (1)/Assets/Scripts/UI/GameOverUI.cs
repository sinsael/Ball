using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    BallMainSystem respawn;
    GameInput input;

    private void Awake()
    {
        input = new GameInput();
        if(respawn == null)
        {
            respawn = FindAnyObjectByType<BallMainSystem>();
        }
    }

    private void OnEnable()
    {
        input.Game.Enable();
        respawn = FindAnyObjectByType<BallMainSystem>();
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
            StageGameManager.instance.RequestRespawn();
    }


}
