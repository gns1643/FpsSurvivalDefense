using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // 근접무기 이름

    //웨폰 유형
    public bool isAxe;
    public bool isHand;
    public bool isPickaxe;

    public float range; // 공격범위
    public int damege; // 공격력
    public float workSpeed; //작업속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA;  // 공격 활성화 시점
    public float attackDelayB; // 공격 비활성화 시점

    public Animator anim;

}
