using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //���� �̸�
    public AudioClip clip; //��
}

public class SoundManager : MonoBehaviour
{
    // �̱���ȭ
    #region singleton
    static public SoundManager instance;
    private void Awake() // ��ü ������ ���� 1ȸ ����
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �ٸ� ������ �Ѿ�� ������ X
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton
    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;
     
    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSound;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (effectSounds[i].name == _name) 
            {// _name�� ��ġ�ϴ� name�� �ִٸ�
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {//������� �ƴ� ������ҽ��� Ŭ���� ���ϴ� ����� Ŭ������ ��ü �� ���
                        playSoundName[j] = _name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ���� ����� �ҽ��� ��� �� ");
                return;
            }
        }
        Debug.Log(_name + "���尡 ����Ŵ����� ��ϵ��� ����");
    }
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("�������" + _name + "����� Ŭ���� ����");
    }
}
