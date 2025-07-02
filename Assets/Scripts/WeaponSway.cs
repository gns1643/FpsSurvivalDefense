using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    //���� ��ġ
    private Vector3 originPos;

    //���� ��ġ
    private Vector3 currentPos;

    //Sway �Ѱ�
    [SerializeField]
    private Vector3 limitPos;

    //������ Sway �Ѱ�
    [SerializeField]
    private Vector3 fineSightLimitPos;

    //�ε巯�� ������ ����
    [SerializeField]
    private Vector3 smoothSway;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;

    void Start()
    {
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }
    void TrySway()
    {
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {//���콺�� �������� �� 
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }
    void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y), originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y), originPos.z);
        }
         
        transform.localPosition = currentPos;
    }

    void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
