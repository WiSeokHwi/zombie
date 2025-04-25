using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 유아이에 ㅈ즉시 접근하고 변경할 수 있도록 허용하는 UI매니저
public class UIManager : MonoBehaviour
{
    //싱글턴 접근근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<UIManager>();
            }
            return m_instance;
        }
        
    }
    private static UIManager m_instance;

    public Text ammoText; //탄알 표시용 텍스트
    public Text scoreText;// 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public GameObject gameOverUI; //게임 오버시 활성화 할 UI
    
    //탄알 텍스트 갱신 
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }
    
    //점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score :" + newScore;
    }
    
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "wave :" + waves + "\nEnemy Left : " + count;
    }

    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
