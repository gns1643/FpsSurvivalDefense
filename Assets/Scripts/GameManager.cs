using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; //�÷��̾��� ������ ����

    public static bool isOpenInventory = false; //�κ��丮 Ȱ��ȭ ����

    public static bool isOpenCraftMenu = false; //���� �޴� Ȱ��ȭ ����

    public static bool isNight = false;

    public static bool isWater = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (isOpenInventory || isOpenCraftMenu) {
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
           
    }
}
