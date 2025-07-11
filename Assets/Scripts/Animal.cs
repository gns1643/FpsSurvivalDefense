using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName; //������ �̸�
    [SerializeField] protected int hp; //������ ü��

    [SerializeField] protected float walkSpeed; //������ �ȴ� �ӵ�
    [SerializeField] protected float runSpeed;//������ �ٴ� �ӵ�
    [SerializeField] protected float turningSpeed; //������ ȸ�� �ӵ�
    protected float applySpeed;

    protected Vector3 direction;

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

    [SerializeField] protected AudioClip[] sound_pig_Normal;
    [SerializeField] protected AudioClip sound_pig_Hurt;
    [SerializeField] protected AudioClip sound_pig_Dead;
    void Start()
    {
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
            Rotation();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }
    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
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
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }
    

    protected void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        Debug.Log("�ȱ�");
        anim.SetBool("Walking", isWalking);
        applySpeed = walkSpeed;
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
