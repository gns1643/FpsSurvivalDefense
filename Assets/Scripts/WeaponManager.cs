using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    //�ٸ� Ŭ�������� ���� ������ static ����
    //���� �ߺ� ��ü ���� (��ü�� true)
    public static bool isChangeWeapon;

    // ���� ����� ���� ������ �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    
    [SerializeField]
    private string currentWeaponType; //���� ������ Ÿ��
    [SerializeField]
    private float changeWeaponDelayTime; //���ⱳü ������
    [SerializeField]
    private float changeWeaponEndDelayTime; // ���� ��ü�� ������ ���� ����

    //��� ���� ���� ����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    //�� ���⿡ ���� ������ �� �ֵ��� ��ųʸ� ��ü ���
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();



    //�ʿ��� ������Ʈ
    //�ѹ��� �� ������ Ȱ��ȭ
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickAxeController thePickaxeController;

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {//���� ��ü : �Ǽ�
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�")); 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {// ���� ��ü: ����ӽŰ�
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {// ���� ��ü: ����
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {// ���� ��ü: ���
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CanclePreWeaponAction(); // ���� ���� ����
        WeaponChange(_type, _name); //���� ��ü

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancleFinesight(); //������ ���� ����
                theGunController.CancleReload(); //������ ���� ����
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickAxeController.isActivate = false;
                break;
        }
    }
    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
            theHandController.CloseWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
    }
}
