using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "InputHandler", menuName = "Input/InputHandler")]
public class InputHandler : ScriptableObject
{
    GameInput input;

    // 다른 스크립트들이 가져다 쓸 수 변수들
    public Vector3 MoveDirection { get; private set; }
    public Vector3 MouseDelta { get; private set; }

    // 아이템 사용처럼 '한 번 클릭'하는 건 이벤트로 처리하면 편합니다.
    public event Action OnUseItemEvent;
    public event Action OnUseItemCancledEvent;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        input = new GameInput();

        // 움직임
        input.Ball.Move.performed += ctx => MoveDirection = ctx.ReadValue<Vector3>();
        input.Ball.Move.canceled += ctx => MoveDirection = Vector3.zero;

        // 아이템 사용
        input.Ball.UseItem.performed += ctx => OnUseItemEvent?.Invoke();
        input.Ball.UseItem.canceled += ctx => OnUseItemCancledEvent?.Invoke();

        // 카메라 이동
        input.Camera.Look.performed += ctx => MouseDelta = ctx.ReadValue<Vector2>();
        input.Camera.Look.canceled += ctx => MouseDelta = Vector2.zero;

        input.Ball.Enable();
        input.Camera.Enable();
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.Ball.Disable();
            input.Game.Disable();
            input.Dispose(); // 메모리 해제
            input = null;
        }
    }
}