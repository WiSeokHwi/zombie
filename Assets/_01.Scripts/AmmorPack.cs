using UnityEngine;

public class AmmorPack : MonoBehaviour,Iitem
{
    public int ammo = 30; // 충전할 탄알 수
    public void Use(GameObject target)
    {
        // 전달받은 게임 오브젝트로부터 PlayerShooter 컴포넌트 가져오기 시도
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();
        
        //PlayerShooter 컴포넌트가 있으며 총 오브젝트가 존재하면
        if (playerShooter && playerShooter.gun)
        {
            // 총의 남은 탄알 수를 ammo만큼 더함
            playerShooter.gun.ammoRemain += ammo;
        }
        // 사용 되었으므로 자신을 파괴ㅏ
        Destroy(this.gameObject);
    }
}
