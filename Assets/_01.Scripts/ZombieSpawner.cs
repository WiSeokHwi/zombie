using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviour
{
    public ZombieController zombiePrefab; // 생성할 좀비 원본 프리팹
    
    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터 
    public Transform[] spawnPoints; // 좀비 au를 소환할 위치

    private List<ZombieController> zombies = new List<ZombieController>(); // 생성된 좀비를 담는 리스트
    private int wave; // 현재 웨이브

    void Update()
    {
        //게임 오버 상태일때는 생선 x
        if (GameManager.instance && GameManager.instance.isGameover)
        {
            return;
        }
        
        // 좀비를 모두 물리친 경우 다음 스폰 진행
        if (zombies.Count <= 0)
        {
            SpawnWave();
        }
        
        // UI 갱신 
        UpdateUI();
    }
    // 웨이브 정보를 UI로 표신
    private void UpdateUI()
    {
        //현재 웨이브와 남은 좀비 수 표시
        UIManager.instance.UpdateWaveText(wave,zombies.Count);
    }
    //현재 웨이브에 맞춰 좀비 생성
    private void SpawnWave()
    {
        //웨이브 1 증가
        wave++;
        
        //형재 웨이브 *1.5 를 반올림 한 수만큼 좀비 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        
        // spawnCount만큼 좀비 생성
        for (int i = 0; i < spawnCount; i++)
        {
            //좀비 생성 처리 실행
            CreateZombie();
        }
    }
    // 좀비를 생성하고 좀비에 추격할 대상 할당
    private void CreateZombie()
    {
        // 사용할 좀비 데이터 랜덤으로 결정
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];
        
        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // 좀비 프리팹으로부터 좀비 생성
        ZombieController zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        
        // 생성한 좀비의 능력치 설정
        zombie.Setup(zombieData);
        
        // 생성된 좀비를 리스트에 추가
        zombies.Add(zombie);
        
        //좀비의 onDeath 이벤트에 익명 메서드 등록
        //사망한 좀비를 리스트에서 제거
        zombie.onDeath += () => zombies.Remove(zombie);
        //사망한 좀비를 10 초 뒤에 파괴
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);
        // 좀비 사망시 점수 상승 
        zombie.onDeath += () => GameManager.instance.AddScore(100);
    }
}
