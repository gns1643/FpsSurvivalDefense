using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName; //������ �̸�
    [SerializeField] private int hp; //������ ü��

    [SerializeField] private float walkSpeed; //������ �ȴ� �ӵ�

    private Vector3 direction;

    //���� ����
    private bool isWalking; //�Ȱ� �ִ°�?
    private bool isAction; //�ൿ ���ΰ�?

    [SerializeField] private float walkTime; //�ȴ� �ð�
    [SerializeField] private float waitTime; //��� �ð�
    private float currentTime;

    //�ʿ��� ������Ʈ
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        ElapseTime();
        Move();
        Rotation();
    }

    void Move()
    {
        if (isWalking)
        {
            rigid.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }
    void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
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
        isWalking = false; isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }
    void RandomAction()
    {
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
    }
}
