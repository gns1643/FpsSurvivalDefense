using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //���̸�
    public float range; //��Ÿ�
    public float accuracy; //��Ȯ��
    public float fireRate; //����ӵ�
    public float reloadTime; //�������ӵ�

    public float damage; //������

    public float reloadBulletCount; //�Ѿ� ������ ����
    public float currentBulletCount; //���� ������ �Ѿ� ����
    public float maxBulletCount; //�ִ� ���� ���� �Ѿ� ����
    public float carryBulletCount; //���� ������ �Ѿ� ����

    public float retroActionForce; //�ݵ� ����
    public float retroActionFineSightForce; //������ �� �ݵ� ����

    public Vector3 fineInsightOriginPos; //������ �� ��ġ
    public Animator anim;
    public ParticleSystem muzzleFlash;

    public AudioClip fireSound;


}
