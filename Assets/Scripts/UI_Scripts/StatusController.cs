using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //ü��
    [SerializeField]
    private int hp;
    private int currentHp;

    //���¹̳�
    [SerializeField]
    private int sp;
    private int currentSp;
    //���¹̳� ȸ�� �ӵ�
    [SerializeField]
    private int spIncreaseSpeed;
    //���¹̳� ȸ�� ������
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;
    //���¹̳� ���� ����
    private bool spUsed;

    //����
    [SerializeField]
    private int dp;
    private int currentDp;

    //�����
    [SerializeField]
    private int hungry;
    private int currentHungry;

    //����� ���� �ӵ�
    [SerializeField]
    private int hungryDescreaseTime;
    private int currentHungryDescreaseTime;

    //�񸶸�
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    //�񸶸� ���� �ӵ�
    [SerializeField]
    private int thirstyDescreaseTime;
    private int currentThirstyDescreaseTime;

    //������
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //�ʿ��� �̹���
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THRISTY = 4, SATISFY = 5;

    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        GaugeUpdate();
        SpRechargeTime();
        SpRecover();
    }
    void SpRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }
    void SpRecover()
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    void Hungry()
    {
        if(currentHungry > 0)
        {
            if(currentHungryDescreaseTime <= hungryDescreaseTime)
                currentHungryDescreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDescreaseTime = 0;
            }
                
        }
        else
        {
            Debug.Log("����� ��ġ�� 0�� �Ǿ���");
        }
    }
    void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDescreaseTime <= thirstyDescreaseTime)
                currentThirstyDescreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDescreaseTime = 0;
            }

        }
        else
        {
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ���");
        }
    }
    void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THRISTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;
        if(currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
        {
            currentSp = 0;
        }

    }
}
