using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private float hp; // 바위의 체력

    [SerializeField]
    private float destroyTime; //파편 소멸 시간

    [SerializeField]
    private SphereCollider col; //구체 콜라이더

    //필요한 오브젝트
    [SerializeField]
    private GameObject go_rock; //일반 바위

    [SerializeField]
    private GameObject go_debris; // 깨진바위

    [SerializeField]
    private GameObject go_effect_prefab; //채굴 이펙트

    //필요한 사운드 이름
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;
    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        var clone = Instantiate(go_effect_prefab, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;    
        if (hp <= 0)
        {
            Destruction();
        }
    }
    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); 
    }
}
