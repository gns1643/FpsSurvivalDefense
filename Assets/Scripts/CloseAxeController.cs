using System.Collections;
using UnityEngine;

public abstract class CloseAxeController : MonoBehaviour
{
    

    [SerializeField]
    protected CloseWeapon currentCloseWeaponHand; //현재 Hand타입의 장착된 무기

    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; //레이캐스트에 닿은 오브젝트의 정보
    // Update is called once per frame
    
    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCorutine());
            }
        }
    }

    protected IEnumerator AttackCorutine()
    {
        isAttack = true;

        currentCloseWeaponHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeaponHand.attackDelayA);
        isSwing = true;

        //공격활성화 시점 
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeaponHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeaponHand.attackDelay
            - currentCloseWeaponHand.attackDelayA - currentCloseWeaponHand.attackDelayB);

        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();
    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeaponHand.range))
        {
            return true;
        }
        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {//무기 변경중이 아닐시 실행
            WeaponManager.currentWeapon.gameObject.SetActive(false); //기존 무기 비활성화
        }

        currentCloseWeaponHand = _closeWeapon; //무기 바꿈
        WeaponManager.currentWeapon = currentCloseWeaponHand.transform;
        WeaponManager.currentWeaponAnim = currentCloseWeaponHand.anim;

        currentCloseWeaponHand.transform.localPosition = Vector3.zero; //바꾼 무기 좌표 초기화
        currentCloseWeaponHand.gameObject.SetActive(true); //바꾼 무기 활성화
    }
}
