using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // �������� �̸�

    //���� ����
    public bool isAxe;
    public bool isHand;
    public bool isPickaxe;

    public float range; // ���ݹ���
    public int damege; // ���ݷ�
    public float workSpeed; //�۾��ӵ�
    public float attackDelay; // ���� ������
    public float attackDelayA;  // ���� Ȱ��ȭ ����
    public float attackDelayB; // ���� ��Ȱ��ȭ ����

    public Animator anim;

}
