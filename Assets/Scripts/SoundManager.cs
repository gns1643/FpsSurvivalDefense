using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour
{
    // 싱글턴화
    #region singleton
    static public SoundManager instance;
    private void Awake() // 객체 생성시 최초 1회 실행
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬으로 넘어가도 삭제가 X
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
            {// _name과 일치하는 name이 있다면
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {//재생중이 아닌 오디오소스의 클립을 원하는 오디오 클립으로 대체 후 재생
                        playSoundName[j] = _name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 오디오 소스가 사용 중 ");
                return;
            }
        }
        Debug.Log(_name + "사운드가 사운드매니저에 등록되지 않음");
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
        Debug.Log("재생중인" + _name + "오디오 클립이 없음");
    }
}
