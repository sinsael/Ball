using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    None,
    Start,
    Playing,
    Paused,
    GameOver,
    GameClear
}

public class StageGameManager : MonoBehaviour
{
    public static StageGameManager instance;

    GameInput input;
    public GameState currentGameState { get; private set; }
    public GameState previousGameState;
    [Header("UI¿¬°á")]
    public GameObject GameStartUI = null;
    public GameObject GamePauseUI = null;
    public GameObject GameOverUI = null;
    public GameObject GameClearUI = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            input = new GameInput();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CloseAllUI();
        ChangeGameState(GameState.Start);
    }

    private void OnEnable()
    {
        input.Game.Enable();
        input.Game.Pause.performed += PauseState;
    }
    private void OnDisable()
    {
        input.Game.Disable();
        input.Game.Pause.performed -=  PauseState;
    }

    private void PauseState(InputAction.CallbackContext context)
    {
        if (currentGameState == GameState.GameClear) return;

        if (input.Game.Pause.WasPerformedThisFrame())
        {
            if (currentGameState == GameState.Paused)
            {
                ChangeGameState(previousGameState);
            }
            else
            {
                previousGameState = currentGameState;
                ChangeGameState(GameState.Paused);
            }
        }
    }

    public void ChangeGameState(GameState state)
    {
        if (currentGameState == state) return;

        if (currentGameState == GameState.Paused && state == GameState.GameOver)
        {
            previousGameState = GameState.GameOver;

            return;
        }
        currentGameState = state;

        CloseAllUI();

        switch (currentGameState)
        {
            case GameState.None:
                break;
            case GameState.Start:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameStartUI.SetActive(true);
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GamePauseUI.SetActive(true);
                Time.timeScale = 0.25f;
                break;
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameOverUI.SetActive(true);
                Time.timeScale = 1;
                break;
            case GameState.GameClear:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameClearUI.SetActive(true);
                Time.timeScale = 1f;
                break;
            default:
                break;

        }

    }

    void CloseAllUI()
    {
        GameStartUI?.SetActive(false);
        GamePauseUI?.SetActive(false);
        GameOverUI?.SetActive(false);
        GameClearUI?.SetActive(false);
    }
}
