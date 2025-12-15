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
        if (Input.GetKeyDown(KeyCode.Escape))
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
                GameStartUI.SetActive(true);
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                GamePauseUI.SetActive(true);
                Time.timeScale = 0.25f;
                break;
            case GameState.GameOver:
                GameOverUI.SetActive(true);
                Time.timeScale = 0.25f;
                break;
            case GameState.GameClear:
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
