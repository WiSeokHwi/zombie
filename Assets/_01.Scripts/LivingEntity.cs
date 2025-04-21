using UnityEngine;
using System;
// 생명체로 동작할 게임 오브젝트들을 위한 뼈대를 제공
//체력 대미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 여부

    public event Action onDeath; // 사망 이벤트
    
    //생명체가 활성화 될 때 상태를 리셋
    protected virtual void OnEnable()
    {
        dead = false; // 사망 여부 초기화
        health = startingHealth; // 체력 초기화
    }

    // 데미지를 입는기능
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead) return; // 이미 사망한 경우 데미지 무시
        health -= damage; // 체력 감소
        if (health <= 0f && !dead)
        {
            Die(); // 사망 처리
        }
    }

    //체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return; // 이미 사망한 경우 회복 무시

        if (startingHealth <= health + newHealth) // 최대 체력보다 클 경우
        {
            newHealth = startingHealth - health; // 최대 체력으로 설정
        }

        //if (startingHealth <= health + newHealth) 
        //{
        //    health = startingHealth; 
        //}

        health += newHealth; // 체력 증가
   
    }

    //사망처리
    public virtual void Die()
    {
        //onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath(); // 사망 이벤트 발생
        }
        dead = true; // 사망 상태로 변경
    }
}


