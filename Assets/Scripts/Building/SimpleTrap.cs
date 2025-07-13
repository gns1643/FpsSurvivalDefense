using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;
    [SerializeField] private GameObject go_Meat;

    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource theAudio;
    [SerializeField] AudioClip sound_Activate;

    [SerializeField] private StatusController theStausController;
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated){
            if (other.transform.tag != "Untagged")
            {// ������ �ƴҽÿ� �ߵ�
                
                isActivated = true;
                theAudio.clip = sound_Activate;
                theAudio.Play();
                Destroy(go_Meat);

                for (int i = 0; i < rigid.Length; i++)
                {//���� �ߵ��� �� ������Ʈ�� rigidbody ����
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false;
                }

                if(other.transform.name == "Player")
                {
                    theStausController.DecreaseHp(damage);
                }
            }
    }   }
}
