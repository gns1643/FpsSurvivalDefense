using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private Hand currentHand; //���� HandŸ���� ������ ����

    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; //����ĳ��Ʈ�� ���� ������Ʈ�� ����
    // Update is called once per frame
    void Update()
    {
        TryAttack();    }
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
}
