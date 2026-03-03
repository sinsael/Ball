using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [Header("이동 설정")]
    // 이제 EndPos를 직접 찍지 않고, "여기서부터 얼마나 이동할지"를 정합니다.
    // 예: (0, 0, 50)이면 Z축으로 50만큼 이동, (0, -10, 0)이면 아래로 10만큼 이동
    [Tooltip("시작점 기준으로 이동할 상대적 거리와 방향")]
    public Vector3 moveOffset = new Vector3(0, 0, 50);

    public float speed = 5f;     // 이동 속도

    [Header("블록 설정")]
    [SerializeField] GameObject chunkPrefab;   // 블록 프리팹
    [SerializeField] int totalChunkCount = 5;  // 화면에 유지할 개수
    [SerializeField] float chunkInterval = 10f; // 블록 간격

    // 내부 변수들
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 moveDirection;
    private List<GameObject> chunks = new List<GameObject>();

    void Start()
    {
        // 1. 시작점과 도착점 계산 (핵심 변경 부분)
        startPos = transform.position;        // 이 스크립트가 붙은 위치가 곧 시작점
        endPos = startPos + moveOffset;       // 도착점 = 시작점 + 이동량(@)

        // 방향 계산 (단위 벡터)
        moveDirection = moveOffset.normalized;

        SpawnInitialChunks();
    }

    void Update()
    {
        MoveChunks();
        CheckAndRelocateChunk();
    }

    // 초기 생성
    void SpawnInitialChunks()
    {
        for (int i = 0; i < totalChunkCount; i++)
        {
            // 움직이는 방향의 '반대쪽'으로 줄을 세워서 생성합니다.
            // 그래야 startPos에서 기차처럼 줄지어 출발하는 모습이 됩니다.
            Vector3 spawnPos = startPos - (moveDirection * chunkInterval * i);

            GameObject newChunk = Instantiate(chunkPrefab, spawnPos, Quaternion.identity, transform);
            chunks.Add(newChunk);
        }
    }

    // 이동 로직
    void MoveChunks()
    {
        foreach (var chunk in chunks)
        {
            // 계산된 방향으로 이동
            chunk.transform.Translate(moveDirection * speed * Time.deltaTime);
        }
    }

    // 도착 확인 및 재배치
    void CheckAndRelocateChunk()
    {
        if (chunks.Count == 0) return;

        GameObject firstChunk = chunks[0];

        // 목표 지점(endPos)과의 거리가 매우 가까워지면 (0.5f 이하)
        if (Vector3.Distance(firstChunk.transform.position, endPos) <= 0.5f)
        {
            GameObject lastChunk = chunks[chunks.Count - 1];

            // 꼬리 블록의 뒤쪽(이동 반대 방향)으로 위치 재설정
            Vector3 newPos = lastChunk.transform.position - (moveDirection * chunkInterval);
            firstChunk.transform.position = newPos;

            // 리스트 순환
            chunks.RemoveAt(0);
            chunks.Add(firstChunk);
        }
    }

    // [보너스] 씬 뷰에서 이동 경로를 눈으로 확인하기 위한 코드
    private void OnDrawGizmos()
    {
        // 플레이 중이 아닐 때도 대략적인 경로를 보여줍니다.
        Vector3 sPos = Application.isPlaying ? startPos : transform.position;
        Vector3 ePos = Application.isPlaying ? endPos : transform.position + moveOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(sPos, ePos); // 시작부터 끝까지 선 그리기
        Gizmos.DrawSphere(ePos, 0.5f); // 도착점에 구 표시
    }
}