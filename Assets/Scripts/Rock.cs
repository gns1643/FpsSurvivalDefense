using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private float hp; // ������ ü��

    [SerializeField]
    private float destroyTime; //���� �Ҹ� �ð�

    [SerializeField]
    private SphereCollider col; //��ü �ݶ��̴�

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_rock; //�Ϲ� ����

    [SerializeField]
    private GameObject go_debris; // ��������

    [SerializeField]
    private GameObject go_effect_prefab; //ä�� ����Ʈ

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;
    public void Mining()
    {
        audioSource.clip = effect_sound;
        audioSource.Play();
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
        audioSource.clip = effect_sound2;
        audioSource.Play();
        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); 
    }
}
