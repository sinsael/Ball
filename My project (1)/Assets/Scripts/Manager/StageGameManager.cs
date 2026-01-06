using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    BallMainSystem respawn;
    BallItemSystem ballItem;
    public GameState currentGameState { get; private set; } // 현재 게임상태저장
    public GameState previousGameState; // 전 게임상태 저장

    public Vector3 lastCheckPointPos; // 체크포인트 지점 저장

    [Header("UI연결")]
    [Tooltip("시작")]
    public GameObject GameStartUI = null;
    [Tooltip("일시정지")]
    public GameObject GamePauseUI = null;
    [Tooltip("게임오버")]
    public GameObject GameOverUI = null;
    [Tooltip("게임클리어")]
    public GameObject GameClearUI = null;

    private List<GameObject> stageItems = new List<GameObject>(); // 스테이지 아이템 저장
    private List<GameObject> stageTrap = new List<GameObject>();
    private List<Ball.Button> stageButtons = new List<Ball.Button>();

    private void Awake()
    {
        // 싱글톤
        if (instance == null)
        {
            instance = this;
            // Input Class 생성
            input = new GameInput();
            // 초기 커서 설정
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Destroy(gameObject);
        }

        respawn = FindFirstObjectByType<BallMainSystem>();
        ballItem = FindFirstObjectByType<BallItemSystem>();
    }

    private void Start()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap"); // 아까 만드신 DestroyBlock은 "Trap" 태그를 붙여주세요!
        Ball.Button[] buttons = FindObjectsByType<Ball.Button>(FindObjectsSortMode.None);

        stageItems.AddRange(items);
        stageTrap.AddRange(traps);
        stageButtons.AddRange(buttons);

        CloseAllUI();
        ChangeGameState(GameState.Start); // 게임 시작 상태로 변경

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) lastCheckPointPos = player.transform.position;
    }


    private void OnEnable()
    {
        input.Game.Enable();
        input.Game.Pause.performed += PauseState;
    }
    private void OnDisable()
    {
        input.Game.Disable();
        input.Game.Pause.performed -= PauseState;

        input.Dispose();
    }

    // 일시정지 상태 전환
    private void PauseState(InputAction.CallbackContext context)
    {
        if (currentGameState == GameState.GameClear) return; // 게임 클리어 상태에서는 일시정지 불가

        if (input.Game.Pause.WasPerformedThisFrame())
        {
            if (currentGameState == GameState.Paused) // 일시정지 상태라면 이전 상태로 복귀
            {
                ChangeGameState(previousGameState); // 이전 상태로 복귀
            }
            else
            {
                previousGameState = currentGameState; // 이전 상태 저장
                ChangeGameState(GameState.Paused); // 일시정지 상태로 변경
            }
        }
    }

    public void ChangeGameState(GameState state)
    {
        if (currentGameState == GameState.GameClear) return; // 게임 클리어 상태에서는 상태 변경 불가
        if (currentGameState == state) return; // 동일 상태라면 무시

        if (currentGameState == GameState.Paused && state == GameState.GameOver)
        {
            if (previousGameState != GameState.GameOver)
            {
                previousGameState = GameState.GameOver;
                return;
            }
        }

        currentGameState = state; // 현재 상태 변경

        CloseAllUI(); // 모든 UI 닫기

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

    // 리스폰 시 호출할 함수
    public void ResetStageElements()
    {
        // 1. 비활성화된 모든 아이템을 다시 활성화
        foreach (GameObject item in stageItems)
        {
            if (item != null) item.SetActive(true);
        }
        foreach (GameObject trap in stageTrap)
        {
            if (trap != null) trap.SetActive(true);
        }
        foreach (var btn in stageButtons)
        {
            if (btn != null) btn.ResetButton();
        }
    }

    void CloseAllUI()
    {
        GameStartUI?.SetActive(false);
        GamePauseUI?.SetActive(false);
        GameOverUI?.SetActive(false);
        GameClearUI?.SetActive(false);

        if (Option_UI.instance != null) Option_UI.instance.gameObject.SetActive(false);
        if (Comming_Soon_Popup.instance != null) Comming_Soon_Popup.instance.gameObject.SetActive(false);
    }

    public void RequestRespawn()
    {
        ResetStageElements();


        ballItem.ResetItemState();
        respawn.Respawn(lastCheckPointPos);
        ChangeGameState(GameState.Playing);
    }
}