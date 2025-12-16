using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    BallInput input;

    private void Awake()
    {
        input = new BallInput();
    }

    private void OnEnable()
    {
        input.UI.Enable();
    }

    private void OnDisable()
    {
        input.UI.Disable();
    }
    private void Update()
    {
        if (input.UI.Pause.WasPressedThisFrame()) return;

        if (input.UI.Anykey.WasPerformedThisFrame())
            StageGameManager.instance.ChangeGameState(GameState.Playing);
    }
}
