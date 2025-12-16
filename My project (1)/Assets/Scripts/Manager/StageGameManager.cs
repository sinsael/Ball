using UnityEngine;

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

    BallInput input;
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
            input = new BallInput();
            Cursor.visible = false;
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
        input.UI.Enable();
    }
    private void OnDisable()
    {
        input.UI.Disable();
    }

    private void Update()
    {
        bool flowControl = PauseState();
        if (!flowControl)
        {
            return;
        }

    }

    private bool PauseState()
    {
        if (currentGameState == GameState.GameClear) return false;
        if (input.UI.Pause.WasPerformedThisFrame())
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

        return true;
    }

    public void ChangeGameState(GameState state)
    {
        if (currentGameState == GameState.GameClear) return;

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
                Cursor.visible = false;
                GameStartUI.SetActive(true);
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Cursor.visible = false;
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Cursor.visible = true;
                GamePauseUI.SetActive(true);
                Time.timeScale = 0.25f;
                break;
            case GameState.GameOver:
                Cursor.visible = true;
                GameOverUI.SetActive(true);
                Time.timeScale = 0.25f;
                break;
            case GameState.GameClear:
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
