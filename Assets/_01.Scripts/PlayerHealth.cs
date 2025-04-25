using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI; // UI관련 코드
public class PlayerHealth : LivingEntity
{
    private static readonly int DieHash = Animator.StringToHash("Die");
    
    public Slider healthSlider; //체렫을 표시할 ui슬라이더

    public AudioClip deathClip; // 사망소리
    public AudioClip hitClip; // 피격소리
    public AudioClip itemPickupClip; // 아이템 습득소리
    
    private AudioSource playerAudio;// 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어 애니메이터
    
    private PlayerMovement playerMovement;// 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        
    }

    protected override void OnEnable()
    {
        // LivingEntity 의 OnEnable() 실행 (상태 초기화 )
        base.OnEnable();
        
        // 체력 슬라이더 활성화
        healthSlider.gameObject.SetActive(true);
        // 체력 슬라이더의 최댓값을 기본 체력값으로 변경
        healthSlider.maxValue = startingHealth;
        // 체력슬라이더의 값을 현재 체력값으로 변경
        healthSlider.value = health;
        
        //플레이어 조작을 받는 컴포넌트 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivineEntity의 ResttoreHealth() 실행 (체력증가)
        base.RestoreHealth(newHealth);
        //갱신된 체력으로 체력 슬라이더를 갱신
        healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!dead)
        {
            // 사망하지 않은 경우에만 효과음 재생
            playerAudio.PlayOneShot(hitClip);
        }
        
        Debug.Log("데미지 : "+damage);
        //LivingEntity의 OnDamage() 실행 (대미지 적용)
        base.OnDamage(damage, hitPoint, hitNormal);
        // 갱신된 체력을 체력 슬라이더에 반영
        healthSlider.value = health;
    }

    public override void Die()
    {
        // LivingEntity Die() 실행 (사망 적용)
        base.Die();
        
        //체력 슬라이더 비활성화
        healthSlider.gameObject.SetActive(false);
        
        //사망음 재생
        playerAudio.PlayOneShot(deathClip);
        // 애니매이터의 Die 트리거를 방동시켜 사망 애니메이션 재생
        playerAnimator.SetTrigger(DieHash);
        
        //플레이어 조작을 받는 컴포넌트 비활성화
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        // 사망하지 않은경우에만 아이템 사용가능
        if (!dead)
        {
            // 충돌한상대방으로부터 IItem 컴포넌트 가져오기 시도
            Iitem item = other.GetComponent<Iitem>();
            
            // 충돌한 상대방으로부터 IItem 컴포넌트를 가져오는데 성공 했다면
            if (item != null)
            {
                // Use 메서드를 실행하여 아이템 사용
                item.Use(gameObject);
                // 아이템 습득 소리 재생
                playerAudio.PlayOneShot(itemPickupClip);
            }
        }
    }
}
