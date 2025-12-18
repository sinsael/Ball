using System;
using UnityEngine;

public class BallItemSystem : MonoBehaviour
{
    [Header("연결 필요")]
    [SerializeField] BallMovement movement;
    [SerializeField] CinemachineCameraController cameraController;
    [SerializeField] GameObject crosshairUI;
    InputHandler inputHandler;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
