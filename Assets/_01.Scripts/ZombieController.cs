using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : LivingEntity
{
    public LayerMask whatIsTarget;// 추적 대상 레이어

    private LivingEntity targetEntity; // 추적 대상
    private NavMeshAgent navMeshAgent; // 경로 계산 ai 에이전트
    
    public ParticleSystem hitEffect; // 피격시 재생할 파티클효과
    public AudioClip deathSound;
    public AudioClip hitSound;
    
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;
    private Renderer zombieRenderer;// 렌더러 컴포넌트
    
    public float damage = 20f; //공격력
    public float timeBetweenAttacks = 0.5f; // 공격 간격
    private float lastAttackTime; //마지막 공격시점

    //추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            //추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            
            //그렇지 않다면
            return false;
        }
    }

    void Awake()
    {
        zombieAnimator = GetComponent<Animator>();
        zombieRenderer = GetComponentInChildren<Renderer>();
        zombieAudioSource = GetComponent<AudioSource>();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    public void Setup(ZombieData zombieData)
    {
        //좀비 Ai 초기 스펙을 결정하는 셋업 메서드
        //체력설정
        startingHealth = zombieData.health;
        damage = zombieData.damage;
        navMeshAgent.speed = zombieData.speed;
        zombieRenderer.material.color = zombieData.skinColor; 
    }
    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    
    void Update()
    {
        // 추적 대상의 존재 여부에 다라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
        
    }
    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        // 살아있는 동안 무한루프
        while (!dead)
        {
            if (hasTarget)
            {
                // 추적 대상 존재  경로를 갱신하고 이동을 계속 진행
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                //추적대상 없음 : AI이동 중지
                navMeshAgent.isStopped = true;
                
                //20유닛의 반지름을 가진 가상의 구를 그렸을때 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링

                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                
                //모든 콜라이더를 순회하면서 살아있는 LivingEntity 찾기
                foreach (Collider col in colliders)
                {
                    // 코라이덜부터 LivingEntity 컴포너트 가져오기
                    LivingEntity livingEntity = col.GetComponent<LivingEntity>();
                    
                    // LivingEntity 가 존재하면, 해당 LivingEntity가 살아있다면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;
                        
                        //for 문 루프 즉시 정지
                        break;
                    }
                }
            }
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }
    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // LivingEntity의 OnDamage() 를 실행하여 대미지 적용
        if (!dead)
        {
            //공격받은 지점과 방향으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            
            //좀비 픽격음 재생
            zombieAudioSource.PlayOneShot(hitSound);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        //LivingEntity 의 Die()실행 하여 기본 사망처리 진행
        base.Die();
        
        //다른 ai를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] zombieColliders = GetComponents<Collider>();
        foreach (Collider col in zombieColliders)
        {
            col.enabled = false;
        }
        
        
        //ai추적을 중지하고 내비메시 컴포넌트 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        
        //사망애니메이션 실행
        zombieAnimator.SetTrigger("Die");
        // 사망 효과음 재생
        zombieAudioSource.PlayOneShot(deathSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        //트리거 충돌한 상대방 오브젝트가 추적대상이라면 공격실행
        //자신이 사망하지 않고
        //최근 공격 시점에서 timeBetAttack 이상 시간이 지났다면
        if (!dead && Time.time >= lastAttackTime + timeBetweenAttacks)
        {
            //상대방의 LivingEntity 타입 가져오기 시도
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            //상대방의 LivingEntity가 자신의 추적 대상이라면 공격실행
            if (attackTarget != null && attackTarget == targetEntity)
            {
                //최근 공격시간 갱신
                lastAttackTime = Time.time;
                
                //상대방의 피격위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNomal = transform.position - other.transform.position;
                
                //공격실행
                attackTarget.OnDamage(damage, hitPoint, hitNomal);
            }
        }
    }
}
