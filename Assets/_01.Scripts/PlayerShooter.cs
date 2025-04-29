using UnityEngine;
using UnityEngine.PlayerLoop;


// 주어진 gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생학 ik를 이용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
    public Gun gun; // 총 오브젝트
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손 위치할곳
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손 위치할곳
    
    private PlayerInput playerInput; // 플레이어 입력
    private Animator playerAnimator; // 플레이어 애니메이터 컴포넌트
    
    void Start()
    {
        //사용할 검포넌트 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true); // 총 오브젝트 활성화
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false); // 총 오브젝트 비활성화
    }

    private void Update()
    {
        // 총이 비활성화 상태라면 아무것도 하지 않음
        if (gun == null || !gun.gameObject.activeSelf)
            return;
        // 발사 입력 감지
        if (playerInput.fire)
        {
            // 총을 쏘는 함수 호출
            gun.Fire();
            
        }
        // 재장전 입력 감지
        if (playerInput.reload)
        {
            // 재장전 함수 호출
            if (gun.Reload())
            {
                // 재장전 애니메이션 재생
                playerAnimator.SetTrigger("Reload");
            }
           
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gun != null && UIManager.instance != null)
        {
            //UI 매니저의 탄알 텍스트에 탄창의 탄알과 남은 ㅊ전체 탄알 표시
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 소잡이에 맞춤
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0F);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0F);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);


        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0F);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0F);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
