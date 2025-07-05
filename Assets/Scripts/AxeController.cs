using System.Collections;
using UnityEngine;

public class AxeController : CloseAxeController
{
    //활성화 여부
    public static bool isActivate = false;
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
