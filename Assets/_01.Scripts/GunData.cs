using UnityEngine;

[CreateAssetMenuAttribute(menuName = "ScriptableObject/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    
    public float damage = 25;
    
    [Header("초기 총 탄알 수")]
    public int startAmmoRemain = 100;
    [Tooltip("한탄창 수")]
    public int magCapacity = 25;
    
    public float timeBetFire = 0.12f;
    public float reloadTime = 1.8f;
    
}
