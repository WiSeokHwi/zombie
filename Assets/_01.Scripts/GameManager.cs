using UnityEditor.Analytics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글턴 변수에 아직 오브젝트가할당되지 않았다면 
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<GameManager>();

            }

            return m_instance;
        }

    }

    private static GameManager m_instance; // 싱글턴이 할당될 static변수
    private int score = 0; // 현재 게임 점수
    public bool isGameover{ get; private set; } 
    // 게임오버 상태
    

    void Awake()
    {
        //씬에 싱글턴 오브젝트가 된 다른 game manager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
       
    }
    
    void Start()
    {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임오버
        FindAnyObjectByType<PlayerHealth>().onDeath += EndGame;
    }
    
    
    // 점수를 추가학 UI 갱신
    public void AddScore(int newScore)
    {
        //게임오버가 아닌 상태에서만 점수 추가가능 
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            //점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    public void EndGame()
    {
        //게임오버 상태를 참으로 변경
        isGameover = true;
        // 게임오버 UI 활성화
        UIManager.instance.SetActiveGameOverUI(true);
    }
}
