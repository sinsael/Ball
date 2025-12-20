using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallItemSystem : MonoBehaviour
{
    [Header("연결 필요")]
    public BallMovement movement;
    public CinemachineCameraController cameraController;
    [Header("조준점")]
    public GameObject crosshairUI;

    GameInput input;

    [Header("아이템 상태")]
    [SerializeField] ItemDataSO currentItemData;
    float currentHoldTimer = 0f;

    bool isHolding = false;

    private void Awake()
    {
        movement = GetComponent<BallMovement>();
        input = new GameInput();

        if(crosshairUI == null )
        {
            return;
        }
    }

    private void OnEnable()
    {
        input.Ball.Enable();
        input.Ball.UseItem.started += OnUseItemStarted;
        input.Ball.UseItem.canceled += OnUseItemCanceled;
    }

    private void OnDisable()
    {
        input.Ball.Disable();
        input.Ball.UseItem.started -= OnUseItemStarted;
        input.Ball.UseItem.canceled -= OnUseItemCanceled;
    }

    private void Update()
    {
        if(isHolding && currentItemData != null)
        {
            currentHoldTimer += Time.unscaledDeltaTime;

            if(currentHoldTimer >= currentItemData.maxHoldTime)
            {
                ExecuteItemUp();
            }
        }
    }

    public void PickUpItem(ItemDataSO newItem)
    {
        // 1. 장착 불가능 아이템 즉시 사용
        if (newItem.isEquippable == false)
        {
            newItem.OnPickup(this);
            return;
        }

        // 이미 아이템이 있다면 삭제
        if (currentItemData != null)
        {
            Debug.Log("아이템이 있어서 새로 먹은 아이템은 소멸했습니다.");
            return;
        }

        // 빈슬롯이라면 장착
        currentItemData = newItem;
        Debug.Log($"아이템 장착: {newItem.itemName}");
    }

    // 마우스를 눌렀을 때
    private void OnUseItemStarted(InputAction.CallbackContext context)
    {
        if (currentItemData == null) return;

        switch (currentItemData.useType)
        {
            case ItemUseType.Instant: // 즉발
                currentItemData.OnUseDown(this);
                currentItemData = null; // 즉시 소모
                break;
            case ItemUseType.Hold: // 홀드
                isHolding = true;
                currentItemData.OnUseDown(this);
                break;
            default:
                break;
        }
    }

    // 좌클릭 뗌 (발사)
    private void OnUseItemCanceled(InputAction.CallbackContext context)
    {
        if (currentItemData == null) return;

        // 홀드형이고, 누르고 있던 상태라면 -> 발사 처리
        if (currentItemData.useType == ItemUseType.Hold && isHolding)
        {
            currentItemData.OnUseUp(this);

            isHolding = false;      // 사용 종료
            currentItemData = null; // 아이템 소모
        }
    }

    void ExecuteItemUp()
    {
        if ((currentItemData == null || !isHolding)) return;

        currentItemData.OnUseUp(this);
        isHolding = false;
        currentItemData=null;
        currentHoldTimer = 0f;
    }
}
