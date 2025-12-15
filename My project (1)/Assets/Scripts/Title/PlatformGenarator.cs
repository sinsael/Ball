using UnityEngine;
using System.Collections.Generic;

public class PlatformGenarator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab; // 청크 프리팹
    [SerializeField] int startChunkCount = 5; // 시작 청크 개수
    [SerializeField] Transform chunckParent; // 청크 부모 오브젝트
    [SerializeField] float chunkLength = 10f; // 청크 길이
    [SerializeField] float moveSpeed = 5f; // 청크 이동 속도

    List<GameObject> chunks = new List<GameObject>(); // 청크 리스트

    void Start()
    {
        SpawnChunks();
    }

    private void Update()
    {
        MoveChunk();
    }

    // 청크 스폰
    void SpawnChunks()
    {
        for (int i = 0; i < startChunkCount; i++)
        {
            StartSpawnChunk();
        }
    }

    // 한 개의 청크 스폰
    void StartSpawnChunk()
    {
        float spawnZ = CalculateSpawnZ();

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnZ);
        GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunckParent);

        chunks.Add(newChunk);
    }

    // 다음 청크의 Z 위치 계산
    float CalculateSpawnZ()
    {
        float spawnZ = 0f;

        if (chunks.Count == 0)
        {
            spawnZ = transform.position.z;
        }
        else
        {
            spawnZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnZ;
    }

    // 청크 이동
    void MoveChunk()
    {
        foreach (var chunk in chunks)
        {
            chunk.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }

        // 지나간 청크 제거
        if (chunks.Count > 0)
        {
            GameObject firstChunk = chunks[0];
            if (firstChunk.transform.position.z <= Camera.main.transform.position.z - 80f)
            {
                chunks.RemoveAt(0);
                Destroy(firstChunk);
                StartSpawnChunk();
            }
        }
    }
}
