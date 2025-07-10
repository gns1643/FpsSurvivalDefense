using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; //������ �̸�
    [SerializeField] private int hp; //������ ü��

    [SerializeField] private float walkSpeed; //������ �ȴ� �ӵ�
    [SerializeField] private float runSpeed;//������ �ٴ� �ӵ�
    private float applySpeed;

    private Vector3 direction;

    //���� ����
    private bool isWalking; //�Ȱ� �ִ°�?
    private bool isAction; //�ൿ ���ΰ�?
    private bool isRunning; //�ٴ� ���ΰ�?
    private bool isDead; //�׾���?

    [SerializeField] private float walkTime; //�ȴ� �ð�
    [SerializeField] private float waitTime; //��� �ð�
    [SerializeField] private float runTime; //�ٴ� �ð�
    private float currentTime;

    //�ʿ��� ������Ʈ
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    private AudioSource theAudio;

    [SerializeField] private AudioClip[] sound_pig_Normal;
    [SerializeField] private AudioClip sound_pig_Hurt;
    [SerializeField] private AudioClip sound_pig_Dead;
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            ElapseTime();
            Move();
            Rotation();
        }
    }

    void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }
    void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                Reset1();
            }
        }
    }
    private void Reset1()
    {
        isWalking = false; isAction = true; isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }
    void RandomAction()
    {
        RandomSound();
        isAction = true;
        int _random = Random.Range(0, 4); //���, �Ա�, ã��, �ȱ�

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
            
    }
    void Wait()
    {
        currentTime = waitTime;
        Debug.Log("���");
    }
    void Eat()
    {
        currentTime = waitTime;
        Debug.Log("�Ա�");
        anim.SetTrigger("Eat");
    }
    void Peek()
    {
        currentTime = waitTime;
        Debug.Log("ã��");
        anim.SetTrigger("Peek");
    }
    void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        Debug.Log("�ȱ�");
        anim.SetBool("Walking", isWalking);
        applySpeed = walkSpeed;
    }
    public void Run(Vector3 _targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;
            if (hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(sound_pig_Hurt);
            anim.SetTrigger("Hurt");
            Run(_targetPos);
        }
        
    }
    void Dead()
    {
        isDead = true;
        PlaySE(sound_pig_Dead);
        isWalking = false; isRunning = false;
        anim.SetTrigger("Dead");
    }
    private void RandomSound()
    {
        int _random = Random.Range(0, 3); //�ϻ� ���� 3��
        PlaySE(sound_pig_Normal[_random]);
    }
    private void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
