using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; //동물의 이름
    [SerializeField] protected int hp; //동물의 체력

    [SerializeField] protected float walkSpeed; //동물의 걷는 속도
    [SerializeField] protected float runSpeed;//동물의 뛰는 속도

    protected Vector3 destination;

    //상태 변수
    protected bool isWalking; //걷고 있는가?
    protected bool isAction; //행동 중인가?
    protected bool isRunning; //뛰는 중인가?
    protected bool isDead; //죽었나?

    [SerializeField] protected float walkTime; //걷는 시간
    [SerializeField] protected float waitTime; //대기 시간
    [SerializeField] protected float runTime; //뛰는 시간
    protected float currentTime;

    //필요한 컴포넌트
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
        Debug.Log("걷기");
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
        int _random = Random.Range(0, 3); //일상 사운드 3개
        PlaySE(sound_pig_Normal[_random]);
    }
    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
