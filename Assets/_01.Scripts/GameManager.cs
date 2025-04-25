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
                m_instace = FindAnyObjectByType<GameObject>();

            }
        }
        return m_instance;
    }
    
    private stati
    public bool isGameover;
    

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
