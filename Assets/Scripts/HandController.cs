using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = false;

    [SerializeField]
    private Hand currentHand; //현재 Hand타입의 장착된 무기

    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; //레이캐스트에 닿은 오브젝트의 정보
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

        //공격활성화 시점 
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
        {//무기 변경중이 아닐시 실행
            WeaponManager.currentWeapon.gameObject.SetActive(false); //기존 무기 비활성화
        }

        currentHand = _hand; //무기 바꿈
        WeaponManager.currentWeapon = currentHand.transform;
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero; //바꾼 무기 좌표 초기화
        currentHand.gameObject.SetActive(true); //바꾼 무기 활성화

        isActivate = true;
    }
}
