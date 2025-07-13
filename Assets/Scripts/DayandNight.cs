using UnityEngine;

public class DayandNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecond; //���Ӽ����� 100�� = ���Ǽ����� 1��


    [SerializeField] private float fogDensityCalc; //������ ����

    [SerializeField] private float nightFogDensity; //������� fog�е�
    private float dayFogDensity; //�������� fog�е�
    private float currentForDensity; //���� fog �е�
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if(transform.eulerAngles.x >= 170)
        {//�¾��� ����������
            GameManager.isNight = true;
        }
        if(transform.eulerAngles.x >= 340)
        {//�¾��� �߰�������
            GameManager.isNight = false;
        }

        if (GameManager.isNight)
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
