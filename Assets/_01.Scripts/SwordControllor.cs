using UnityEngine;
using UnityEngine.PlayerLoop;


// 주어진 gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생학 ik를 이용해 캐릭터 양손이 총에 위치하도록 조정
public class SwordControllor : MonoBehaviour
{

    public Transform swordPivot; // 총 배치의 기준점
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



    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        swordPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

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
