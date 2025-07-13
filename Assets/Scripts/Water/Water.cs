using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; //¹°¼Ó Áß·Â
    private float originDrag; //¿ø·¡ Áß·Â

    [SerializeField] private Color waterColor; //¹° ¼Ó »ö±ò
    [SerializeField] private float waterFogDensity; //¹° ¼Ó Å¹ÇÔ Á¤µµ

    [SerializeField] private Color waterNightColor; //¹° ¼Ó »ö±ò(¹ã)
    [SerializeField] private float waterNightFogDensity; //¹° ¼Ó Å¹ÇÔ Á¤µµ(¹ã)

    private Color originColor;
    private float originFogDensity;

    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;

    [SerializeField] private string sound_Water_Out;
    [SerializeField] private string sound_Water_In;
    [SerializeField] private string sound_Water_Breath;

    [SerializeField] private float breathTime;
    private float currentTime;


    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;
        originDrag = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isWater)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= breathTime)
            {
                SoundManager.instance.PlaySE(sound_Water_Breath);
                currentTime = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    void GetWater(Collider _player)
    {
        SoundManager.instance.PlaySE(sound_Water_In);
        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().linearDamping = waterDrag;
        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    void GetOutWater(Collider _player)
    {
        if (GameManager.isWater)
        {
            SoundManager.instance.PlaySE(sound_Water_Out);
            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().linearDamping = originDrag;

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
