using UnityEngine;

public class DayandNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; //게임세계의 100초 = 현실세계의 1초

    private bool isNight = false;

    [SerializeField] private float fogDensityCalc; //증감량 비율

    [SerializeField] private float nightFogDensity; //밤상태의 fog밀도
    private float dayFogDensity; //낮상태의 fog밀도
    private float currentForDensity; //현재 fog 밀도
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if(transform.eulerAngles.x >= 170)
        {//태양이 지고있으면
            isNight = true;
        }
        if(transform.eulerAngles.x <= 10)
        {//태양이 뜨고있으면
            isNight = false;
        }

        if (isNight)
        {
            if(currentForDensity <= nightFogDensity)
            {
                currentForDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentForDensity;
            }
        }
        else
        {
            if (currentForDensity >= dayFogDensity)
            {
                currentForDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentForDensity;
            }
        }
    }
}
