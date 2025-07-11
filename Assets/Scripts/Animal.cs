using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; //������ �̸�
    [SerializeField] protected int hp; //������ ü��

    [SerializeField] protected float walkSpeed; //������ �ȴ� �ӵ�
    [SerializeField] protected float runSpeed;//������ �ٴ� �ӵ�

    protected Vector3 destination;

    //���� ����
    protected bool isWalking; //�Ȱ� �ִ°�?
    protected bool isAction; //�ൿ ���ΰ�?
    protected bool isRunning; //�ٴ� ���ΰ�?
    protected bool isDead; //�׾���?

    [SerializeField] protected float walkTime; //�ȴ� �ð�
    [SerializeField] protected float waitTime; //��� �ð�
    [SerializeField] protected float runTime; //�ٴ� �ð�
    protected float currentTime;

    //�ʿ��� ������Ʈ
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;

    protected AudioSource theAudio;
    protected NavMeshAgent nav;
    [SerializeField] protected AudioClip[] sound_pig_Normal;
    [SerializeField] protected AudioClip sound_pig_Hurt;
    [SerializeField] protected AudioClip sound_pig_Dead;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (!isDead)
        {
            ElapseTime();
            Move();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position + destination * 5f);
        }
    }
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                Reset1();
            }
        }
    }
    protected virtual void Reset1()
    {
        isWalking = false; isAction = true; isRunning = false;
        nav.speed = walkSpeed;
        nav.ResetPath();
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }
    

    protected void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        Debug.Log("�ȱ�");
        anim.SetBool("Walking", isWalking);
        nav.speed = walkSpeed;
    }
    

    public virtual void Damage(int _dmg, Vector3 _targetPos)
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
        }

    }
    protected void Dead()
    {
        isDead = true;
        PlaySE(sound_pig_Dead);
        isWalking = false; isRunning = false;
        anim.SetTrigger("Dead");
    }
    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); //�ϻ� ���� 3��
        PlaySE(sound_pig_Normal[_random]);
    }
    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
