using UnityEngine;
using System;
// ����ü�� ������ ���� ������Ʈ���� ���� ���븦 ����
//ü�� ����� �޾Ƶ��̱�, ��� ���, ��� �̺�Ʈ�� ����
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f; // ���� ü��
    public float health { get; protected set; } // ���� ü��
    public bool dead { get; protected set; } // ��� ����

    public event Action onDeath; // ��� �̺�Ʈ
    
    //����ü�� Ȱ��ȭ �� �� ���¸� ����
    protected virtual void OnEnable()
    {
        dead = false; // ��� ���� �ʱ�ȭ
        health = startingHealth; // ü�� �ʱ�ȭ
    }

    // �������� �Դ±��
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead) return; // �̹� ����� ��� ������ ����
        health -= damage; // ü�� ����
        if (health <= 0f && !dead)
        {
            Die(); // ��� ó��
        }
    }

    //ü���� ȸ���ϴ� ���
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return; // �̹� ����� ��� ȸ�� ����

        if (startingHealth <= health + newHealth) // �ִ� ü�º��� Ŭ ���
        {
            newHealth = startingHealth - health; // �ִ� ü������ ����
        }

        //if (startingHealth <= health + newHealth) 
        //{
        //    health = startingHealth; 
        //}

        health += newHealth; // ü�� ����
   
    }

    //���ó��
    public virtual void Die()
    {
        //onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (onDeath != null)
        {
            onDeath(); // ��� �̺�Ʈ �߻�
        }
        dead = true; // ��� ���·� ����
    }
}


