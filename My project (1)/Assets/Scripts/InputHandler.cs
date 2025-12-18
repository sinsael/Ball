using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    GameInput input;

    // 다른 스크립트들이 가져다 쓸 수 변수들
    public Vector3 MoveDirection { get; private set; }
    public Vector3 MouseDelta { get; private set; }

    // 아이템 사용처럼 '한 번 클릭'하는 건 이벤트로 처리하면 편합니다.
    public event Action OnUseItemEvent;
    public event Action OnUseItemCancledEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        input = new GameInput();
    }

    private void OnEnable()
    {
        input.Enable();

        // 움직임
        input.Ball.Move.performed += ctx => MoveDirection = ctx.ReadValue<Vector3>();
        input.Ball.Move.canceled += ctx => MoveDirection = Vector3.zero;

        // 아이템 사용
        input.Ball.UseItem.performed += ctx => OnUseItemEvent?.Invoke();
        input.Ball.UseItem.canceled += ctx => OnUseItemCancledEvent?.Invoke();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
