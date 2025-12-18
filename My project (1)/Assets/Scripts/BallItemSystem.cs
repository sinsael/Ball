using System;
using UnityEngine;

public class BallItemSystem : MonoBehaviour
{
    [Header("연결 필요")]
    public BallMovement movement;
    public CinemachineCameraController cameraController;
    public GameObject crosshairUI;

    GameInput input;

    [Header("아이템 상태")]
    [SerializeField] ItemDataSO currentItemData;
    bool isEffectActive = false;
    bool stopItemRequest = false;
    Coroutine activeItemCoroutine;


    private void Awake()
    {
        movement = GetComponent<BallMovement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        input.Ball.Enable();
    }

    private void OnDisable()
    {
        input.Ball.Disable();
    }


    public void SetItem(ItemDataSO newItem)
    {
        currentItemData = newItem;
    }

    void UseItem()
    {
        currentItemData.Effect(this);
    }
}
