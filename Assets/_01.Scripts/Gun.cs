using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;

public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State
    {
        Ready, //발사 준비됨
        Empty, // 탄창이 빔
        Reloading, //재장전 중
    }
    
    public State state { get; private set; } //현재 총의 상태
    
    public Transform fireTransform; // 탄알 발사 위치
    
    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect;
    
    private LineRenderer bulletLineRenderer;
    
    private AudioSource gunAudioPlayer; //총 소리 재생기

    public GunData gunData; // 총 현재 데이터
    
    private float fireDistance = 10f; //사정거리
    
    public int ammoRemain; // 현재 탄장에 남아 있는 탄알
    public int magAmmo; // 현재 탄창에 남아있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake()
    {
        //사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer=GetComponent<AudioSource>();
        bulletLineRenderer=GetComponent<LineRenderer>();
        
        //사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인렌더러를 비활성화
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        //총기 상태 초기화
        // 전체 예비 탄알 양 초기화
        ammoRemain = gunData.startAmmoRemain;
        // 현재 탄창을 가득 채우기
        magAmmo = gunData.magCapacity;
        
        // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        state = State.Ready;
        // 마지막으로 총을 쏜 시점을 초기호
        lastFireTime = 0;
    }

    //발사 시도
    public void Fire()
    {
        // 현재 상태가 발사 가능한 생태
        // && 마지막 ㅊㅇ 발사 시점에서 gunData.timeBetFire 이상의 시간이 지남
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            // 마지막 총 방사 시점 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
            Shot();
        }
    }
    
    //실제 발사처리
    private void Shot()
    {
        // 레이케스트에 의한 충동 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 탄알이 맞은 곳을 저장할 변수
        Vector3 hitPoint = Vector3.zero;
        
        
        
        // 레이케스트 ( 시작 시점, 방향, 충동정보 컨테이너, 사정거리)
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            //레이가 어떤 물체와 출돌한 경우
            
            // 충돌한 상대방으로부터IDamageable 오브젝트 가져오기 시도
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            
            // 충돌한 상대방으로부터 IDamageable 오브젝트를 가져오는데 성공 했다면

            if (target != null)
            {
                //상대방의 OnDamage 함수를 실행시켜 상대방에게 데미지 주기
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
            
            hitPoint = hit.point;
            
        }
        else
        {
            //레이가 다른물체와 출돌하지 않았다면
            //탄알이 최대 사정거리까지 날아갔을때의 위치를 충동위치로 사용
            hitPoint = fireTransform.position + fireTransform.forward * fireDistance;
        }
        
        // 발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPoint));
        
        //남은 탄알 수를 -1 
        magAmmo--;
        if (magAmmo <= 0)
        {
            //탄창에 남은 탄알이 없다면 총의 현재 상태를 Empty로 갱신
            state =State.Empty;
        }

    }
    
    //발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        //총구 화연 효과 재생
        muzzleFlashEffect.Play();
        //탄피배출 효과 재생
        shellEjectEffect.Play();
        
        //총격소리 재생 
        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        
        //선의 시작점은 총구 의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        //선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        //라인 렌더러를 활성화 하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;
        
        //0.03초 동안 잠시 대기
        yield return new WaitForSeconds(0.03f);
        
        // 라인 렌더러를 비활성화 하여 탄알 궤적을 지움\
        bulletLineRenderer.enabled = false;
    }
    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            // 이미 재장전 중이거나 남은 탄알이 없거나
            // 탄창에 탄알이 이미 가득한 경우 재장전 할 수 없음
            return false;
        }
        
        //재장전 처리 시작
        StartCoroutine(ReloadRoutine());
        return true;
    }
    
    //실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        // 재장전 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
        
        // 재장전 소요 시간만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);
        
        // 탄창에 채워야할 탄알 계산
        int ammoToFill = gunData.magCapacity - magAmmo;
        //탄창에 채워야할 탄알이 남은 탄알보다 많다면
        // 채워야할 탄알 수를 남은 탄알수에 맞춰 줄입
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }
        
        //탄창을 채움
        magAmmo += ammoToFill;
        //남은 탄알에서 탄창을 채운만큼 탄알을 뺌
        ammoRemain -= ammoToFill;
        UIManager.instance.ammoText.text = magAmmo + "/" + ammoRemain;
        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}
