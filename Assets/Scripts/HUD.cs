using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    //�ʿ��� ��� HUD ȣ����
    [SerializeField]
    private GameObject go_BulletHUD;


    //�Ѿ˰���
    [SerializeField]
    private TMP_Text[] text_Bullet;
    
    void Update()
    {
        CheakBullet();   
    }

    void CheakBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
