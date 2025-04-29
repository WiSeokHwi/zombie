using UnityEngine;

public class HealthPack : MonoBehaviour, Iitem
{
    
    public float health = 50; // 체력 회복 할 수치
    
    public void Use(GameObject target)
    {
        // 전달받은 게임오브젝트로부터 LivingEntity 컴포넌트 가져오기 시도
        LivingEntity life = target.GetComponent<LivingEntity>();
        
        // LivingEntity 가 있다면
        if (life)
        {
            //체력 회복 실행
            life.RestoreHealth(health);
        }
        //사용 되었으므로 자신을 파괴
        Destroy(this.gameObject);
    }
}
