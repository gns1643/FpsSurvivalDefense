using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = false;

    [SerializeField]
    private Hand currentHand; //���� HandŸ���� ������ ����

    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; //����ĳ��Ʈ�� ���� ������Ʈ�� ����
    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            TryAttack();
        }
    }
    void TryAttack()
    {
        if (Input.GetButton("Fire1")){
            if (!isAttack)
            {
                StartCoroutine(AttackCorutine());
            }
        }
    }

    IEnumerator AttackCorutine()
    {
        isAttack = true;

        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //����Ȱ��ȭ ���� 
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay 
            - currentHand.attackDelayA - currentHand.attackDelayB);

        isAttack = false;
    }

    IEnumerator HitCoroutine()
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
    bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {//���� �������� �ƴҽ� ����
            WeaponManager.currentWeapon.gameObject.SetActive(false); //���� ���� ��Ȱ��ȭ
        }

        currentHand = _hand; //���� �ٲ�
        WeaponManager.currentWeapon = currentHand.transform;
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero; //�ٲ� ���� ��ǥ �ʱ�ȭ
        currentHand.gameObject.SetActive(true); //�ٲ� ���� Ȱ��ȭ

        isActivate = true;
    }
}
