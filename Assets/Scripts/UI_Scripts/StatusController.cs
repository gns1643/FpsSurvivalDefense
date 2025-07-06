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
        currentThirsty = thirsty;
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
    public void IncreaseHp(int _count)
    {
        if (currentHp + _count >= hp)
            currentHp = hp;
        else
            currentHp += _count;
    }
    public void DecreaseHp(int _count)
    {
        if(currentDp > 0)
        {
            DecreaseDp(_count);
            return;
        }
        currentHp -= _count;
        if(currentHp < 0)
        {
            Debug.Log("ĳ������ hp�� 0�� ��");
        }
    }
    public void IncreaseSp(int _count)
    {
        if (currentSp + _count >= sp)
            currentSp = sp;
        else
            currentSp += _count;
    }
    public void DecreaseSp(int _count)
    {
        currentSp -= _count;
        if (currentSp < 0)
        {
            Debug.Log("ĳ������ sp�� 0�� ��");
        }
    }

    public void IncreaseDp(int _count)
    {
        if (currentDp + _count >= dp)
            currentDp = dp;
        else
            currentDp += _count;
    }
    public void DecreaseDp(int _count)
    {
        currentDp -= _count;
        if (currentDp < 0)
        {
            Debug.Log("ĳ������ dp�� 0�� ��");
        }
    }
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count >= hungry)
            currentHungry = hungry;
        else
            currentHungry += _count;
    }
    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count >= thirsty)
            currentThirsty = thirsty;
        else
            currentThirsty += _count;
    }
    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
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
    public int GetCurrentSp()
    {
        return currentSp;
    }
}
