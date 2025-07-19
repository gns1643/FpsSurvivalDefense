using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; //�÷��̾��� ������ ����

    public static bool isOpenInventory = false; //�κ��丮 Ȱ��ȭ ����

    public static bool isOpenCraftMenu = false; //���� �޴� Ȱ��ȭ ����

    public static bool isPause = false; // �Ͻ����� â Ȱ��ȭ ����

    public static bool isNight = false;

    public static bool isWater = false;

    private bool flag = false;

    private WeaponManager theWM;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        theWM = FindFirstObjectByType<WeaponManager>();
    }
    void Update()
    {
        if (isOpenInventory || isOpenCraftMenu || isPause) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }
        if (isWater){
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWM.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                theWM.WeaponOut();
                flag = false;
            }
            
        }
    }
}
