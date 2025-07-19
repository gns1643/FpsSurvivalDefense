using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; //플레이어의 움직임 제어

    public static bool isOpenInventory = false; //인벤토리 활성화 여부

    public static bool isOpenCraftMenu = false; //건축 메뉴 활성화 여부

    public static bool isPause = false; // 일시정지 창 활성화 여부

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
