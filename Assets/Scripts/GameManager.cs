using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; //플레이어의 움직임 제어

    public static bool isOpenInventory = false; //인벤토리 활성화 여부

    public static bool isOpenCraftMenu = false; //건축 메뉴 활성화 여부
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
