using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;
    private int currentHp;

    //스태미나
    [SerializeField]
    private int sp;
    private int currentSp;
    //스태미나 회복 속도
    [SerializeField]
    private int spIncreaseSpeed;
    //스태미나 회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;
    //스태미나 감소 여부
    private bool spUsed;

    //방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    //배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    //배고픔 감소 속도
    [SerializeField]
    private int hungryDescreaseTime;
    private int currentHungryDescreaseTime;

    //목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    //목마름 감소 속도
    [SerializeField]
    private int thirstyDescreaseTime;
    private int currentThirstyDescreaseTime;

    //만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //필요한 이미지
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
            Debug.Log("배고픔 수치가 0이 되었음");
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
            Debug.Log("목마름 수치가 0이 되었음");
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
