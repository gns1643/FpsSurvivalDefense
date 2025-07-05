using System.Collections;
using UnityEngine;

public abstract class CloseAxeController : MonoBehaviour
{
    

    [SerializeField]
    protected CloseWeapon currentCloseWeaponHand; //���� HandŸ���� ������ ����

    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; //����ĳ��Ʈ�� ���� ������Ʈ�� ����
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

        //����Ȱ��ȭ ���� 
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
        {//���� �������� �ƴҽ� ����
            WeaponManager.currentWeapon.gameObject.SetActive(false); //���� ���� ��Ȱ��ȭ
        }

        currentCloseWeaponHand = _closeWeapon; //���� �ٲ�
        WeaponManager.currentWeapon = currentCloseWeaponHand.transform;
        WeaponManager.currentWeaponAnim = currentCloseWeaponHand.anim;

        currentCloseWeaponHand.transform.localPosition = Vector3.zero; //�ٲ� ���� ��ǥ �ʱ�ȭ
        currentCloseWeaponHand.gameObject.SetActive(true); //�ٲ� ���� Ȱ��ȭ
    }
}
