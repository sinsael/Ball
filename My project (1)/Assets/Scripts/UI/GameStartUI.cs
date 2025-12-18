using UnityEngine;

public class GameStartUI : MonoBehaviour
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
    private void Update()
    {
        if (input.Game.Pause.WasPressedThisFrame()) return;

        if (input.Game.Anykey.WasPerformedThisFrame())
            StageGameManager.instance.ChangeGameState(GameState.Playing);
    }
}
