using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    //다른 클래스에서 접근 가능한 static 변수
    //무기 중복 교체 방지 (교체시 true)
    public static bool isChangeWeapon;

    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    
    [SerializeField]
    private string currentWeaponType; //현재 무기의 타입
    [SerializeField]
    private float changeWeaponDelayTime; //무기교체 딜레이
    [SerializeField]
    private float changeWeaponEndDelayTime; // 무기 교체가 완전히 끝난 시점

    //모든 무기 종류 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    //각 무기에 쉽게 접근할 수 있도록 딕셔너리 객체 사용
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();


    

    //필요한 컴포넌트
    //한번에 한 종류만 활성화
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;


    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {//무기 교체 : 맨손
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손")); 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {// 무기 교체: 서브머신건
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CanclePreWeaponAction(); // 이전 무기 해제
        WeaponChange(_type, _name); //무기 교체

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancleFinesight(); //정조준 상태 해제
                theGunController.CancleReload(); //재장전 상태 해제
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }
    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
            theHandController.HandChange(handDictionary[_name]);
    }
}
