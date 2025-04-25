using UnityEngine;
using UnityEngine.AI; //내비메시 관련 코드
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // 생성할 아이템
    public Transform platerTransform;
    
    public float maxDistance = 5f;// 플레이어 위치에 아이템이 배치될 반경

    public float timeBetSpawnMax = 7f; // 최대 시간 간격
    public float timeBetSpawnMin = 2f; // 최소 시간간격
    private float timeBetSpawn;

    private float lastSpawnTime; // 마지막 생성 시점
    void Start()
    {
        //생성 간격과 마지막 생성 시점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;

    }

    // 주기적으로 아이템 생성처리
    void Update()
    {
        //현재 시점이 마지막 생성시점에서 생성 주기 이상 지남
        //&& 플레이어 캐릭터가 존재함
        if (Time.time >= lastSpawnTime + timeBetSpawn && platerTransform)
        {
            //마지막 생성시간 갱신
            lastSpawnTime = Time.time;
            //생성 주기를 랜덤으로 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // 아이템 생성 실해
            Spawn();
        }
    }
    // 실제 아이템 생성처리
    private void Spawn()
    {
        //플레이어 근처에서 내비 메시 위의 랜덤 위치 가져오기
        Vector3 spawnPosition = GetRandomPointOnNavMesh(platerTransform.position, maxDistance);
        //바닥에서 0.5만큼 위로올리기
        spawnPosition += Vector3.up * 0.5f;
        
        // 아이템중 하나를 무작위로 골라 랜덤위치 생성
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);
        
        Destroy(item, 5f);
    }
    //내비메시 위의 랜덤한 위치를 반환하는 메서드
    //center를 중심으로 distance 반경 안에서의 랜덤한위치를 찾음
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        NavMeshHit hit;
        
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);
        
        return hit.position;
    }
}

