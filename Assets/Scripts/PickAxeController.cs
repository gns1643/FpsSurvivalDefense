using System.Collections;
using UnityEngine;

public class PickAxeController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = true;
    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeaponHand.transform;
        WeaponManager.currentWeaponAnim = currentCloseWeaponHand.anim;
    }
    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                else if(hitInfo.transform.tag == "NPC")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<Pig>().Damage(currentCloseWeaponHand.damege, transform.position);
                }
                    isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
