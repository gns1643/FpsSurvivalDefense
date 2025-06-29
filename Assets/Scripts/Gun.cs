using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //총이름
    public float range; //사거리
    public float accuracy; //정확도
    public float fireRate; //연사속도
    public float reloadTime; //재장전속도

    public float damage; //데미지

    public float reloadBulletCount; //총알 재장전 개수
    public float currentBulletCount; //현재 장전된 총알 개수
    public float maxBulletCount; //최대 소유 가능 총알 개수
    public float carryBulletCount; //현재 소유한 총알 개수

    public float retroActionForce; //반동 세기
    public float retroActionFineSightForce; //정조준 시 반동 세기

    public Vector3 fineInsightOriginPos; //정조준 시 위치
    public Animator anim;
    public ParticleSystem muzzleFlash;

    public AudioClip fireSound;


}
